﻿using SpaceCat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpaceCat_Xamarin_Frontend
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DataEntryPage : ContentPage
    {
        public DataEntryPage(Floor thisFloor)
        {
            InitializeComponent();
        }
    }
}