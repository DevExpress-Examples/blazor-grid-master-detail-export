namespace GridMasterDetailExport.Data;
public class GridState {
    public string[] MasterColumnNames { get; set; } = [];

    public string[] DetailColumnNames { get; set; } = [];

    public Dictionary<string, bool> RowExpandedStates { get; set; } = [];
}
