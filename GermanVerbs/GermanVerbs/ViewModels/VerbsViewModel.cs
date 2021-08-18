using GermanVerbs.Data;
using GermanVerbs.Models;
using System.Windows.Input;
using Xamarin.Forms;

namespace GermanVerbs.ViewModels
{
    class VerbsViewModel
    {

        public ICommand DeleteCommand => new Command<Conjugation>(RemoveMonkey);

        public VerbsViewModel()
        {

        }

        void RemoveMonkey(Conjugation verb)
        {
            if (ConjugationData.Conjugations.Contains(verb))
            {
                ConjugationData.Conjugations.Remove(verb);
            }
        }
    }
}
