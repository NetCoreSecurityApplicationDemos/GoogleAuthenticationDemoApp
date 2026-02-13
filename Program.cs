using GoogleAuthenticationDemoApp.Data;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

namespace GoogleAuthenticationDemoApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            builder.Services.AddSwaggerGen();
            builder.Services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document,context,cancellationtoken) =>
                {
                    document.Info.Version = "10.0";
                    document.Info.Title = "Demo Volkan Tolkan API";
                    document.Info.Description = "Bunu yazan  tosun okuyana kosun !!!";
                    document.Info.TermsOfService = new Uri("https://a@b.com");

                    document.Info.Contact = new OpenApiContact
                    {
                        Name = "Volkan Genç",
                        Email = "gencvolkan@gmail.com",
                        Url = new Uri("https://a@b.com")
                    };
                    document.Info.License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }; 

                    return Task.CompletedTask;

                });

            });
            //Authentication
            builder.Services.AddAuthentication().AddGoogle(GoogleOptions =>
            {
                GoogleOptions.ClientId = builder.Configuration.GetValue<string>("OAuth:Google:ClientId")!;  // "884293809648-ob2jnalpmnl30lpf7v1i0sfvs2d1dnnd.apps.googleusercontent.com";
                GoogleOptions.ClientSecret = builder.Configuration.GetValue<string>("OAuth:Google:ClientSecret")!;
                //options.CallbackPath = "/signin-google";

                GoogleOptions.Scope.Add("email");
                GoogleOptions.Scope.Add("profile");
            });


            // Cookie ayarı (Correlation & SameSite problemi yaşamamak için)
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });





            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {

                app.UseSwagger();
                app.UseSwaggerUI();
                app.MapOpenApi();
                app.MapScalarApiReference();

                //app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Swagger"));
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
