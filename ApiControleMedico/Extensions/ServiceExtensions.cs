using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiControleMedico.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddScoped<PacienteService>();
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
