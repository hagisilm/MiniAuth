using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniAuth.IdentityAuth.Models;
using System.Diagnostics;

namespace MiniAuth.Identity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            Debug.WriteLine("* start Services add");
            builder.Services.AddCors(options => 
            options.AddPolicy("AllowAll", 
            builder => builder
                .WithOrigins(
                    "http://localhost:5173"
                )
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
            ));
            builder.Services.AddMiniAuth();
            builder.Services.AddControllers();
#if DEBUG
            //builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            //builder.Services.AddIdentityApiEndpoints<IdentityUser>();
#endif

            Debug.WriteLine("* start builder build");   
            var app = builder.Build();
            app.UseCors("AllowAll");
            app.MapGet("/", () => "Hello World!");
            //app.MapGroup("/api").MapIdentityApi<IdentityUser>();
            app.MapControllers();
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}
            //app.UseMiniAuth();
            app.Run();
        }
    }


}
