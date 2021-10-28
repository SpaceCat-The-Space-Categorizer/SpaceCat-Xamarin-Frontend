using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

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
            Navigation.PushModalAsync(new Test_MapCreatePage());
        }

        private void Clicked_Edit(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Test_Building building = ((BuildingListViewModel)BindingContext).Buildings.Where(build => build.BuildingID == (int)btn.CommandParameter).FirstOrDefault();
            Navigation.PushModalAsync(new Test_MapCreatePage(building));
        }

        private void Clicked_Delete(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Test_Building building = ((BuildingListViewModel)BindingContext).Buildings.Where(build => build.BuildingID == (int)btn.CommandParameter).FirstOrDefault();
            ((BuildingListViewModel)BindingContext).Buildings.Remove(building);
        }

        private void Clicked_Analysis(object sender, EventArgs e)
        {

        }
    }
}
