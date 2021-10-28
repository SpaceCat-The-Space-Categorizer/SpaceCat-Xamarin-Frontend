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
    public partial class Test_MapCreatePage : ContentPage
    {
        public Test_MapCreatePage(Test_Building building = null)
        {
            InitializeComponent();

            if (building != null)
            {
                ((Test_MapCreateViewModel)BindingContext).Building = building;
            }
        }

        private async void Clicked_Save(object sender, EventArgs e)
        {
            Test_Building building = ((Test_MapCreateViewModel)BindingContext).Building;
            MessagingCenter.Send(this, "CreateBuilding", building);
            
            await Navigation.PopModalAsync();
        }
    }
}