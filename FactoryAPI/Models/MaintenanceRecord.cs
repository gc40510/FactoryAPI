using System.Text.Json.Serialization;

namespace FactoryAPI.Models;

public class MaintenanceRecord
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty; // 維修內容描述
    public DateTime ServiceDate { get; set; } = DateTime.Now; // 維修日期

    // 外鍵：這筆紀錄屬於哪台機台
    public int MachineId { get; set; }

    // 導覽屬性：讓程式知道這筆紀錄是對應到哪一台 Machine
    [JsonIgnore]
    public Machine? Machine { get; set; }
}