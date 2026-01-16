namespace GridMasterDetailExport.Data;
public class DataProvider {
    private static readonly List<User> users = [
        new User(1, "Alex"),
        new User(2, "Kevin"),
        new User(3, "Rose"),
    ];

    private static readonly List<Order> orders = [
        new Order(1, 1, DateTime.Today.AddDays(1), "Book"),
        new Order(2, 1, DateTime.Today.AddDays(2), "Sugar"),
        new Order(3, 1, DateTime.Today.AddDays(3), "Laptop"),
        new Order(4, 2, DateTime.Today.AddDays(1), "Brick"),
        new Order(5, 2, DateTime.Today.AddDays(2), "Stone"),
        new Order(6, 2, DateTime.Today.AddDays(3), "Potato"),
        new Order(7, 2, DateTime.Today.AddDays(4), "Cucumber"),
        new Order(8, 2, DateTime.Today.AddDays(5), "Keyboard"),
        new Order(9, 3, DateTime.Today.AddDays(6), "Pencil"),
        new Order(10, 3, DateTime.Today.AddDays(7), "Mug"),
        new Order(11, 3, DateTime.Today.AddDays(8), "Magnet"),
        new Order(12, 3, DateTime.Today.AddDays(9), "Vitamins"),
    ];

    public static List<User> GetUsers() => users;

    public static List<Order> GetOrders() => orders;
}
