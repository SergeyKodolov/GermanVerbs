using HtmlAgilityPack;
using LiteDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GermanVerbs
{
    public partial class QuizPage : ContentPage
    {
        static string Answer = "";

        static LiteDatabase db = new LiteDatabase(Constants.DatabasePath);
        static LiteCollection<Conjugation> conjugations = (LiteCollection<Conjugation>)db.GetCollection<Conjugation>();

        public static Conjugation CurrentVerb { get; set; }

        public QuizPage()
        {
            InitializeComponent();
            VerbEntry.Conjugations = conjugations.FindAll().ToList();
        }

        async void VerbEntry_Search(object sender, Conjugation verb)
        {
            InfinitiveLabel.Text = "";
            TenseLabel.Text = "";
            Answer = "";
            AnswerEntry.Text = "";
            PronounLabel.Text = "";
            NextButton.IsEnabled = false;

            if (verb != null)
            {
                CurrentVerb = verb;
            }
            else
            {
                if (VerbEntry.Query == "")
                {
                    await DisplayAlert("Ошибка", "Нужно ввести глагол", "ОК");
                    return;
                }

                CurrentVerb = conjugations.FindOne(x => x.Infinitive == VerbEntry.Query) ?? await GetConjugation();
                
                if (CurrentVerb == null)
                {
                    await DisplayAlert("Ошибка", "Не могу найти глагол", ":(");
                    VerbEntry.Query = "";
                    return;
                }
            }

            InfinitiveLabel.Text = CurrentVerb.Infinitive;
            VerbEntry.Query = "";
            CreateQuiz();
        }

        private async Task<Conjugation> GetConjugation()
        {
            var conjugateUrl = $"https://glagol.reverso.net/спряжение-немецкий-глагол-{VerbEntry.Query}.html";
            var translateUrl = $"https://context.reverso.net/перевод/немецкий-русский/{VerbEntry.Query}";
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
            var newConjugation = await ConjugationParser.ParseFromHtmlDoc(conjugateDoc, translateDoc);

            if (newConjugation != null)
            {
                await Task.Run(() => conjugations.Insert(newConjugation));
                await Task.Run(() => VerbEntry.Conjugations = conjugations.FindAll().ToList());
            }

            return newConjugation;
        }

        void CreateQuiz()
        {
            var propertyList = CurrentVerb.GetType().GetProperties().ToList();
            var randProperty = propertyList[App.Randomizer.Next(1, propertyList.Count)];

            var descriptionAtribute = (DescriptionAttribute)randProperty.GetCustomAttributes(typeof(DescriptionAttribute), false)[0];
            TenseLabel.Text = descriptionAtribute.Description;

            if (randProperty.PropertyType == typeof(string))
            {
                Answer = (string)randProperty.GetValue(CurrentVerb);
            }
            else
            {
                var dict = (Dictionary<string, string>)randProperty.GetValue(CurrentVerb);
                var pronounList = dict.Keys.ToList();
                var randPronoun = pronounList[App.Randomizer.Next(0, pronounList.Count)];
                PronounLabel.Text = randPronoun;
                Answer = dict[randPronoun];
            }
        }

        private void AnswerEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Answer != "")
                if (AnswerEntry.Text != Answer)
                {
                    AnswerEntry.TextColor = Color.Red;
                }
                else
                {
                    AnswerEntry.TextColor = Color.Green;
                    NextButton.IsEnabled = true;
                }
        }

        private void NextButton_Clicked(object sender, EventArgs e)
        {
            TenseLabel.Text = "";
            Answer = "";
            AnswerEntry.Text = "";
            PronounLabel.Text = "";
            NextButton.IsEnabled = false;
            CreateQuiz();
        }

        private void TipButton_Clicked(object sender, EventArgs e)
        {
            if (Answer != "" && AnswerEntry.Text != Answer)
            {
                var ansLength = AnswerEntry.Text.Length;

                if (ansLength == 0)
                {
                    AnswerEntry.Text = Answer[0].ToString();
                    return;
                }

                // if word has mistake
                for (int i = 0; i < ansLength; i++)
                {
                    if (AnswerEntry.Text[i] != Answer[i] || i == Answer.Length - 1)
                    {
                        AnswerEntry.Text = Answer.Substring(0, i + 1);
                        return;
                    }
                }

                // if word is incomplete
                AnswerEntry.Text = Answer.Substring(0, ansLength + 1);
            }
        }

        private void TipButton_LongPressed(object sender, EventArgs e)
        {
            AnswerEntry.Text = Answer;
        }
    }
}