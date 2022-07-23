using PersonalFinanceManagement.API.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Text.Json;
using PersonalFinanceManagement.API.Database.Entities.DTOs;
using PersonalFinanceManagement.API.Database;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceManagement.API.Database.Repositories;
using PersonalFinanceManagement.API.Extensions;
using PersonalFinanceManagement.API.Database.Entities;
using System.Reflection;
using PersonalFinanceManagement.API.Services;
using AutoMapper;
using PersonalFinanceManagement.API.Mappings;
using PersonalFinanceManagement.API.CustomExceptionMiddleware;

namespace PersonalFinanceManagement.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<ITransactionService, TransactionService>();
            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            builder.Services.AddTransient<ExceptionMiddleware>();

            builder.Services.AddDbContext<TransactionsDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("TransactionConnectionString"));
            });

            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

            builder.Services.AddMvc(options =>
            {
                options.InputFormatters.Insert(0, new CSVInputTransactionFormatter());
                options.InputFormatters.Insert(1, new CSVInputCategoryFormatter());
            });

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();
            //app.MigrateDatabase<TransactionsDbContext>();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("CorsPolicy");

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
