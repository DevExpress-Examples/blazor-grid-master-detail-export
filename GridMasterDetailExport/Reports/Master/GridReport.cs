using DevExpress.XtraReports.UI;
using GridMasterDetailExport.Data;
using GridMasterDetailExport.Reports.Detail;
using System.Collections;

namespace GridMasterDetailExport.Reports.Master; 
public class GridReport : XtraReport {
    public IEnumerable MasterDataSource => DataProvider.GetUsers();
    public IEnumerable DetailDataSource => DataProvider.GetOrders();
    public string MasterKey => "UserID";
    public GridReport(
        string[] masterColumns, 
        string[] detailColumns, 
        Dictionary<string, bool> expandedStates) {
        Bands.Add(new MasterGridHeader(masterColumns));
        Bands.Add(new MasterGridBody(
            MasterDataSource, MasterKey, masterColumns, expandedStates));
        Bands.Add(new DetailBand());
        Bands.Add(new DetailGrid(
            DetailDataSource, MasterKey, detailColumns, expandedStates));
        DataSource = MasterDataSource;
        CreateDocument();
    }
}
