using SpaceCat;
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
    public partial class FloorSelectionEditPage : ContentPage
    {
        public FloorSelectionEditPage(Building thisBuilding, bool newBuild)
        {
            InitializeComponent();
            ((FloorSelectionEditViewModel)BindingContext).NewBuild = newBuild;
            ((FloorSelectionEditViewModel)BindingContext).LoadBuilding(thisBuilding);
        }

        private async void Tapped_NewFloor(object sender, EventArgs e)
        {
            b1.IsEnabled = false;
            b2.IsEnabled = false;
            b3.IsEnabled = false;
            string name = null;
            int number = 0;

            bool validName = false;
            while (!validName)
            {
                name = await DisplayPromptAsync("New Floor", "Enter a name for the new floor: ", "OK", "Cancel", "a name...");
                if (name != null && name.Length > 0)
                    validName = true;
                else if (name == null)
                    break;
                else
                    await DisplayAlert("New Floor", "The floor needs a valid name!", "OK");
            }

            bool cancel = true;
            bool accept = false;
            
            while (!accept && validName)
            {
                string result = await DisplayPromptAsync("New Floor", "On which floor is this floorplan located? : ", "OK", "Cancel", "", 5, Keyboard.Numeric, "");
                try
                {
                    if (result != null)
                    {
                        number = Int32.Parse(result);
                        cancel = false;
                    }
                    accept = true;
                }
                catch (FormatException)
                {
                    await DisplayAlert("New Floor", "The new floor number needs to be a whole number!", "OK");
                }
            }

            b1.IsEnabled = true;
            b2.IsEnabled = true;
            b3.IsEnabled = true;

            if (!cancel)
                await Navigation.PushModalAsync(new MapCreationPage(new Floor(number, name), true));
        }

        private async void Tapped_EditFloor(object sender, EventArgs e)
        {
            Floor editFloor = ((FloorSelectionEditViewModel)BindingContext).SelectedFloor;
            if (editFloor != null)
                await Navigation.PushModalAsync(new MapCreationPage(editFloor, false));
        }

        private void Tapped_DeleteFloor(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Floor floor = ((FloorSelectionEditViewModel)BindingContext).Floors.Where(f => f.FloorNumber == (int)btn.CommandParameter).FirstOrDefault();
            ((FloorSelectionEditViewModel)BindingContext).Floors.Remove(floor);
        }

        private async void Tapped_SaveExit(object sender, EventArgs e)
        {
            ((FloorSelectionEditViewModel)BindingContext).SaveExit();
            await Navigation.PopModalAsync();
        }
    }
}