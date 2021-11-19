﻿using SpaceCat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchTracking;
using TouchTracking.Forms;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using Xamarin.Forms.Xaml;

// View (Map Creation page) - handles page navigation

namespace SpaceCat_Xamarin_Frontend
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapCreationPage : ContentPage
    {
        private bool NewFloor;
        public MapCreationPage(Floor thisFloor, bool newFloor)
        {
            InitializeComponent();
            NewFloor = newFloor;
            ImageButton[] presets = ((MapCreationViewModel)BindingContext).LoadFloor(thisFloor);
            int column = 0;
            for (int i = 0; i < presets.Length; i++)
            {
                FurniturePresetList.Children.Add(presets[i], column, i / 2);
                if (column == 0)
                    column = 1;
                else
                    column = 0;
            }
        }

        private void TappedMap(object sender, TouchActionEventArgs args)
        {
            Point tapLoc = new Point(args.Location.X, args.Location.Y);
            ((MapCreationViewModel)BindingContext).AreaInputHandler(args.Type, tapLoc);
        }

        public async void ExitPage(object sender, EventArgs e)
        {
            Floor updatedFloor = ((MapCreationViewModel)BindingContext).UpdateFloor();
            MessagingCenter.Send(this, "UpdateFloor", (updatedFloor, NewFloor));
            await Navigation.PopModalAsync();
        }
    }
}