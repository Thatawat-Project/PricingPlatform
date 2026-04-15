var builder = WebApplication.CreateBuilder(args);

// 1. ดึง Configuration ของ Reverse Proxy จาก appsettings.json
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// (Option) เพิ่ม CORS หากคุณต้องเรียกจาก Web Browser (Frontend)
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// 2. ตั้งค่าลำดับ Middleware
app.UseCors("DefaultPolicy");

// แสดง Log พื้นฐานเพื่อดูว่ามี Request เข้ามา
app.Use(async (context, next) =>
{
    Console.WriteLine($"Incoming request: {context.Request.Method} {context.Request.Path}");
    await next();
});

app.UseRouting();

// 3. Map Reverse Proxy
app.MapReverseProxy();

app.Run();