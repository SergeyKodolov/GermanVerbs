using GermanVerbs.Data;
using System;
using Xamarin.Forms;

namespace GermanVerbs
{
    public partial class App : Application
    {
        public static Random Randomizer { get; } = new Random();

        public App()
        {
            InitializeComponent();
            Device.SetFlags(new string[] { "AppTheme_Experimental" });
            Current.UserAppTheme = OSAppTheme.Unspecified;
            MainPage = new AppShell();
        }

        protected override void OnSleep()
        {
            ConjugationData.SaveToDB();
            base.OnSleep();
        }
    }
}
