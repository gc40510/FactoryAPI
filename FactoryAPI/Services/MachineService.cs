using FactoryAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace FactoryAPI.Services;
using FactoryAPI.Models;

public class MachineService : IMachineService
{
    private readonly AppDbContext _context;

    public MachineService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Machine>> GetAllMachinesAsync()
    {
        return await _context.Machines.Include(m => m.MaintenanceRecords).ToListAsync();
    }

    public async Task<Machine?> GetMachineByIdAsync(int id)
    {
        return await _context.Machines.Include(m => m.MaintenanceRecords)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    // 實作：取得停機機台 (Status == 0)
    public async Task<IEnumerable<Machine>> GetStoppedMachinesAsync()
    {
        return await _context.Machines.Where(m => m.Status == 0).ToListAsync();
    }

    // 實作：根據狀態查詢
    public async Task<IEnumerable<Machine>> GetMachinesByStatusAsync(int status)
    {
        return await _context.Machines.Where(m => m.Status == status).ToListAsync();
    }

    public async Task<Machine> CreateMachineAsync(Machine machine)
    {
        _context.Machines.Add(machine);
        await _context.SaveChangesAsync();
        return machine;
    }

    // 實作：更新機台邏輯
    public async Task<Machine?> UpdateMachineAsync(int id, Machine updatedData)
    {
        var machine = await _context.Machines.FindAsync(id);
        if (machine == null) return null;

        machine.Name = updatedData.Name;
        machine.Status = updatedData.Status;
        machine.Load = updatedData.Load;

        await _context.SaveChangesAsync();
        return machine;
    }

    // 實作：刪除機台
    public async Task<bool> DeleteMachineAsync(int id)
    {
        var machine = await _context.Machines.FindAsync(id);
        if (machine == null) return false;

        _context.Machines.Remove(machine);
        await _context.SaveChangesAsync();
        return true;
    }

    // 實作：新增維修紀錄並綁定機台 ID
    public async Task<MaintenanceRecord?> AddMaintenanceAsync(int machineId, MaintenanceRecord record)
    {
        var machine = await _context.Machines.FindAsync(machineId);
        if (machine == null) return null;

        record.MachineId = machineId;
        _context.MaintenanceRecords.Add(record);
        await _context.SaveChangesAsync();
        return record;
    }
}