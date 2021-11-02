using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

// View (Test Map Creation page) - messages BuildingListViewModel with the new or edited building object

namespace SpaceCat_Xamarin_Frontend
{
    // IDK when/if I need this line
    [XamlCompilation(XamlCompilationOptions.Compile)]


    public partial class Test_MapCreatePage : ContentPage
    {
        public Test_MapCreatePage(Test_Building building = null)
        {
            // no argument constructor used when creating new building
            // single arg constructor used when editting recieved building
            
            InitializeComponent();

            if (building != null)   //assign view model's Building variable with the one recieved so it can be editted
            {
                ((Test_MapCreateViewModel)BindingContext).Building = building;
            }
        }

        private async void Clicked_Save(object sender, EventArgs e)
        {
            // Messaging Center sends the newly created or editted version building object to the Building list view model
            // The map creation page is then popped off the stack in order to return to the landing page

            Test_Building building = ((Test_MapCreateViewModel)BindingContext).Building;
            MessagingCenter.Send(this, "CreateBuilding", building);

            await Navigation.PopModalAsync();
        }
    }
}