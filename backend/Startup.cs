using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


// Pra chegar nesse processo primeiro instalamos a 

// Instalamos o Entity Framework (gera os models do nosso banco)
// dotnet tool install --global dotnet-ef
//                     (instalar de maneira global (na maquina) para usarmos em todos os projetos) 

// Baixamos o SQL Server do Entity Framework
// dotnet add package Microsoft.EntityFrameworkCore.SqlServer

// Baixamos o pacote que irá escrever os nosso códigos
// dotnet add package Microsoft.EntityFrameworkCore.Design

// Testamos a instalação do EntityFramework
// dotnet ef

// Código que criará o nosso Contexto da Base de Dados e nossos Models (a história do nosso banco)
// dotnet ef dbcontext scaffold "Server=DESKTOP-2R9DLMI\SQLEXPRESS; Database=Gufos; User Id=sa; Password=132 "Microsoft.EntityFrameworkCore.SqlServer -o Models -d
// -o (cria o diretorio/pasta) -d (cria as data notations ex: )

namespace backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           services.AddControllersWithViews().AddNewtonsoftJson(
                opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
