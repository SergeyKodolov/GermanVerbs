﻿using GermanVerbs.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GermanVerbs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerbsPage : ContentPage
    {
        public VerbsPage()
        {
            InitializeComponent();
            BindingContext = new VerbsViewModel();
        }
    }
}