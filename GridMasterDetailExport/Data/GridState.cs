using Microsoft.AspNetCore.Mvc;

namespace GridMasterDetailExport.Data; 
public class GridState {
    public string[] MasterColumnNames 
        => MasterColumnsString?.Split(';') ?? [];

    public string[] DetailColumnNames
        => DetailColumnsString?.Split(";") ?? [];

    public Dictionary<string, bool> RowExpandedStates
        => (ExpandedStatesString ?? "")
            .Split(';', StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.Trim('[', ']'))
            .Select(p => p.Split(',', StringSplitOptions.RemoveEmptyEntries))
            .ToDictionary(
                p => p[0].Trim(),
                p => bool.Parse(p[1].Trim())
            );

    [FromForm(Name = "masterColumns")]
    public string? MasterColumnsString { get; set; }

    [FromForm(Name = "detailColumns")]
    public string? DetailColumnsString { get; set; }

    [FromForm(Name = "expandedStates")]
    public string? ExpandedStatesString { get; set; }
}
