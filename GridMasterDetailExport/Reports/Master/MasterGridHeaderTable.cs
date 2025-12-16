using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using System.ComponentModel;
using System.Drawing;

namespace GridMasterDetailExport.Reports.Master; 
public class MasterGridHeaderTable : XRTable {
    public MasterGridHeaderTable(string[] columns) {
        TextAlignment = TextAlignment.MiddleCenter;
        BackColor = Color.AliceBlue;
        Borders = BorderSide.All;
        Rows.Add(CreateTableRow(columns));
        BeforePrint += AdjustTable;
    }

    private XRTableRow CreateTableRow(string[] columns) {
        var row = new XRTableRow();
        foreach(var column in columns) {
            row.Cells.Add(new XRTableCell() { Text = column });
        }
        return row;
    }

    private void AdjustTable(object sender, CancelEventArgs e) {
        if(sender is XRTable t && t.RootReport is XtraReport report) {
            var margins = report.Margins.Left + report.Margins.Right;
            t.WidthF = report.PageWidth - margins;
        }
    }
}
