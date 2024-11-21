using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        // Adiciona suporte a controllers
        services.AddControllers();

       
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage(); // Página detalhada de erros durante o desenvolvimento
          
        }

        app.UseHttpsRedirection(); // Redireciona HTTP para HTTPS
        app.UseRouting(); // Habilita o roteamento

        app.UseAuthorization(); // Middleware para autorização

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers(); // Mapeia os controllers
        });
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>(); // Define o Startup
            });
}
