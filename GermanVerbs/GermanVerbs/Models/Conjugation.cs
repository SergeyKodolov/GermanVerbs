using System.Collections.Generic;
using System.ComponentModel;

namespace GermanVerbs.Models
{
    public class Conjugation
    {
        [Description("Infinitive")]
        public string Infinitive { get; set; }

        [Description("Перевод")]
        public string Translation { get; set; }

        [Description("Present Ind.")]
        public Dictionary<string, string> PresentIndicative { get; set; }

        [Description("Perfect Ind.")]
        public Dictionary<string, string> PerfectIndicative { get; set; }

        [Description("Present Imp.")]
        public Dictionary<string, string> PresentImperative { get; set; }

        public bool Active { get; set; }
    }
}
