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
        public ICommand TapCommand => new Command<string>(SelectBuilding);

        
        public MainPage()
        {
            InitializeComponent();
        }

        void SelectBuilding(string build)
        {

        }

        private void Clicked_Analysis(object sender, EventArgs e)
        {

        }

        private void Clicked_Create(object sender, EventArgs e)
        {

        }
    }
}
