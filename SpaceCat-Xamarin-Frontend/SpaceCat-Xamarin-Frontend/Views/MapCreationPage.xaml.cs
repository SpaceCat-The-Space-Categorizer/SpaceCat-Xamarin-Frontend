using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public MapCreationPage()
        {
            InitializeComponent();
            DrawArea();
            if (Device.RuntimePlatform == Device.UWP)
            {
                // add a pointer event to theMap
                
            }
        }

        public void DrawArea()
        {
            // calls the view model's Create Area method with a point collection and
            // adds the result to the map
            Polygon anArea = ((MapCreationViewModel)BindingContext).CreateArea(new PointCollection { new Point(50, 50), new Point(300, 50), new Point(300, 300), new Point(50, 300) });
            theMap.Children.Add(anArea);
        }

        public async void ExitPage(object sender, EventArgs e)
        {
            // navigates back to landing page
            // TODO: send message back with updated building object
            await Navigation.PopModalAsync();
        }
    }
}