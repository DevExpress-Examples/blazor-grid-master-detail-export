using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using System.ComponentModel;

namespace GridMasterDetailExport.Reports.Master; 
public class MasterGridBodyTable : XRTable {
    public MasterGridBodyTable(
        string masterKeyField,
        string[] columns, 
        Dictionary<string, bool> expandedStates) {
        TextAlignment = TextAlignment.MiddleCenter;
        Rows.Add(CreateTableRow(masterKeyField, columns, expandedStates));
        BeforePrint += AdjustTable;
    }

    private XRTableRow CreateTableRow(
        string masterKeyField,
        string[] columns, 
        Dictionary<string, bool> expandedStates) {
        var row = new XRTableRow();
        row.BeforePrint += (s, e) => {
            if(s is XRTableRow row)
                SetBorders(row, masterKeyField, expandedStates);
        };

        foreach(var column in columns) {
            var cell = new XRTableCell();
            cell.ExpressionBindings.Add(
                new("BeforePrint", "Text", $"[{column}]"));
            row.Cells.Add(cell);
        }
        return row;
    }

    private void SetBorders(
        XRTableRow row,
        string masterKeyField,
        Dictionary<string, bool> expandedStates) {
        row.Borders = BorderSide.Left | BorderSide.Right | BorderSide.Bottom;
        var prevKeyValue = row.Report.RootReport
            .GetPreviousColumnValue(masterKeyField)
            .ToString();
        var currKeyValue = row.Report.RootReport
            .GetCurrentColumnValue(masterKeyField)
            .ToString();
        if(!string.IsNullOrEmpty(prevKeyValue) 
            && expandedStates[prevKeyValue]
            && currKeyValue != prevKeyValue) {
            row.Borders = BorderSide.All;
        }
    }

    private void AdjustTable(object sender, CancelEventArgs e) {
        if(sender is XRTable t && t.RootReport is XtraReport report) {
            var margins = report.Margins.Left + report.Margins.Right;
            t.WidthF = report.PageWidth - margins;
        }
    }
}
