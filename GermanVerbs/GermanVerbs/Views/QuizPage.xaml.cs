using GermanVerbs.Data;
using GermanVerbs.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GermanVerbs.Views
{
    public partial class QuizPage : ContentPage
    {
        static string Answer = "";
        static Conjugation currentVerb;

        Color correctAnswerColor;
        Color wrongAnswerColor;

        public QuizPage()
        {
            InitializeComponent();

            VerbEntry.Conjugations = ConjugationData.Conjugations;

            wrongAnswerColor = Color.FromHex("f64336");
            correctAnswerColor = Color.FromHex("3dca42");
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
                currentVerb = verb;
            }
            else
            {
                if (VerbEntry.Query == "")
                {
                    await DisplayAlert("Ошибка", "Нужно ввести глагол", "ОК");
                    return;
                }

                currentVerb = await Task.Run(() => ConjugationData.FindOne(VerbEntry.Query)) ?? await ConjugationParser.GetConjugation(VerbEntry.Query);

                if (currentVerb == null)
                {
                    await DisplayAlert("Ошибка", "Не могу найти глагол", ":(");
                    return;
                }
            }

            InfinitiveLabel.Text = currentVerb._id;
            VerbEntry.Conjugations = ConjugationData.Conjugations;
            CreateQuiz();
        }


        void CreateQuiz()
        {
            var selected = propertySettings.SelectedItems;
            var all = SettingsData.Settings.ToList();

            KeyValuePair<string, PropertyInfo> randProperty =
                selected.Count == 0 ?
                all[App.Randomizer.Next(0, all.Count)] :
                (KeyValuePair<string, PropertyInfo>)selected[App.Randomizer.Next(0, selected.Count)];

            TenseLabel.Text = randProperty.Key;

            if (randProperty.Value.PropertyType == typeof(string))
            {
                Answer = (string)randProperty.Value.GetValue(currentVerb);
            }
            else
            {
                var dict = (Dictionary<string, string>)randProperty.Value.GetValue(currentVerb);
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
                    AnswerEntry.TextColor = wrongAnswerColor;
                }
                else
                {
                    AnswerEntry.TextColor = correctAnswerColor;
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

        private void NextRandButton_Clicked(object sender, EventArgs e)
        {
            var checkedVerbs = ConjugationData.GetActive();
            var checkedVerbsLength = checkedVerbs.Count;

            if (checkedVerbsLength == 0)
            {
                return;
            }

            currentVerb = checkedVerbs[App.Randomizer.Next(0, checkedVerbsLength)];
            InfinitiveLabel.Text = currentVerb._id;
            NextButton_Clicked(this, EventArgs.Empty);
        }

        protected override void OnDisappearing()
        {
            ConjugationData.Sort();
            base.OnDisappearing();
        }
    }
}