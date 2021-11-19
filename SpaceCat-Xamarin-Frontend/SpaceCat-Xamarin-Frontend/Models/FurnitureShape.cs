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
        private string _source;
        private Xamarin.Forms.Rectangle _bounds;
        public Furniture Furn
        {
            get { return _furn; }
            set { _furn = value; OnPropertyChanged(); }
        }
        public string Source
        {
            get { return _source; }
            set { _source = value; OnPropertyChanged(); }
        }
        public Xamarin.Forms.Rectangle Bounds
        {
            get { return _bounds; }
            set { _bounds = value; OnPropertyChanged(); }
        }

        public FurnitureShape(FurnitureBlueprint blueprint)
        {
            Furn = blueprint.NewInstance();
            Source = blueprint.Filepath;
            Bounds = new Xamarin.Forms.Rectangle(0, 0, 100.0 * Furn.StretchX, 100.0 * Furn.StretchY);
            Furn.Corner = new Tuple<double,double>(Bounds.X, Bounds.Y);
        }

        public FurnitureShape(FurnitureShape oldShape, Point newLoc)
        {
            Furn = oldShape.Furn;
            Source = oldShape.Source;
            Bounds = new Xamarin.Forms.Rectangle(newLoc.X, newLoc.Y, oldShape.Bounds.Width, oldShape.Bounds.Height);
        }

        public void Move(Point newLoc)
        {
            Bounds = new Xamarin.Forms.Rectangle(newLoc.X, newLoc.Y, Bounds.Width, Bounds.Height);
            Furn.Corner = new Tuple<double, double>(Bounds.X, Bounds.Y);
            //AbsoluteLayout.SetLayoutBounds(ImgButton, Bounds);
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
