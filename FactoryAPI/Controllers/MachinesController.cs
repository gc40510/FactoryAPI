using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FactoryAPI.Data;

namespace FactoryAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MachinesController : ControllerBase
{
    private readonly AppDbContext _context;

    // 透過建構子注入資料庫上下文，這座橋樑連接到你的 SQL Server
    public MachinesController(AppDbContext context)
    {
        _context = context;
    }

    // 1. 取得所有機台 (從資料庫抓取)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Machine>>> GetAll()
    {
        // 舊 單筆資料 return await _context.Machines.ToListAsync();
        // 使用 Include 同時抓出該機台關聯的所有維修紀錄
        return await _context.Machines
            .Include(m => m.MaintenanceRecords)
            .ToListAsync();
    }

    // 2. 取得停機中的機台 (資料庫版 LINQ)
    [HttpGet("stopped")]
    public async Task<ActionResult<IEnumerable<Machine>>> GetStopped()
    {
        return await _context.Machines.Where(m => m.Status == 0).ToListAsync();
    }

    // 3. 根據狀態查詢
    [HttpGet("status/{status}")]
    public async Task<ActionResult<IEnumerable<Machine>>> GetByStatus(int status)
    {
        return await _context.Machines.Where(m => m.Status == status).ToListAsync();
    }

    // 4. 新增機台 (正式寫入資料庫)
    [HttpPost]
    public async Task<ActionResult<Machine>> CreateMachine([FromBody] Machine newMachine)
    {
        if (newMachine == null || string.IsNullOrEmpty(newMachine.Name))
        {
            return BadRequest("機台資料不完整");
        }

        _context.Machines.Add(newMachine);
        await _context.SaveChangesAsync(); // 這一行才會發送 INSERT 指令到 SQL

        return CreatedAtAction(nameof(GetAll), new { id = newMachine.Id }, newMachine);
    }

    // 5. 更新機台資訊 (根據 ID 尋找並修改)
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMachine(int id, [FromBody] Machine updatedData)
    {
        var machine = await _context.Machines.FindAsync(id);

        if (machine == null)
        {
            return NotFound($"找不到 ID 為 {id} 的機台");
        }

        machine.Status = updatedData.Status;
        machine.Load = updatedData.Load;
        machine.Name = updatedData.Name;

        await _context.SaveChangesAsync(); // 發送 UPDATE 指令
        return Ok(machine);
    }

    // 6. 刪除機台
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMachine(int id)
    {
        var machine = await _context.Machines.FindAsync(id);

        if (machine == null)
        {
            return NotFound($"找不到 ID 為 {id} 的機台");
        }

        _context.Machines.Remove(machine);
        await _context.SaveChangesAsync(); // 發送 DELETE 指令

        return NoContent();
    }

    // 7. 新增維修紀錄：POST api/Machines/{machineId}/maintenance
    [HttpPost("{machineId}/maintenance")]
    public async Task<ActionResult<MaintenanceRecord>> PostMaintenance(int machineId, MaintenanceRecord record)
    {
        // 檢查機台是否存在
        var machine = await _context.Machines.FindAsync(machineId);
        if (machine == null)
        {
            return NotFound($"找不到 ID 為 {machineId} 的機台，無法新增維修紀錄");
        }

        // 關鍵動作：將維修紀錄與該機台的 ID 綁定 (這就是 FK 的運作)
        record.MachineId = machineId;

        _context.MaintenanceRecords.Add(record);
        await _context.SaveChangesAsync(); // 正式寫入 SQL Server

        return Ok(record);
    }

}