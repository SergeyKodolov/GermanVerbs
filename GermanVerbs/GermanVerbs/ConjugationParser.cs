using HtmlAgilityPack;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GermanVerbs
{
    public static class ConjugationParser
    {
        public static async Task<Conjugation> ParseFromHtmlDoc(HtmlDocument conjugateHtmlDoc, HtmlDocument translateHtmlDoc)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var node = translateHtmlDoc.DocumentNode
                        .SelectSingleNode("//a[@class='translation ltr dict v']");
                    var translation = node.InnerText.Trim();

                    node = conjugateHtmlDoc.DocumentNode
                        .SelectSingleNode("//a[@class='targetted-word-transl']");
                    var invinitive = node.InnerText.Trim();
                
                    Dictionary<string, string> presentIndicative = GetTense(conjugateHtmlDoc, "Indikativ Präsens");
                    Dictionary<string, string> perfectIndicative = GetTense(conjugateHtmlDoc, "Indikativ Perfekt");
                    Dictionary<string, string> presentImperative = GetTense(conjugateHtmlDoc, "Imperativ Präsens");

                    return new Conjugation()
                    {
                        Translation = translation,
                        Infinitive = invinitive,
                        PresentIndicative = presentIndicative,
                        PerfectIndicative = perfectIndicative,
                        PresentImperative = presentImperative
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
