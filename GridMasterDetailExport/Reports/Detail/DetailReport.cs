using DevExpress.XtraReports.UI;
using System.Collections;

namespace GridMasterDetailExport.Reports.Detail; 
public class DetailReport : XtraReport {
    public DetailReport(
        IEnumerable data,
        string masterKeyField,
        string[] columns, 
        Dictionary<string, bool> expandedStates) {
        Bands.Add(new DetailGridHeader(
            masterKeyField, columns, expandedStates));
        Bands.Add(new DetailGridBody(
            masterKeyField, columns, expandedStates));
        Bands.Add(new DetailGridFooter(
            masterKeyField, expandedStates));
        DataSourceDemanded += (s, e) => {
            AssignDetailDataSources(expandedStates, data, masterKeyField);
        };
    }

    private void AssignDetailDataSources(
        Dictionary<string, bool> expandedStates, 
        IEnumerable dataSource,
        string masterKeyField) {
        var masterKeyValue = this.MasterReport
            .GetCurrentColumnValue(masterKeyField)
            .ToString();
        if(!string.IsNullOrEmpty(masterKeyValue) 
            && expandedStates[masterKeyValue]) {
            List<object> reportData = [];
            foreach(var record in dataSource) {
                var recordValue = GetMasterKeyValue(record, masterKeyField);
                if(recordValue.Equals(masterKeyValue)) {
                    reportData.Add(record);
                }
            }
            this.DataSource = reportData;
        }
    }

    private string GetMasterKeyValue(object record, string masterKeyField) {
        var masterKeyProperty = record.GetType().GetProperty(masterKeyField);
        if(masterKeyProperty is null) {
            throw new MissingMemberException(
                $"Property `{masterKeyField}` not found in a data record");
        }
        return masterKeyProperty.GetValue(record)?.ToString() ?? string.Empty;
    }
}
