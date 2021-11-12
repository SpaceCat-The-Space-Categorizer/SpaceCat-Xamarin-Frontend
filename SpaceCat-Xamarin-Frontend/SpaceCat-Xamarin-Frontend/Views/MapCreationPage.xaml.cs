using SpaceCat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchTracking;
using TouchTracking.Forms;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using Xamarin.Forms.Xaml;

// View (Map Creation page) - handles page navigation

namespace SpaceCat_Xamarin_Frontend
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapCreationPage : ContentPage
    {
        public MapCreationPage(Floor map)
        {
            InitializeComponent();
            ((MapCreationViewModel)BindingContext).LoadFloor(map);
        }

        private void TappedMap(object sender, TouchActionEventArgs args)
        {
            Point tapLoc = new Point(args.Location.X, args.Location.Y);
            ((MapCreationViewModel)BindingContext).AreaInputHandler(args.Type, tapLoc);
        }

        public async void ExitPage(object sender, EventArgs e)
        {
            // TODO: send message back with updated floor object
            MessagingCenter.Send(this, "UpdateFloor", ((MapCreationViewModel)BindingContext).ThisFloor);
            await Navigation.PopModalAsync();
        }
    }
}