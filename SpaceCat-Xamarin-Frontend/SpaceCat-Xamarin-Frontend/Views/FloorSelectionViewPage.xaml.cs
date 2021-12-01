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
    public partial class FloorSelectionViewPage : ContentPage
    {
        public FloorSelectionViewPage(RecentBuilding thisBuilding)
        {
            InitializeComponent();
            ((FloorSelectionViewViewModel)BindingContext).LoadBuilding(Persistence.LoadBuilding(thisBuilding.Name));
        }

        private async void Tapped_SaveExit(object sender, EventArgs e)
        {
            ((FloorSelectionViewViewModel)BindingContext).SaveExit();
            await Navigation.PopModalAsync();
        }

        private async void FloorSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Floor surveyFloor = ((FloorSelectionViewViewModel)BindingContext).SelectedFloor;
            if (surveyFloor != null)
                await Navigation.PushModalAsync(new DataEntryPage(surveyFloor));
        }
    }
}