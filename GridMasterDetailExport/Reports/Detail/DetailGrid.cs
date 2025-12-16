using DevExpress.XtraReports.UI;
using System.Collections;

namespace GridMasterDetailExport.Reports.Detail; 
public class DetailGrid : GroupFooterBand {
    public DetailGrid(
        IEnumerable dataSource,
        string masterKeyField,
        string[] columns, 
        Dictionary<string, bool> expandedStates) {
        var subreport = new XRSubreport() {
            ReportSource = new DetailReport(dataSource, masterKeyField, columns, expandedStates),
            GenerateOwnPages = false,
        };
        Controls.Add(subreport);
        Height = subreport.Height;
    }
}
