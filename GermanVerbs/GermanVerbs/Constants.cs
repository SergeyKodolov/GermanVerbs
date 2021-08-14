using System;
using System.IO;

namespace GermanVerbs
{
    class Constants
    {
        public const string DatabaseFilename = "Conjugation.db";

        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, DatabaseFilename);
            }
        }
    }
}
