using GermanVerbs.Data;
using GermanVerbs.Models;
using System.Windows.Input;
using Xamarin.Forms;

namespace GermanVerbs.ViewModels
{
    class VerbsViewModel
    {
        public ICommand DeleteCommand => new Command<Conjugation>(RemoveVerb);
       
        public VerbsViewModel() { }

        void RemoveVerb(Conjugation verb)
        {
            ConjugationData.Remove(verb);
        }
    }
}
