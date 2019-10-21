using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;



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

// SWAGGER - Documentação 

// Instalamos o pacote
// dotnet add backend.csproj package Swashbuckle.AspNetCore -v 5.0.0-rc4

// JWT - JSON Web Token 

// Adicionamos o pacote JWT
// dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 3.0.0

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
            // Configuramos como os objetos relacionados aparecerão nos retornos

           services.AddControllersWithViews().AddNewtonsoftJson(
                opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            );

            // Configuramos o Swagger
            services.AddSwaggerGen(c => 
            {
                c.SwaggerDoc("v1", new OpenApiInfo{ Title = "API", Version = "v1"} );

                // Definimos o caminho e arquivo temporário de documentação
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

                // Definimos o caminho
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                
                c.IncludeXmlComments(xmlPath);
            });

            // Configuramos o JWT

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => 
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                    (Configuration["Jwt:Key"]))

                } 
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Usamos efetivamente o Swagger
            app.UseSwagger();
            // Especificamos o Endpoint na aplicação 
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });

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
