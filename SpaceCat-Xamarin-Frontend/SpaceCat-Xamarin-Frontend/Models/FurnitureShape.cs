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
        private string _status;
        private string _source;
        private Xamarin.Forms.Rectangle _bounds;
        /// <summary>
        /// The furniture object associated with this shape.
        /// </summary>
        public Furniture Furn
        {
            get { return _furn; }
            set { _furn = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// The color indicating the status of the shape.
        /// Green has been counted, Red hasn't been counted, Yellow indicates the shape has been selected.
        /// </summary>
        public string Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// The file name of the image representing the furniture.
        /// </summary>
        public string Source
        {
            get { return _source; }
            set { _source = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// Contains the X location, Y location, width, and height of the furniture image.
        /// </summary>
        public Xamarin.Forms.Rectangle Bounds
        {
            get { return _bounds; }
            set { _bounds = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Used for creating new furniture.
        /// </summary>
        /// <param name="blueprint">The blueprint to use for creating this furniture shape.</param>
        public FurnitureShape(FurnitureBlueprint blueprint)
        {
            Furn = blueprint.NewInstance();
            Status = "Uncounted";
            Source = blueprint.Filepath;
            Bounds = new Xamarin.Forms.Rectangle(0, 0, 100.0 * Furn.StretchX, 100.0 * Furn.StretchY);
            Furn.Corner = new Tuple<double,double>(Bounds.X, Bounds.Y);
        }

        /// <summary>
        /// Used for updating background color of furniture shape's frame on update of status.
        /// </summary>
        /// <param name="oldShape">The shape being altered.</param>
        /// <param name="newStatus">The new status of the shape (Uncounted, Counted, or InProgress).</param>
        public FurnitureShape(FurnitureShape oldShape, string newStatus)
        {
            Furn = oldShape.Furn;
            Status = newStatus;
            Source = oldShape.Source;
            Bounds = oldShape.Bounds;
            Furn.Corner = oldShape.Furn.Corner;
        }

        /// <summary>
        /// Used for recreating existing furniture.
        /// </summary>
        /// <param name="existingFurn">The pre-existing furniture object.</param>
        public FurnitureShape(Furniture existingFurn)
        {
            Furn = existingFurn;
            Status = "Uncounted";
            Source = existingFurn.Filepath;
            Bounds = new Xamarin.Forms.Rectangle(existingFurn.Corner.Item1, existingFurn.Corner.Item2, 100.0 * Furn.StretchX, 100.0 * Furn.StretchY);
        }

        /// <summary>
        /// Moves this object to a new location.
        /// </summary>
        /// <param name="newLoc">The location to move the object to.</param>
        public void Move(Point newLoc)
        {
            Bounds = new Xamarin.Forms.Rectangle(newLoc.X, newLoc.Y, Bounds.Width, Bounds.Height);
            Furn.Corner = new Tuple<double, double>(Bounds.X, Bounds.Y);
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
