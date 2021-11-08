using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

// View (Building List page) - handles user input and page navigation on Building List content page

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

            Navigation.PushModalAsync(new MapCreationPage());
        }

        private void Clicked_Edit(object sender, EventArgs e)
        {
            // gets the building associated with the edit button selected and opens a Test_MapCreatePage to edit that building

            Button btn = (Button)sender;
            Test_Building building = ((BuildingListViewModel)BindingContext).Buildings.Where(build => build.BuildingID == (int)btn.CommandParameter).FirstOrDefault();
            Navigation.PushModalAsync(new Test_MapCreatePage(building));
        }

        private void Clicked_Delete(object sender, EventArgs e)
        {
            // gets the building associated with the delete button selected and removes that building from the list view

            Button btn = (Button)sender;
            Test_Building building = ((BuildingListViewModel)BindingContext).Buildings.Where(build => build.BuildingID == (int)btn.CommandParameter).FirstOrDefault();
            ((BuildingListViewModel)BindingContext).Buildings.Remove(building);
        }

        private void Clicked_Analysis(object sender, EventArgs e)
        {

        }
    }
}
