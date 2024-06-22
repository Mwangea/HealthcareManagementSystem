using HealthcareManagementSystem.Data;
using HealthcareManagementSystem.Services;
using HealthcareManagementSystem.Servives;
using HealthcareManagementSystem.Servives.AdminService;
using HealthcareManagementSystem.Servives.AppointmentService;
using HealthcareManagementSystem.Servives.DoctorService;
using HealthcareManagementSystem.Servives.InvoiceServices;
//using HealthcareManagementSystem.Servives.LabaratoryService;
using HealthcareManagementSystem.Servives.LaboratoryService;
using HealthcareManagementSystem.Servives.MedicalService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HealthcareManagementSystem
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            // Configure DbContext with MySQL
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseMySql(
                    Configuration.GetConnectionString("DefaultConnection"),
                    ServerVersion.AutoDetect(Configuration.GetConnectionString("DefaultConnection"))
                );
            });

            var key = Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]);

            

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("Doctor", policy => policy.RequireRole("Doctor"));
                options.AddPolicy("Doctor,Admin", policy => policy.RequireRole("Doctor","Admin"));
            });

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IMedicalService, MedicalService>();
            services.AddScoped<ILaboratoryService, LaboratoryService>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

