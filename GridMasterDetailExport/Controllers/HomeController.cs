using DevExpress.XtraReports.UI;
using GridMasterDetailExport.Data;
using GridMasterDetailExport.Reports;
using Microsoft.AspNetCore.Mvc;

namespace GridMasterDetailExport.Controllers;
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
        using(var report = ReportGenerationHelpers.GetReportFromDxGrid(gridState))
        using(var ms = new MemoryStream()) {
            exportAction?.Invoke(report, ms);
            return File(ms.ToArray(), contentType);
        }
    }
}
