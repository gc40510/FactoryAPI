namespace FactoryAPI;

public class Machine
{
    // 加入這一行作為主鍵
    public int Id { get; set; }
    public string Name { get; set; }
    public int Status { get; set; } // 0=停機, 1=運作中
    public double Load { get; set; }

    // --- 加入下面這行 ---
    // 這代表：一台機台可以擁有多筆維修紀錄
    public List<MaintenanceRecord> MaintenanceRecords { get; set; } = new();
}