using SpaceCat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SpaceCat_Xamarin_Frontend
{
    /// <summary>
    ///     ListView data container for the building list on the landing page.
    /// </summary>
    public class BuildingListItem
    {
        private RecentBuilding _build;
        private string _name;
        private DateTime _date;
        public RecentBuilding Build
        {
            get { return _build; }
            set { _build = value; OnPropertyChanged(); }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged(); }
        }

        /// <summary>
        ///     Initalizes a new instance of the BuildingListItem class.
        /// </summary>
        /// <param name="building">The building to base this BuildingListItem object on.</param>
        public BuildingListItem (RecentBuilding building)
        {
            Build = building;
            Name = building.Name;
            Date = building.DateCreated;
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
