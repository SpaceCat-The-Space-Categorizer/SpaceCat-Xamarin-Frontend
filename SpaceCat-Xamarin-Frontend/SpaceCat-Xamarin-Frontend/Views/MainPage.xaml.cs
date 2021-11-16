﻿using SpaceCat;
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

        private void Clicked_Create(object sender, EventArgs e)
        {
            // opens a MapCreationPage to create a new building
            Navigation.PushModalAsync(new FloorSelectionEditPage(new Building("A Fackin Building"), true));
        }

        private void Clicked_Import(object sender, EventArgs e)
        {
            // calls the file picker method for building maps
            FilePickBuilding();
            // TODO: check if imported file is already on list (and potentially select it if it is)
        }

        private void Clicked_Edit(object sender, EventArgs e)
        {
            // gets the building associated with the edit button selected and opens a Test_MapCreatePage to edit that building

            Button btn = (Button)sender;
            BuildingListItem building = ((BuildingListViewModel)BindingContext).Buildings.Where(build => build.Build.Name == (string)btn.CommandParameter).FirstOrDefault();
            Navigation.PushModalAsync(new FloorSelectionEditPage(building.Build, false));
        }

        private void Clicked_Delete(object sender, EventArgs e)
        {
            // gets the building associated with the delete button selected and removes that building from the list view

            Button btn = (Button)sender;
            BuildingListItem building = ((BuildingListViewModel)BindingContext).Buildings.Where(build => build.Build.Name == (string)btn.CommandParameter).FirstOrDefault();
            ((BuildingListViewModel)BindingContext).Buildings.Remove(building);
        }

        private void Clicked_Analysis(object sender, EventArgs e)
        {
            
        }

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
