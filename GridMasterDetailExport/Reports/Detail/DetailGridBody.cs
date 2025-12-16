using DevExpress.XtraReports.UI;

namespace GridMasterDetailExport.Reports.Detail; 
public class DetailGridBody : DetailBand {
    public DetailGridBody(
        string masterKeyField, 
        string[] columns, 
        Dictionary<string, bool> expandedStates) {
        var table = new DetailGridBodyTable(columns);
        Controls.Add(table);
        Height = table.Height;
        BeforePrint += (s, e) => {
            if(s is XRControl c && c.Report is XtraReport report){
                var masterKeyValue = report.MasterReport.GetCurrentColumnValue(masterKeyField).ToString();
                e.Cancel = !string.IsNullOrEmpty(masterKeyValue) && !expandedStates[masterKeyValue];
            }
        };
    }
}
