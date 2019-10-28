using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiControleMedico.Modelos;
using ApiControleMedico.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;

namespace ApiControleMedico.Extensions
{
    public static class ServiceExtensions
    {

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
        }
        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options => { options.ForwardClientCertificate = false; });
        }

        public static void RegistraServices(this IServiceCollection services)
        {


            //in MongoDB I need to register class-maps dynamically that I don't need to register all the current and future-classes myself.

            //There is a Method called

            //BsonClassMap.RegisterClassMap<T>
            //but with this I need to know the Class (T) and can't use a Type instead.

            //I want to iterate through the assembly-types like this to register the class-maps:

            //var assemblyTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(t => t.GetTypes()).Where(t => t.IsClass);

            //var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => typeof(IService).IsAssignableFrom(p));

            //foreach (var type in types)
            //{
            //    services.AddScoped<Type>();


            //    //Register classmap for type
            //    if (BsonClassMap.IsClassMapRegistered(type))
            //        return;

            //    //will check if the type is registered. if not it will be automatically registered.
            //    //AutoMap will also called automatically.
            //    BsonClassMap.LookupClassMap(type);


            //}

            services.AddScoped<PacienteService>();
            services.AddScoped<ContaReceberService>();
            services.AddScoped<ContaPagarService>();
            services.AddScoped<FornecedorService>();
            services.AddScoped<PacienteService>();
            services.AddScoped<AppService>();
            services.AddScoped<CaixaService>();
            services.AddScoped<OficioService>();
            services.AddScoped<FuncionarioService>();
            services.AddScoped<UsuarioService>();
            services.AddScoped<LoginService>();
            services.AddScoped<MedicoService>();
            services.AddScoped<ConvenioService>();
            services.AddScoped<EnderecoService>();
            services.AddScoped<EspecialidadeService>();
            services.AddScoped<ProcedimentoService>();
            services.AddScoped<ConfiguracaoAgendaService>();
            services.AddScoped<LocalService>();
            services.AddScoped<CirurgiaService>();
            services.AddScoped<ExameService>();
            services.AddScoped<AgendamentoService>();
            services.AddScoped<ClinicaService>();
            services.AddScoped<FormaDePagamentoService>();
        }
    }
}
