using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SpaceCat_Xamarin_Frontend
{
    class Test_MapCreateViewModel : INotifyPropertyChanged
    {
        private Test_Building _building;
        public Test_Building Building
        {
            get { return _building; }
            set { _building = value; OnPropertyChanged(); }
        }

        public Test_MapCreateViewModel()
        {
            Building = new Test_Building();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
