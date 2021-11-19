using System;
using System.Collections.Generic;
using System.Text;
using SpaceCat;

namespace SpaceCat_Xamarin_Frontend
{
    class AnalysisViewModel
    {
        public Building WorkingBuilding { get; set; }
        public AnalysisViewModel()
        {
        }

        public void CSVExport_Clicked()
        {
            //We should consider automatically creating views at some point
            WorkingBuilding.DatabaseHandler.CreateViews(true);
            //We should add code to handle files with different names
            WorkingBuilding.DatabaseHandler.ExportCSV("test");
        }
    }
}
