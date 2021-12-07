using SpaceCat;
using System;
using System.Collections.Generic;
using System.IO;
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
            SetFloorImage();
            panButtons.TranslationX = Application.Current.MainPage.Width - 200;
            panButtons.TranslationY = Application.Current.MainPage.Height - 200;
        }

        private async void SetFloorImage()
        {
            string fileName = ((DataEntryViewModel)BindingContext).GetFullFloorName();
            Stream stream = await DependencyService.Get<IFileService>().GetPicture(fileName);
            if (stream != null)
            {
                ImageSource myImage = ImageSource.FromStream(() => stream);
                floorImg.Source = myImage;
            }
        }

        private void TappedMap(object sender, TouchActionEventArgs args)
        {
            if (args.Type == TouchActionType.Released)
            {
                Point tapLoc = new Point(args.Location.X, args.Location.Y);
                string selection = ((DataEntryViewModel)BindingContext).SelectionHandler(tapLoc);

                if (selection == "furniture")
                {
                    CounterFrame.IsVisible = true;
                    AddButton.IsEnabled = true;
                    MinusButton.IsEnabled = true;
                }
                else if (selection == "area")
                {
                    CounterFrame.IsVisible = true;
                    ((DataEntryViewModel)BindingContext).SeatingText = "";
                    AddButton.IsEnabled = false;
                    MinusButton.IsEnabled = false;
                }
                else
                {
                    CounterFrame.IsVisible = false;
                }
            }
            else if (((DataEntryViewModel)BindingContext).SelectedFigureIndex == -1)
            {
                CounterFrame.IsVisible = false;
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
            if (theMap.TranslationX < CounterFrame.Width)
            {
                if (theMap.TranslationX + halfScreenWidth > CounterFrame.Width)
                    theMap.TranslationX = CounterFrame.Width;
                else
                    theMap.TranslationX += halfScreenWidth;
            }
        }

        private void Tapped_MapRight(object sender, EventArgs e)
        {
            double screenWidth = Application.Current.MainPage.Width;
            double halfScreenWidth = screenWidth / 2.0;
            double minTranslation = -20.0 - (floorImg.Width - screenWidth);
            if (theMap.TranslationX > minTranslation)
            {
                if (theMap.TranslationX - halfScreenWidth < minTranslation)
                    theMap.TranslationX = minTranslation;
                else
                    theMap.TranslationX -= halfScreenWidth;
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
            bool discard = await DisplayAlert("Abort Survey", "Are you sure you want to discard this survey?", "Continue", "No, Cancel!");
            if (discard)
                await Navigation.PopModalAsync();
        }
    }
}