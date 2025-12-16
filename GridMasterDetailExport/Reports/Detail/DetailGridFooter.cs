using DevExpress.XtraReports.UI;

namespace GridMasterDetailExport.Reports.Detail; 
public class DetailGridFooter : ReportFooterBand {
    public DetailGridFooter(
        string masterKeyField, 
        Dictionary<string, bool> expandedStates) {
        Controls.Add(new XRLabel());
        Height = 20;
        BeforePrint += (s, e) => {
            if(s is XRControl c && c.Report is XtraReport report) {
                var masterKeyValue = report.MasterReport
                    .GetCurrentColumnValue(masterKeyField).ToString();
                e.Cancel = !string.IsNullOrEmpty(masterKeyValue) && !expandedStates[masterKeyValue];
            }
        };
    }
}
