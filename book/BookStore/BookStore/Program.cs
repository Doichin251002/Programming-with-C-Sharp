using BookStore.BL;
using BookStore.HealthChecks;
using BookStore.MapsterConfig;
using BookStore.ServiceExtensions;
using BookStore.Validators;
using BookStore.Validators.Book;
using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace BookStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(theme:
                    AnsiConsoleTheme.Code)
                .CreateLogger();

            builder.Logging.AddSerilog(logger);

            builder.Services
                .AddConfigurations(builder.Configuration)
                .RegisterDataLayer()
                .RegisterBusinessLayer();

            MapsterConfiguration.Configure();
            builder.Services.AddMapster();

            builder.Services
                .AddValidatorsFromAssemblyContaining<AddBookRequestValidator>();
            builder.Services
                .AddValidatorsFromAssemblyContaining<UpdateBookRequestValidator>();
            builder.Services
                .AddValidatorsFromAssemblyContaining<AddAuthorRequestValidator>();
            builder.Services
                .AddValidatorsFromAssemblyContaining<UpdateAuthorRequestValidator>();

            builder.Services.AddFluentValidationAutoValidation();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddHealthChecks();

            builder.Services.AddHealthChecks()
                .AddCheck<SampleHealthCheck>("Sample");

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapHealthChecks("/Sample");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
