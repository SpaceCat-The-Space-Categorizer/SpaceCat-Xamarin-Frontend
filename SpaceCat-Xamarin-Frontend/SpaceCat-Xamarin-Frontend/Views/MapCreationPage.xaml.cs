using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchTracking;
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
        }

        private void TappedMap(object sender, TouchActionEventArgs args)
        {
            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    //mouse down
                    if (((MapCreationViewModel)BindingContext).NewAreaToolOn || 
                        ((MapCreationViewModel)BindingContext).AddAreaToolOn)
                    {
                        DrawArea(new Point(args.Location.X, args.Location.Y), new Point(args.Location.X + 1.0, args.Location.Y + 1.0));
                    }
                    break;
                case TouchActionType.Moved:
                case TouchActionType.Released:
                    //mouse move or up
                    ((MapCreationViewModel)BindingContext).AreaCreationHandler(args);
                    break;
            }
        }

        public void DrawArea(Point start, Point end)
        {
            // calls the view model's Create Area method with a point collection and
            // adds the result to the map
            PointCollection points = new PointCollection { new Point(start.X, start.Y), new Point(end.X, start.Y), new Point(end.X, end.Y), new Point(start.X, end.Y) };
            Polygon anArea = ((MapCreationViewModel)BindingContext).CreateArea(points);
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