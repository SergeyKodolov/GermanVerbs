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

            MainPage = new AppShell();
        }

        protected override void OnSleep()
        {
            ConjugationData.SaveToDB();
            base.OnSleep();
        }
    }
}
