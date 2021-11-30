﻿using SpaceCat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchTracking;
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
            ((DataEntryViewModel)BindingContext).LoadFloor(thisFloor);
        }

        private void TappedMap(object sender, TouchActionEventArgs args)
        {
            if (args.Type == TouchActionType.Released)
            {
                Point tapLoc = new Point(args.Location.X, args.Location.Y);
                bool furnitureSelected = ((DataEntryViewModel)BindingContext).SelectionHandler(tapLoc);
                if (furnitureSelected)
                    CounterFrame.IsVisible = true;
                else
                    CounterFrame.IsVisible = false;
            }
        }

        private void TappedAddAreaNote(object sender, EventArgs e)
        {
            // (unimplemented) add pop-up entry to add note to area
        }

        private async void TappedSave (object sender, EventArgs e)
        {
            // TODO: save survey data
            bool success = true;

            if (success)
                await Navigation.PopModalAsync();
            else
            {
                // TODO: add pop-up indicating an error occurred
            }
        }

        private async void TappedAbort(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}