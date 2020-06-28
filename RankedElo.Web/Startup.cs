using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RankedElo.Core.Interfaces;
using RankedElo.Core.Services;
using Microsoft.Extensions.Hosting;
using RankedElo.Persistence;
using RankedElo.Persistence.Repositories;
using RankedElo.Web.Models;

namespace RankedElo.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IMatchService, MatchService>();
            services.AddScoped<IMatchRepository, MatchRepository>();
            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();

            services.AddDbContext<RankedEloDbContext>(options =>
                options.UseMySql("server=db;database=rankedelo;user=root;password=developer", providerOpt => providerOpt.EnableRetryOnFailure()));

            services.AddControllers()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<BaseMatchDtoValidator<BaseMatchDto>>());
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RankedEloDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("./swagger/v1/swagger.json", "Ranked Foosball V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseRouting();
            app.UseEndpoints(c => c.MapControllers());
            // Run all migrations on startup for simplicity
            context.Database.Migrate();
        }
    }
}
