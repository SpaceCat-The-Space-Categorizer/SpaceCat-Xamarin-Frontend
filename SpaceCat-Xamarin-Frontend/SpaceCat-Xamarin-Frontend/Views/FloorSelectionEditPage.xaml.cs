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
            Floor newFloor = ((FloorSelectionEditViewModel)BindingContext).ExecuteNewFloor();
            await Navigation.PushModalAsync(new MapCreationPage(newFloor));
        }

        private async void Tapped_EditFloor(object sender, EventArgs e)
        {
            Floor editFloor = ((FloorSelectionEditViewModel)BindingContext).SelectedFloor;
            await Navigation.PushModalAsync(new MapCreationPage(editFloor));
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