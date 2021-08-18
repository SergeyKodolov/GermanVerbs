using GermanVerbs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace GermanVerbs.Controls
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
                    .Where(x => x._id.ToLower()
                    .Contains(newValue.ToLower()))
                    .ToList();
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
