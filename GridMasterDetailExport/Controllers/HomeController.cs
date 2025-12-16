using DevExpress.XtraReports.UI;
using GridMasterDetailExport.Data;
using GridMasterDetailExport.Reports.Master;
using Microsoft.AspNetCore.Mvc;

namespace DXApplication1.Controllers;
[ApiController]
[Route("api/[controller]")]
public class HomeController : ControllerBase {
    [HttpPost("pdf")]
    public IActionResult PDF([FromForm] GridState gridState) {
        return GetExportResult(gridState, 
            (r, ms) => r.ExportToPdf(ms), 
            "application/pdf");
    }

    [HttpPost("xlsx")]
    public IActionResult Xlsx([FromForm] GridState gridState) {
        return GetExportResult(gridState,
            (r, ms) => r.ExportToXlsx(ms),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    [HttpPost("csv")]
    public IActionResult Csv([FromForm] GridState gridState) {
        return GetExportResult(gridState,
            (r, ms) => r.ExportToCsv(ms),
            "text/csv");
    }

    private FileContentResult GetExportResult(
        GridState gridState,
        Action<XtraReport, MemoryStream> exportAction,
        string contentType) {
        using(var ms = new MemoryStream()) {
            XtraReport report = new GridReport(
                gridState.MasterColumnNames,
                gridState.DetailColumnNames,
                gridState.RowExpandedStates
            );
            exportAction?.Invoke(report, ms);
            return File(ms.ToArray(), contentType);
        }
    }
}
