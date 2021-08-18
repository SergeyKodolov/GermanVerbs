using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GermanVerbs
{
    public class VerbSearchHandler : SearchHandler
    {
        public IList<Conjugation> Conjugations { get; set; }

        protected override void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);

            if (string.IsNullOrWhiteSpace(newValue))
            {
                ItemsSource = null;
            }
            else
            {
                ItemsSource = Conjugations
                    .Where(x => x.Infinitive.ToLower()
                    .Contains(newValue.ToLower()))
                    .ToList<Conjugation>();
            }
        }

        public event EventHandler<Conjugation> QueryConfirmed;

        protected override void OnQueryConfirmed()
        {
            base.OnQueryConfirmed();
            QueryConfirmed?.Invoke(this, null);
        }

        protected override void OnItemSelected(object item)
        {
            base.OnItemSelected(item);
            QueryConfirmed?.Invoke(this, (Conjugation)item);
        }
    }
}
