using Microsoft.EntityFrameworkCore;
using StudentRegApi.DB;
using StudentRegApi.Services; 
using Amazon;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<StudentDb>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("StConnectionString")));

        builder.Services.AddSingleton<S3Service>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
        app.UseAuthorization();
        app.MapControllers();
        app.Run();

    }
}
