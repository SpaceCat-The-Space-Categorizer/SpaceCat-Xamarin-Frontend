﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SpaceCat;

namespace SpaceCat_Xamarin_Frontend
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Area temp = new Area();
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
