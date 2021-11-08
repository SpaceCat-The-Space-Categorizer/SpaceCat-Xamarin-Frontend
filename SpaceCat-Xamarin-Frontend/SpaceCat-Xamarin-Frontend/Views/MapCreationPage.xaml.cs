using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
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

        public async void ExitPage(object sender, EventArgs e)
        {
            // navigates back to landing page
            // TODO: send message back with updated building object
            await Navigation.PopModalAsync();
        }
    }
}