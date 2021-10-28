using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceCat_Xamarin_Frontend
{
    public class Test_Building
    {
        public int BuildingID { get; set; }
        public string BuildingName { get; set; }
        public string BuildingDate { get; set; }
        public string BuildingFloorplan { get; set; }
        public string BuildingStatus { get; set; }

        public Test_Building()
        {

        }

        public Test_Building(int buildingID, string buildingName, string buildingDate, string buildingLayout, string buildingStatus)
        {
            BuildingID = buildingID;
            BuildingName = buildingName;
            BuildingDate = buildingDate;
            BuildingFloorplan = buildingLayout;
            BuildingStatus = buildingStatus;
        }
    }
}
