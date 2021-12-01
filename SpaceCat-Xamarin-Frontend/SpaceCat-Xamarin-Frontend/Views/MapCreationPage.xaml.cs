using SpaceCat;
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
        public MapCreationPage(Floor thisFloor)
        {
            InitializeComponent();
            ImageButton[] presets = ((MapCreationViewModel)BindingContext).LoadFloor(thisFloor);
            int column = 0;
            for (int i = 0; i < presets.Length; i++)
            {
                presets[i].Clicked += Tapped_NewFurniture;
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
            int movedIndex = ((MapCreationViewModel)BindingContext).MapInputHandler(args.Type, tapLoc);
            if (movedIndex != -1)
            {
                Xamarin.Forms.Rectangle newBounds = ((MapCreationViewModel)BindingContext).Shapes[movedIndex].Bounds;
                AbsoluteLayout.SetLayoutBounds(mapFurniture.Children.ElementAt(movedIndex), newBounds);
            }
        }

        private void Tapped_NewFurniture(object sender, EventArgs e)
        {
            ImageButton ib = (ImageButton)sender;
            ((MapCreationViewModel)BindingContext).AddNewFurniture(ib);
        }

        public async void ExitPage(object sender, EventArgs e)
        {
            Floor updatedFloor = ((MapCreationViewModel)BindingContext).UpdateFloor(false);
            bool userContinue = false;
            if (updatedFloor == null)
                userContinue = await FreeFurnitureWarning();
            
            if (updatedFloor != null)
            {
                MessagingCenter.Send(this, "UpdateFloor", (updatedFloor));
                await Navigation.PopModalAsync();
            }
            else if (userContinue)
            {
                updatedFloor = ((MapCreationViewModel)BindingContext).UpdateFloor(true);
                MessagingCenter.Send(this, "UpdateFloor", (updatedFloor));
                await Navigation.PopModalAsync();
            }
        }

        public async Task<bool> FreeFurnitureWarning()
        {
            return await DisplayAlert("Furniture not placed in area!",
                        "Any furniture not placed in inside a defined area will be deleted! Are you sure you want to continue?",
                        "Continue Save", "Cancel");
        }
    }
}