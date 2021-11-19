using SpaceCat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace SpaceCat_Xamarin_Frontend
{
    public class FurnitureShape
    {
        private Furniture _furn;
        private ImageSource _source;
        private double _scaleX;
        private double _scaleY;
        public Furniture Furn
        {
            get { return _furn; }
            set { _furn = value; OnPropertyChanged(); }
        }
        public ImageSource Source
        {
            get { return _source; }
            set { _source = value; OnPropertyChanged(); }
        }
        public double ScaleX
        {
            get { return _scaleX; }
            set { _scaleX = value; OnPropertyChanged(); }
        }
        public double ScaleY
        {
            get { return _scaleY; }
            set { _scaleY = value; OnPropertyChanged(); }
        }

        public FurnitureShape(Furniture myFurn)
        {
            Furn = myFurn;
            Source = myFurn.Filepath;
            ScaleX = myFurn.StretchX;
            ScaleY = myFurn.StretchY;
        }

        public FurnitureShape(FurnitureBlueprint blueprint)
        {
            Furn = blueprint.NewInstance();
            Source = blueprint.Filepath;
            ScaleX = Furn.StretchX;
            ScaleY = Furn.StretchY;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        ///     Indicates that the UI should be updated to reflect some kind of change to bound variables.
        /// </summary>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
