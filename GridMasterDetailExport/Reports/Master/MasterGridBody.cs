using DevExpress.XtraReports.UI;
using System.Collections;

namespace GridMasterDetailExport.Reports.Master; 
public class MasterGridBody : GroupHeaderBand {
    public MasterGridBody(
        IEnumerable dataSource, 
        string masterKey, 
        string[] masterColumns, 
        Dictionary<string, bool> expandedStates) {
        var table = new MasterGridBodyTable(
            masterKey, masterColumns, expandedStates);
        Controls.Add(table);
        Height = table.Height;
        GroupFields.Add(new(masterKey));
    }
}
