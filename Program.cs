using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Enable CORS
// cho phép truy cập từ mọi nguồn gốc
builder.Services.AddCors(c => {
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// JSON Serializer  
// đảm bảo rằng việc serialize và deserialize đối tượng JSON
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
.AddNewtonsoftJson(options => 
    options.SerializerSettings.ContractResolver = new DefaultContractResolver());

var app = builder.Build();

// Enable CORS
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Quản lí các 'Route' attribute
app.MapControllers();

// Cấu hình lưu file ảnh
app.UseStaticFiles(new StaticFileOptions {
    FileProvider = new PhysicalFileProvider(
        // Đường dẫn đến thư mục Photos để làm việc
        Path.Combine(Directory.GetCurrentDirectory(), "Photos")),
    // Khi có 1 yêu cầu gửi đến /Photos/ thì Middleware sẽ tìm tệp trong thư mục Photos
    RequestPath = "/Photos"
}); 

app.MapGet("/", () => "Hello World!");

app.Run();
