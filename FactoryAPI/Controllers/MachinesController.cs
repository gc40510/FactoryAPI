using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using FactoryAPI.Data;
using FactoryAPI.Services;
using FactoryAPI.Models;

namespace FactoryAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MachinesController : ControllerBase
{
    private readonly IMachineService _machineService;

    public MachinesController(IMachineService machineService)
    {
        _machineService = machineService;
    }

    /// <summary>
    /// 取得所有工廠機台的狀態與完整維修紀錄
    /// </summary>
    /// <response code="200">成功取得所有機台清單與關聯的維修紀錄</response>
    /// <response code="404">獲取失敗</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Machine>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Machine>>> GetAll()
    {
        var machines = await _machineService.GetAllMachinesAsync();
        return Ok(machines);
    }

    /// <summary>
    /// 取得目前處於停機狀態 (Status = 0) 的所有機台
    /// </summary>
    /// <response code="200">成功回傳目前停機中的機台清單</response>
    /// <response code="404">目前沒有任何機台處於停機狀態</response>
    [HttpGet("stopped")]
    [ProducesResponseType(typeof(IEnumerable<Machine>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Machine>>> GetStopped()
    {
        var machines = await _machineService.GetStoppedMachinesAsync();
        return Ok(machines);
    }

    /// <summary>
    /// 根據特定的狀態碼查詢機台
    /// </summary>
    /// <param name="status">狀態代碼 (例如：0 為停機，1 為運行中)</param>
    /// <response code="200">成功取得符合該狀態的機台清單</response>
    [HttpGet("status/{status}")]
    [ProducesResponseType(typeof(IEnumerable<Machine>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Machine>>> GetByStatus(int status)
    {
        var machines = await _machineService.GetMachinesByStatusAsync(status);
        return Ok(machines);
    }

    /// <summary>
    /// 在系統中建立一台新的工廠機台
    /// </summary>
    /// <param name="newMachine">機台的基本資訊 (名稱、狀態、負載)</param>
    /// <response code="200">機台建立成功並回傳完整資料</response>
    [HttpPost]
    [ProducesResponseType(typeof(Machine), StatusCodes.Status200OK)]
    public async Task<ActionResult<Machine>> CreateMachine([FromBody] Machine newMachine)
    {
        var result = await _machineService.CreateMachineAsync(newMachine);
        return Ok(result);
    }

    /// <summary>
    /// 修改指定機台的名稱、狀態或負載資訊
    /// </summary>
    /// <param name="id">欲修改的機台 ID</param>
    /// <param name="updatedData">新的機台屬性資料</param>
    /// <response code="200">機台資訊更新成功</response>
    /// <response code="404">找不到指定的機台 ID，無法更新</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Machine), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateMachine(int id, [FromBody] Machine updatedData)
    {
        var result = await _machineService.UpdateMachineAsync(id, updatedData);
        if (result == null) return NotFound($"找不到 ID 為 {id} 的機台");

        return Ok(result);
    }

    /// <summary>
    /// 根據 ID 從系統中永久移除特定機台
    /// </summary>
    /// <param name="id">欲刪除的機台 ID</param>
    /// <response code="204">機台已成功從資料庫移除</response>
    /// <response code="404">找不到指定的機台 ID，無法刪除</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMachine(int id)
    {
        var success = await _machineService.DeleteMachineAsync(id);
        if (!success) return NotFound($"找不到 ID 為 {id} 的機台");

        return NoContent();
    }

    /// <summary>
    /// 針對特定機台新增一筆報修或保養紀錄
    /// </summary>
    /// <param name="machineId">目標機台 ID</param>
    /// <param name="record">維修內容描述與日期</param>
    /// <response code="200">維修紀錄新增成功</response>
    /// <response code="404">找不到對應的機台 ID，無法掛載維修紀錄</response>
    [HttpPost("{machineId}/maintenance")]
    [ProducesResponseType(typeof(MaintenanceRecord), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MaintenanceRecord>> PostMaintenance(int machineId, MaintenanceRecord record)
    {
        var result = await _machineService.AddMaintenanceAsync(machineId, record);
        if (result == null) return NotFound($"找不到 ID 為 {machineId} 的機台");

        return Ok(result);
    }
}