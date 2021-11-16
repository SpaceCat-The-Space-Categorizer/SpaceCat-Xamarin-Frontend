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
        public FloorSelectionEditPage(Building thisBuilding)
        {
            InitializeComponent();
            ((FloorSelectionEditViewModel)BindingContext).LoadBuilding(thisBuilding);
        }

        private async void Tapped_NewFloor(object sender, EventArgs e)
        {
            string name = await DisplayPromptAsync("New Floor", "Enter a name for the new floor: ", "OK", "Cancel", "a name...");
            int number = 0;
            bool cancel = true;
            bool accept = false;
            
            while (!accept)
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
            
            if (name != null && !cancel)
                await Navigation.PushModalAsync(new MapCreationPage(new Floor(number), true));
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