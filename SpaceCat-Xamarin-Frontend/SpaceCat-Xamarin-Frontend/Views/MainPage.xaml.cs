using SpaceCat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

// View (Building List page) - handles user input and page navigation on Building List content page, opens FilePicker

namespace SpaceCat_Xamarin_Frontend
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Opens a FloorSelectionEditPage with a new building.
        /// </summary>
        /// <param name="sender">The Create New Building button object.</param>
        /// <param name="e">The event associated with the button.</param>
        private async void Clicked_Create(object sender, EventArgs e)
        {
            // opens a MapCreationPage to create a new building
            string name = null;
            bool accept = false;
            while (!accept)
            {
                name = await DisplayPromptAsync("New Building", "What is the name of this building?", "OK", "Cancel", "", 50, Keyboard.Default, "");
                if (name != "" && name != null)
                    accept = true;
                else if (name == null)
                    break;
                else
                    await DisplayAlert("New Building", "Invalid name, try again!", "OK");
            }
            if (accept)
                await Navigation.PushModalAsync(new FloorSelectionEditPage(new RecentBuilding(new Building(name)), true));
        }

        /// <summary>
        /// Calls the file picker method FilePickBuilding.
        /// </summary>
        /// <param name="sender">The Import Building button object.</param>
        /// <param name="e">The event associated with the button.</param>
        private void Clicked_Import(object sender, EventArgs e)
        {
            // calls the file picker method for building maps
            FilePickBuilding();
            // TODO: check if imported file is already on list (and potentially select it if it is)
        }

        /// <summary>
        /// Gets the building associated with the tapped edit button and opens a FloorSelection page to edit that building.
        /// </summary>
        /// <param name="sender">The edit button object.</param>
        /// <param name="e">The event associated with the button.</param>
        private void Clicked_Edit(object sender, EventArgs e)
        {
            // gets the building associated with the edit button selected and opens a Test_MapCreatePage to edit that building

            Button btn = (Button)sender;
            BuildingListItem building = ((BuildingListViewModel)BindingContext).Buildings.Where(build => build.Build.Name == (string)btn.CommandParameter).FirstOrDefault();
            Navigation.PushModalAsync(new FloorSelectionEditPage(building.Build, false));
        }

        /// <summary>
        /// Gets the building associated with the tapped delete button and removes that building from the list view.
        /// </summary>
        /// <param name="sender">The delete button object.</param>
        /// <param name="e">The event associated with the button.</param>
        private void Clicked_Delete(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            BuildingListItem building = ((BuildingListViewModel)BindingContext).Buildings.Where(build => build.Build.Name == (string)btn.CommandParameter).FirstOrDefault();
            ((BuildingListViewModel)BindingContext).Buildings.Remove(building);
        }

        private void Clicked_Survey(object sender, EventArgs e)
        {
            if (((BuildingListViewModel)BindingContext).SelectedBuilding != null)
            {
                Navigation.PushModalAsync(new FloorSelectionViewPage(((BuildingListViewModel)BindingContext).SelectedBuilding.Build));
            }
            
        }

        private void Clicked_Analysis(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Opens a file picker allowing the user to select a building from a list of accepted file types.
        /// </summary>
        private async void FilePickBuilding()
        {
            // specifies file types to allow user to choose from and opens the file
            // picker to select that type of file (others are greyed out)
            // calls import building method from view model with file result

            var customFileType =
                new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, new[] { "application/buildings" } },
                    { DevicePlatform.UWP, new[] { ".txt", ".csv" } },
                });

            var options = new PickOptions
            {
                PickerTitle = "Please select a building file",
                FileTypes = customFileType,
            };

            // open file picker
            try
            {
                var result = await FilePicker.PickAsync(options);
                if (result != null)
                {
                    ((BuildingListViewModel)BindingContext).ImportBuilding(result);
                }
            }
            catch (Exception e)
            {
                // exit fail
                System.Diagnostics.Debug.WriteLine("Exception on MainPage.xaml.cs in FilePickMap: " + e);
            }
        }
    }
}
