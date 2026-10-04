using m_verify_BE.Data;
using m_verify_BE.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(myAllowSpecificOrigins,
        policy =>
        {
            if (builder.Environment.IsDevelopment())
            {
                policy.WithOrigins("http://localhost:5173", "http://localhost:3000", "https://m-verify.onrender.com")
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            }
            else
            {
                policy.WithOrigins("https://m-verify.onrender.com")
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            }
        });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=mverify.db"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
    await DbInitializer.SeedAsync(db);
}

app.UseMiddleware<ApiKeyMiddleware>();
app.UseCors(myAllowSpecificOrigins);
app.UseHttpsRedirection();
app.MapControllers();
app.MapGet("/", () => Results.Ok(new { app = "m-verify-be", status = "running", endpoints = new[] { "/api/verify/health", "/api/verify/search", "/api/admin/records (requires X-API-KEY)", "/api/admin/auth/verify" } }));
app.Run();
