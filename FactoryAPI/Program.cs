using FactoryAPI.Data;
using Microsoft.EntityFrameworkCore;
using FactoryAPI.Data; // 記得引用你剛才建立的資料夾
using FactoryAPI.Services; // 這裡要換成你 Services 資料夾的實際命名空間
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    // 取得 XML 檔名 (通常跟專案名稱一樣)
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

    // 告訴 Swagger 去讀取這份檔案
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});


// 註冊資料庫連線
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// 註冊依賴注入 (Dependency Injection)
builder.Services.AddScoped<IMachineService, MachineService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
