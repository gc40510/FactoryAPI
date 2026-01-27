using FactoryAPI;

namespace FactoryAPI.Services;

public interface IMachineService
{
    // 取得所有機台
    Task<IEnumerable<Machine>> GetAllMachinesAsync();

    // 根據 ID 取得特定機台
    Task<Machine?> GetMachineByIdAsync(int id);

    // 取得停機中的機台 (Status == 0)
    Task<IEnumerable<Machine>> GetStoppedMachinesAsync();

    // 根據特定狀態查詢機台
    Task<IEnumerable<Machine>> GetMachinesByStatusAsync(int status);

    // 新增機台
    Task<Machine> CreateMachineAsync(Machine machine);

    // 更新機台
    Task<Machine?> UpdateMachineAsync(int id, Machine updatedData);

    // 刪除機台
    Task<bool> DeleteMachineAsync(int id);

    // 新增維修紀錄
    Task<MaintenanceRecord?> AddMaintenanceAsync(int machineId, MaintenanceRecord record);
}