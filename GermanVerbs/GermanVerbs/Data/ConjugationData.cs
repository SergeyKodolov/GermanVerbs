using GermanVerbs.Models;
using LiteDB;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GermanVerbs.Data
{
    public static class ConjugationData
    {
        static LiteCollection<Conjugation> conjugationCollection;
        public static ObservableCollection<Conjugation> Conjugations { get; set; }

        static ConjugationData()
        {
            var db = new LiteDatabase(Constants.DatabasePath);
            conjugationCollection = (LiteCollection<Conjugation>)db.GetCollection<Conjugation>();
            Conjugations = new ObservableCollection<Conjugation>(conjugationCollection.FindAll().OrderBy(x => x.Infinitive).ToList());
        }

        internal static void Save()
        {
            conjugationCollection.DeleteAll();
            conjugationCollection.Insert(Conjugations);
        }

        internal static void Insert(Conjugation newConjugation)
        {
            if (!Conjugations.Contains(newConjugation))
                Conjugations.Add(newConjugation);
        }

        internal static Conjugation FindOne(string infinitive)
        {
            return Conjugations.SingleOrDefault(x => x.Infinitive == infinitive);
        }

        internal static List<Conjugation> GetActive()
        {
            return Conjugations.Where(x => x.Active).ToList();
        }
    }
}
