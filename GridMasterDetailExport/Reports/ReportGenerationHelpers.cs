using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;
using DevExpress.DataAccess;
using DevExpress.DataAccess.DataFederation;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.Drawing;
using DevExpress.Drawing.Printing;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using GridMasterDetailExport.Data;

namespace GridMasterDetailExport.Reports {
    public static class ReportGenerationHelpers {
        public static XtraReport GetReportFromDxGrid(string[] masterColumnNames, string[] detailColumnNames, Dictionary<string, bool> rowExpandedStates) {
            XtraReport report = new XtraReport() {
                ReportUnit = ReportUnit.HundredthsOfAnInch,
                PaperKind = DXPaperKind.Letter,
                Margins = new DXMargins(100, 100, 100, 100)
            };

            #region Styles
            var tableHeaderStyle = new XRControlStyle {
                Name = "tableHeaderStyle",
                TextAlignment = TextAlignment.MiddleCenter,
                BackColor = Color.AliceBlue,
            };
            report.StyleSheet.Add(tableHeaderStyle);

            var tableRowStyle = new XRControlStyle() {
                Name = "tableRowStyle",
                TextAlignment = TextAlignment.MiddleCenter
            };
            report.StyleSheet.Add(tableRowStyle);
            #endregion

            #region DataSource

            var masterDataSource = new ObjectDataSource() {
                Name = "Users",
                DataSource = typeof(DataProvider),
                DataMember = nameof(DataProvider.GetUsers)
            };
            masterDataSource.RebuildResultSchema();

            var detailDataSource = new ObjectDataSource() {
                Name = "Orders",
                DataSource = typeof(DataProvider),
                DataMember = nameof(DataProvider.GetOrders)
            };
            detailDataSource.RebuildResultSchema();

            var fds = ReportGenerationHelpers.CreateFederationDataSource(masterDataSource, detailDataSource, nameof(User.UserID), nameof(Order.UserID), rowExpandedStates);
            #endregion

            float contentWidth = report.PageWidthF - report.Margins.Left - report.Margins.Right;

            report.DataSource = fds;
            report.DataMember = masterDataSource.Name;

            report.AddTableHeaderBand(new RectangleF(0f, 0f, contentWidth, 25f), tableHeaderStyle.Name, masterColumnNames);
            report.AddTableRowBand(new RectangleF(0f, 0f, contentWidth, 25f), tableRowStyle.Name, masterColumnNames, true);
            report.AddDetailReport(
                $"{masterDataSource.Name}.{masterDataSource.Name}{detailDataSource.Name}",
                detailColumnNames,
                new RectangleF(20f, 20f, contentWidth - 20f, 25f),
                tableHeaderStyle.Name,
                tableRowStyle.Name);

            return report;
        }

        static FederationDataSource CreateFederationDataSource(DataComponentBase masterDataSource, DataComponentBase detailDataSource, string parentKeyColumn, string nestedKeyColumn, Dictionary<string, bool> expandedStates) {
            ObjectDataSource expandedStateDataSource = new ObjectDataSource() {
                Name = "ExpandedState",
                DataSource = typeof(ExpandedState),
                DataMember = nameof(ExpandedState.LoadExpandedStates)
            };
            expandedStateDataSource.Parameters.Add(new Parameter(
                "json",
                typeof(string),
                JsonSerializer.Serialize(expandedStates.Select((i) => new ExpandedState() { RowKey = i.Key, IsExpanded = i.Value }))));

            Source masterSource = new Source(masterDataSource.Name, masterDataSource);
            Source detailSource = new Source(detailDataSource.Name, detailDataSource);
            Source expandedStatesSource = new Source("ExpandedState", expandedStateDataSource);


            var masterQuery = masterSource.From()
                .SelectAll()
                .InnerJoin(expandedStatesSource, $"[{masterSource.Name}.{parentKeyColumn}] = [{expandedStatesSource.Name}].[{nameof(ExpandedState.RowKey)}]")
                .Select(nameof(ExpandedState.IsExpanded))
                .Build(masterSource.Name);
            var detailQuery = detailSource.From()
                .SelectAll()
                .Build(detailSource.Name);

            var federationDataSource = new FederationDataSource();
            federationDataSource.Queries.AddRange([masterQuery, detailQuery]);

            var relation = new FederationMasterDetailInfo(masterSource.Name, detailSource.Name, new FederationRelationColumnInfo(parentKeyColumn, nestedKeyColumn));
            federationDataSource.Relations.Add(relation);
            federationDataSource.RebuildResultSchema();

            return federationDataSource;
        }

        public static void AddTableHeaderBand(this XtraReportBase report, RectangleF rect, string styleName, string[] columnCaptions) {
            var headerBand = new GroupHeaderBand() {
                HeightF = rect.Height,
                RepeatEveryPage = true,
                KeepTogether = true,
                GroupUnion = GroupUnion.WithFirstDetail
            };
            report.Bands.Add(headerBand);

            headerBand.AddXRTable(columnCaptions, rect, styleName,
                (table) => {
                    table.Borders = BorderSide.All;
                },
                (cell, caption) => {
                    cell.Text = caption;
                });
        }

        public static void AddTableRowBand(this XtraReportBase report, RectangleF rect, string styleName, string[] fieldNames, bool isExpandable = false) {
            var rowBand = new DetailBand() {
                HeightF = rect.Height,
                KeepTogether = true,
                KeepTogetherWithDetailReports = true                
            };
            report.Bands.Add(rowBand);

            rowBand.AddXRTable(fieldNames, rect, styleName,
                (table) => {
                    if(isExpandable) {
                        table.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Borders", $"IIf([DataSource.IsFirstRow] || !PrevRowColumnValue('{nameof(ExpandedState.IsExpanded)}'),'Left,Right,Bottom', 'All')\r\n"));
                    }
                    else {
                        table.Borders = BorderSide.Left | BorderSide.Right | BorderSide.Bottom;
                    }
                },
                (cell, fieldName) => {
                    cell.ExpressionBindings.Add(
                       new("BeforePrint", "Text", $"[{fieldName}]"));
                });
        }

        public static void AddEmptyTableFooterBand(this XtraReportBase report, float height) {
            var footerBand = new GroupFooterBand() {
                HeightF = height,
                GroupUnion = GroupFooterUnion.WithLastDetail
            };
            report.Bands.Add(footerBand);
            footerBand.Controls.Add(new XRPanel() { HeightF = height });
        }

        public static void AddDetailReport(this XtraReportBase report, string dataMember, string[] columns, RectangleF rect, string headerStyleName, string rowStyleName) {
            var detailReportBand = new DetailReportBand() {
                DataSource = report.DataSource,
                DataMember = dataMember
            };
            report.Bands.Add(detailReportBand);
            detailReportBand.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Visible", $"[{nameof(ReportGenerationHelpers.ExpandedState.IsExpanded)}]"));

            detailReportBand.AddTableHeaderBand(rect, headerStyleName, columns);
            detailReportBand.AddTableRowBand(new RectangleF(rect.X, 0f, rect.Width, rect.Height), rowStyleName, columns);
            detailReportBand.AddEmptyTableFooterBand(rect.Height);
        }

        private static void AddXRTable<T>(this Band band, T[] columns, RectangleF rect, string styleName, Action<XRTable> tableInitializer, Action<XRTableCell, T> cellInitializer) {
            var table = new XRTable();
            band.Controls.Add(table);
            table.BeginInit();
            table.LocationF = rect.Location;
            table.SizeF = rect.Size;
            table.StyleName = styleName;
            tableInitializer(table);

            var row = new XRTableRow();
            table.Rows.Add(row);

            foreach(T column in columns) {
                XRTableCell cell = new XRTableCell();
                row.Cells.Add(cell);
                cellInitializer(cell, column);
            }

            table.EndInit();
        }

        public class ExpandedState {
            [JsonRequired]
            public string RowKey { get; set; } = "";
            [JsonRequired]
            public bool IsExpanded { get; set; }

            public static IEnumerable<ExpandedState>? LoadExpandedStates(string json) {
                return JsonSerializer.Deserialize<List<ExpandedState>>(json);
            }
        }
    }
}
