using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using System.ComponentModel;
using System.Drawing;

namespace GridMasterDetailExport.Reports.Detail; 
public class DetailGridHeaderTable : XRTable {
    private float leftMargin = 20f;
    private float topMargin = 20f;
    public DetailGridHeaderTable(string[] columns) {
        LeftF = leftMargin;
        TopF = topMargin;
        BackColor = Color.AliceBlue;
        TextAlignment = TextAlignment.MiddleCenter;
        Borders = BorderSide.All;
        Rows.Add(CreateTableRow(columns));
        BeforePrint += AdjustDetailTable;
    }

    private XRTableRow CreateTableRow(string[] columns) {
        var row = new XRTableRow();
        foreach(var column in columns) {
            var c = new XRTableCell() { Text = column };
            row.Cells.Add(c);
        }
        return row;
    }

    private void AdjustDetailTable(object sender, CancelEventArgs e) {
        if(sender is XRTable t && t.RootReport is XtraReport report) {
            var margins = report.Margins.Left + report.Margins.Right;
            t.WidthF = report.PageWidth - margins - leftMargin;
        }
    }
}
