using GermanVerbs.Models;
using LiteDB;
using System;
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
            Conjugations = new ObservableCollection<Conjugation>(conjugationCollection.FindAll().OrderBy(x => x._id).ToList());
        }

        internal static void Remove(Conjugation conjugation)
        {
            if (Conjugations.Contains(conjugation))
            {
                Conjugations.Remove(conjugation);
            }
        }

        internal static void Insert(Conjugation newConjugation)
        {
            if (!Conjugations.Contains(newConjugation))
            {
                Conjugations.Add(newConjugation);
            }
        }

        internal static Conjugation FindOne(string infinitive)
        {
            return Conjugations.SingleOrDefault(x => x._id == infinitive);
        }

        internal static List<Conjugation> GetActive()
        {
            return Conjugations.Where(x => x.IsActive).ToList();
        }

        internal static void Sort()
        {
            Conjugations.Sort();
        }

        public static void Sort<T>(this ObservableCollection<T> collection) where T : IComparable<Conjugation>
        {
            List<T> sorted = collection.OrderBy(x => x).ToList();
            for (int i = 0; i < sorted.Count(); i++)
                collection.Move(collection.IndexOf(sorted[i]), i);
        }

        internal static void SaveToDB()
        {
            conjugationCollection.Upsert(Conjugations);
        }
    }
}
