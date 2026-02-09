namespace FactoryAPI.Dtos;

public class MachineDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Status { get; set; }
    public double Load { get; set; }

    // 這裡我們改用簡化版的維修紀錄 DTO，避免無窮迴圈
    public List<MaintenanceRecordDto> MaintenanceRecords { get; set; } = new();
}