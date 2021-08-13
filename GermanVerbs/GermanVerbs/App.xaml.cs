using System;
using System.IO;
using Xamarin.Forms;

namespace GermanVerbs
{
    public partial class App : Application
    {
        public static Random Randomizer { get; } = new Random();
        
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
