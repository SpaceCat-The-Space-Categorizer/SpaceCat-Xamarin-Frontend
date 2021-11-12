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
        public FloorSelectionEditPage()//Building thisBuilding)
        {
            //((FloorSelectionEditViewModel)BindingContext).ThisBuilding = thisBuilding;
            InitializeComponent();
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

        private async void Tapped_SaveExit(object sender, EventArgs e)
        {
            // TODO: add updated floors list to building floors
            await Navigation.PopModalAsync();
        }
    }
}