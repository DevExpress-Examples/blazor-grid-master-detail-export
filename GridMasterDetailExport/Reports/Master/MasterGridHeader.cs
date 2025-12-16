using DevExpress.XtraReports.UI;

namespace GridMasterDetailExport.Reports.Master; 
public class MasterGridHeader : ReportHeaderBand {
    public MasterGridHeader(string[] columns) {
        var table = new MasterGridHeaderTable(columns);
        Controls.Add(table);
        Height = table.Height;
    }
}
