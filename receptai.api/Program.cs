using Microsoft.EntityFrameworkCore;
using receptai.api.Interfaces;
using receptai.api.Repositories;
using receptai.data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<RecipePlatformDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SQLite")));

builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddScoped<ISubfoodditRepository, SubfoodditRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

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
