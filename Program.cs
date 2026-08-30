var builder = WebApplication.CreateBuilder(args);
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
// 1. Configure CORS (Allows requests from any origin)
builder.Services.AddCors(options =>
{
    options.AddPolicy(myAllowSpecificOrigins,
                          policy =>
                          {
                              policy.WithOrigins("https://m-verify.onrender.com"),
                                                  
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod();
                          });
});
builder.Services.AddOpenApi("v1");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalar();
}

app.UseHttpsRedirection();



app.Run();

// 4. Data Model
public class UserModel
{
    public Guid Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Miscellaneous { get; set; } = string.Empty;
}
