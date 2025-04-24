using Microsoft.EntityFrameworkCore;
using WebAPI.DataAccess;
using WebAPI.Services;
using WebAPI.Repositories;
using WebAPI.Models;
using Microsoft.OpenApi.Models;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<WebAPIContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IStudentRepository, StudentRepository>();

            builder.Services.AddScoped<IStudentService, StudentService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Student API",
                    Version = "v1",
                    Description = "API for managing student data",
                    Contact = new OpenApiContact
                    {
                        Name = "Mohamed Gamal",
                        Email = "imgamal00@gmail.com"
                    }
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Student API v1");
                    c.RoutePrefix = string.Empty;
                });

                app.MapOpenApi();
            }

            app.UseStaticFiles();

            app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
