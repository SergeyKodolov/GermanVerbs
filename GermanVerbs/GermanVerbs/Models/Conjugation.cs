using LiteDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace GermanVerbs.Models
{
    public class Conjugation : IComparable<Conjugation>
    {
        [Description("Infinitive")]
        public string _id { get; set; }

        [Description("Перевод")]
        public string Translation { get; set; }

        [Description("Präsens Indikativ")]
        public Dictionary<string, string> PresentIndicative { get; set; }

        [Description("Perfekt Indikativ")]
        public Dictionary<string, string> PerfectIndicative { get; set; }

        [Description("Präsens Imperativ")]
        public Dictionary<string, string> PresentImperative { get; set; }

        public bool IsActive { get; set; }

        public int CompareTo(Conjugation other) {
            
            return _id.CompareTo(other._id);
        }
    }
}
