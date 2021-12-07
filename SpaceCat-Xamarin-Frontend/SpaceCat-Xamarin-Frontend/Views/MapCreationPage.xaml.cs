using SpaceCat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
            SetFloorImage();
            mainStack.RaiseChild(toolStack);
            toolStack.HeightRequest = Application.Current.MainPage.Height;
            toolStack.TranslationX = -10;
            toolStack.TranslationY = -10;
            panButtons.TranslationX = Application.Current.MainPage.Width - 200;
            panButtons.TranslationY = Application.Current.MainPage.Height - 200;
            theMap.TranslationX = 230;
        }

        private async void SetFloorImage()
        {
            string fileName = ((MapCreationViewModel)BindingContext).GetFullFloorName();
            Stream stream = await DependencyService.Get<IFileService>().GetPicture(fileName);
            if (stream != null)
            {
                ImageSource myImage = ImageSource.FromStream(() => stream);
                floorImg.Source = myImage;
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

        private void Tapped_MapUp(object sender, EventArgs e)
        {
            double halfScreenHeight = Application.Current.MainPage.Height / 2.0;
            if (theMap.TranslationY < 0)
            {
                if (theMap.TranslationY + halfScreenHeight > 0)
                    theMap.TranslationY = 0;
                else
                    theMap.TranslationY += halfScreenHeight;
            }
        }
        private void Tapped_MapDown(object sender, EventArgs e)
        {
            double halfScreenHeight = Application.Current.MainPage.Height / 2.0;
            double minTranslation = -20.0 - (floorImg.Height - (halfScreenHeight * 2.0));
            if (theMap.TranslationY > minTranslation)
            {
                if (theMap.TranslationY - halfScreenHeight < minTranslation)
                    theMap.TranslationY = minTranslation;
                else
                    theMap.TranslationY -= halfScreenHeight;
            }
        }

        private void Tapped_MapLeft(object sender, EventArgs e)
        {
            double halfScreenWidth = Application.Current.MainPage.Width / 2.0;
            if (theMap.TranslationX < toolStack.Width)
            {
                if (theMap.TranslationX + halfScreenWidth > toolStack.Width)
                    theMap.TranslationX = toolStack.Width;
                else
                    theMap.TranslationX += halfScreenWidth;
            }
        }

        private void Tapped_MapRight(object sender, EventArgs e)
        {
            double screenWidth = Application.Current.MainPage.Width;
            double halfScreenWidth = screenWidth / 2.0 - 200;
            double minTranslation = -20.0 - (floorImg.Width - screenWidth);
            if (theMap.TranslationX > minTranslation)
            {
                if (theMap.TranslationX - halfScreenWidth < minTranslation)
                    theMap.TranslationX = minTranslation;
                else
                    theMap.TranslationX -= halfScreenWidth;
            }
        }

        private void Tapped_NewFurniture(object sender, EventArgs e)
        {
            ImageButton ib = (ImageButton)sender;
            ((MapCreationViewModel)BindingContext).AddNewFurniture(ib);
        }

        private async void Tapped_MapSettings(object sender, EventArgs e)
        {
            (sender as Button).IsEnabled = false;

            Stream stream = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
            if (stream != null)
            {
                string fileName = ((MapCreationViewModel)BindingContext).GetFullFloorName();
                DependencyService.Get<IFileService>().SavePicture(fileName, stream);
                SetFloorImage();
            }

            (sender as Button).IsEnabled = true;
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