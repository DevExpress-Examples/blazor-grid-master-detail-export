using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using System.ComponentModel;

namespace GridMasterDetailExport.Reports.Detail; 
public class DetailGridBodyTable : XRTable {
    private float leftMargin = 20f;
    public DetailGridBodyTable(string[] columns) {
        LeftF = leftMargin;
        TextAlignment = TextAlignment.MiddleCenter;
        Borders = BorderSide.Left | BorderSide.Right | BorderSide.Bottom;
        Rows.Add(CreateTableRow(columns));
        BeforePrint += AdjustDetailTable;
    }

    private XRTableRow CreateTableRow(string[] columns) {
        var row = new XRTableRow();
        foreach(var col in columns) {
            var cell = new XRTableCell();
            cell.ExpressionBindings.Add(
                new("BeforePrint", "Text", $"[{col}]"));
            row.Cells.Add(cell);
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
