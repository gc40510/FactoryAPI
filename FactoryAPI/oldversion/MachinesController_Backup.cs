using Microsoft.AspNetCore.Mvc;
using FactoryAPI.Models;

namespace FactoryAPI.oldversion;

[ApiController]
[Route("api/[controller]")] // 網址會是 api/machines
public class MachinesController_Backup : ControllerBase
{

    // 模擬從資料庫抓出來的資料
    private static List<Machine> _machines = new List<Machine> {
        new Machine { Name = "沖壓機", Status = 1, Load = 85.5 },
        new Machine { Name = "鑽孔機", Status = 0, Load = 0.0 },
        new Machine { Name = "機械臂", Status = 1, Load = 45.2 }
    };

    // 取得所有機台：GET api/machines
    [HttpGet]
    public IEnumerable<Machine> GetAll()
    {
        return _machines;
    }

    // 取得停機中的機台：GET api/machines/stopped
    [HttpGet("stopped")]
    public IEnumerable<Machine> GetStopped()
    {
        // 使用你剛才學會的 LINQ
        return _machines.Where(m => m.Status == 0).ToList();
    }
    //day3

    // 根據狀態查詢：GET api/machines/status/{status}
    [HttpGet("status/{status}")]
    public IEnumerable<Machine> GetByStatus(int status)
    {
        // 這裡的 status 是從網址抓進來的
        return _machines.Where(m => m.Status == status).ToList();
    }
    // 根據最低負載查詢：GET api/machines/filter?minLoad=50
    [HttpGet("filter")]
    public IEnumerable<Machine> GetByLoad([FromQuery] double minLoad)
    {
        return _machines.Where(m => m.Load >= minLoad).ToList();
    }

    // 新增機台：POST api/machines
    [HttpPost]
    public IActionResult CreateMachine([FromBody] Machine newMachine)
    {
        // 1. 簡單的邏輯檢查
        if (newMachine == null || string.IsNullOrEmpty(newMachine.Name))
        {
            return BadRequest("機台資料不完整"); // 回傳 400 錯誤
        }

        // 2. 將資料加入模擬的資料庫 (List)
        _machines.Add(newMachine);

        // 3. 回傳 201 Created 狀態碼，並告訴使用者新資料的內容
        return CreatedAtAction(nameof(GetAll), new { name = newMachine.Name }, newMachine);
    }

    // 更新機台資訊：PUT api/machines/{name}
    [HttpPut("{name}")]
    public IActionResult UpdateMachine(string name, [FromBody] Machine updatedData)
    {
        // 1. 使用你練過的 LINQ 找出那台機台 (就像 SQL 的 WHERE Name = '...')
        var machine = _machines.FirstOrDefault(m => m.Name == name);

        // 2. 如果找不到，回傳 404
        if (machine == null)
        {
            return NotFound($"找不到名為 {name} 的機台");
        }

        // 3. 修改資料
        machine.Status = updatedData.Status;
        machine.Load = updatedData.Load;

        // 4. 回傳 200 OK 與更新後的結果
        return Ok(machine);
    }

    // 刪除機台：DELETE api/machines/{name}
    [HttpDelete("{name}")]
    public IActionResult DeleteMachine(string name)
    {
        // 1. 尋找該機台
        var machine = _machines.FirstOrDefault(m => m.Name == name);

        // 2. 如果不存在，回傳 404
        if (machine == null)
        {
            return NotFound($"找不到名為 {name} 的機台，無法刪除");
        }

        // 3. 從 List 中移除
        _machines.Remove(machine);

        // 4. 回傳 204 No Content
        // 這是標準做法：告訴對方「刪除成功」，且因為資料已不在，所以沒內容可回傳了
        return NoContent();
    }
}