using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceCat;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpaceCat_Xamarin_Frontend
{
	[XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class AnalysisPage : ContentPage

    {
        public AnalysisPage(Building buildingToAnalyze)
        {
            InitializeComponent();
            ((AnalysisViewModel)BindingContext).WorkingBuilding = buildingToAnalyze;
        }

        private async void Clicked_Back(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void Clicked_Export(object sender, EventArgs e)
        {
            ((AnalysisViewModel)BindingContext).CSVExport_Clicked();
        }
    }
}