var builder = WebApplication.CreateBuilder(args);

// 1. Configure CORS (Allows requests from any origin)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

CORS Middleware
app.UseCors("AllowAll");

// In-memory data store for demonstration
var users = new List<UserModel>();

// User Endpoints

// GET: Retrieve all users
app.MapGet("/api/users", () => Results.Ok(users))
   .WithName("GetUsers");

// GET: Retrieve a user by ID
app.MapGet("/api/users/{id:guid}", (Guid id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    return user is not null ? Results.Ok(user) : Results.NotFound();
})
.WithName("GetUserById");

// POST: Create a new user
app.MapPost("/api/users", (UserModel newUser) =>
{
    newUser.Id = Guid.NewGuid();
    users.Add(newUser);
    return Results.Created($"/api/users/{newUser.Id}", newUser);
})
.WithName("CreateUser");

// PUT: Update an existing user
app.MapPut("/api/users/{id:guid}", (Guid id, UserModel updatedUser) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user is null) return Results.NotFound();

    user.Number = updatedUser.Number;
    user.Email = updatedUser.Email;
    user.Miscellaneous = updatedUser.Miscellaneous;

    return Results.Ok(user);
})
.WithName("UpdateUser");

//  Remove a user
app.MapDelete("/api/users/{id:guid}", (Guid id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user is null) return Results.NotFound();

    users.Remove(user);
    return Results.NoContent();
})
.WithName("DeleteUser");

app.Run();

// 4. Data Model
public class UserModel
{
    public Guid Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Miscellaneous { get; set; } = string.Empty;
}
