using GermanVerbs.Models;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GermanVerbs.Data
{
    public static class ConjugationParser
    {
        public static async Task<Conjugation> GetConjugation(string verb)
        {
            var conjugateUrl = $"https://glagol.reverso.net/спряжение-немецкий-глагол-{verb}.html";
            var translateUrl = $"https://context.reverso.net/перевод/немецкий-русский/{verb}";
            var web = new HtmlWeb();
            HtmlDocument conjugateDoc, translateDoc;
            try
            {
                conjugateDoc = await web.LoadFromWebAsync(conjugateUrl);
                translateDoc = await web.LoadFromWebAsync(translateUrl);
            }
            catch
            {
                return null;
            }
            var newConjugation = await ParseFromHtmlDoc(verb, conjugateDoc, translateDoc);

            if (newConjugation != null)
            {
                await Task.Run(() => ConjugationData.Insert(newConjugation));
            }

            return newConjugation;
        }

        public static async Task<Conjugation> ParseFromHtmlDoc(string verb, HtmlDocument conjugateHtmlDoc, HtmlDocument translateHtmlDoc)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var invinitive = verb;

                    var node = translateHtmlDoc.DocumentNode.SelectSingleNode("//a[@class='translation ltr dict v']");
                    var translation = node.InnerText.Trim();                    

                    Dictionary<string, string> presentIndicative = GetTense(conjugateHtmlDoc, "Indikativ Präsens");
                    Dictionary<string, string> perfectIndicative = GetTense(conjugateHtmlDoc, "Indikativ Perfekt");
                    Dictionary<string, string> presentImperative = GetTense(conjugateHtmlDoc, "Imperativ Präsens");

                    return new Conjugation()
                    {
                        _id = invinitive,
                        Translation = translation,
                        PresentIndicative = presentIndicative,
                        PerfectIndicative = perfectIndicative,
                        PresentImperative = presentImperative,
                        IsActive = true
                    };
                });
            }
            catch
            {
                return null;
            }
        }

        private static Dictionary<string, string> GetTense(HtmlDocument conjugateHtmlDoc, string mobileTitle)
        {
            var nodeCollection = conjugateHtmlDoc.DocumentNode
                .SelectNodes($"//div[@mobile-title='{mobileTitle}']/ul/li");

            var tenseDict = new Dictionary<string, string>();

            foreach (var n in nodeCollection)
            {
                var pronoun = n.InnerText.Substring(0, n.InnerText.IndexOf(' '));
                var verbForm = n.InnerText.Substring(n.InnerText.IndexOf(' ') + 1);

                if (mobileTitle.Contains("Imperativ"))
                {
                    (pronoun, verbForm) = (verbForm, pronoun);
                    pronoun = (n.Attributes.Contains("v") && pronoun == "(du)" ? "opt. " : "") + pronoun;
                }

                tenseDict[pronoun] = verbForm;
            }

            return tenseDict;
        }
    }
}
