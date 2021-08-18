using GermanVerbs.Data;
using GermanVerbs.Models;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace GermanVerbs.ViewModels
{
    class VerbsViewModel
    {
        public VerbsViewModel() { }

        public ICommand DeleteCommand => new Command<Conjugation>(RemoveVerb);
        public ICommand TapCommand => new Command<CheckBox>(Tap);

        private void Tap(CheckBox checkBox)
        {
           checkBox.IsChecked = !checkBox.IsChecked;
        }

        void RemoveVerb(Conjugation verb)
        {
            ConjugationData.Remove(verb);
        }
    }
}
