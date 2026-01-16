namespace GridMasterDetailExport.Data;
public class User(int userID, string? userName) {
    public int UserID { get; set; } = userID;
    public string? UserName { get; set; } = userName;
}
