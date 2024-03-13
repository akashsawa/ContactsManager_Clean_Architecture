using CrudExample.Controllers;
using ServiceContracts;
using Services;
using Microsoft.EntityFrameworkCore;
using Entities;
using Repositories;
using RepositoryContracts;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using CrudExample.Middleware;
using ContactsManager.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);
//add services into ioc container
//builder.Services.AddSingleton<ICountriesService, CountriesService>();
//builder.Services.AddSingleton<IPersonsService, PersonsService>();

//logging 
//builder.Host.ConfigureLogging(loggingProviders => { 
//    loggingProviders.ClearProviders();  // clearing all default logging providers like console, debug, eventlog

//    loggingProviders.AddConsole(); // means logs message will bedisplayed in console window only and not in debug window.

//    loggingProviders.AddEventLog();
//});
//logging

//replacing logging with serilog : serilog
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(context.Configuration). // read configuration settings from built in iconfiguration 
    ReadFrom.Services(services); ////readout   current app's services and make them available to serilog. 
});
//replacing logging with serilog : serilog

builder.Services.AddControllersWithViews();



builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();

builder.Services.AddScoped<IPersonsRepository, PersonsRepository>();

builder.Services.AddScoped<ICountriesService, CountriesService>();
//builder.Services.AddScoped<IPersonsService, PersonsService>();
builder.Services.AddScoped<IPersonsAdderService, PersonsAdderService>();
//builder.Services.AddScoped<IPersonsGetterService, PersonsGetterService>();

//fir OCP principle 
builder.Services.AddScoped<PersonsGetterService, PersonsGetterService>();
builder.Services.AddScoped<IPersonsGetterService, PersonsGetterServiceWithFewExcelFields>();
//for OCP principle

//for ocp inheritance
//builder.Services.AddScoped<PersonsGetterServiceChild, PersonsGetterServiceChild>();
//builder.Services.AddScoped<IPersonsGetterService, PersonsGetterServiceChild>();
//

builder.Services.AddScoped<IPersonsDeleterService, PersonsDeleterService>();
builder.Services.AddScoped<IPersonsUpdaterService, PersonsUpdaterService>();
builder.Services.AddScoped<IPersonsSorterService, PersonsSorterService>();




builder.Services.AddDbContext<ApplicationDBContext>(
    options =>
    {
        options.UseSqlServer(builder
            .Configuration.GetConnectionString("DefaultConnectionString")); // for using sql server or any other sql
    });

//Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PersonsDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False

//addidentity

//password complexity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options => { options.Password.RequiredLength = 5; options.Password.RequireNonAlphanumeric = false; options.Password.RequireUppercase = false; options.Password.RequireLowercase = true; options.Password.RequireDigit = false; options.Password.RequiredUniqueChars = 3; /*eg: ab12ab */ }).AddEntityFrameworkStores<ApplicationDBContext>().AddDefaultTokenProviders().AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDBContext, Guid>>().AddRoleStore<RoleStore<ApplicationRole, ApplicationDBContext, Guid>>();
//password complexity
// hey asp.net core i would like to add identity  services , and the model class for storing user details we are using class applicationuser. using applicationuser class for creating users table and applicationrole class for creating roles table and use ApplicationDBContext. 
// AddUserStore<ApplicationUser, ApplicationRole,ApplicationDBContext, Guid>()  :  use this method for providing the repository layer.
//AddRoleStore<RoleStore< ApplicationRole, ApplicationDBContext, Guid>>() : form manipulating rolesstore data.
//AddDefaultTokenProviders : at time of login email confirmation or phone verification we have to generate tokens randomly at run time and send the same token to user so to generate the sam e tokens we have to add this statement.                                                                                                                             //addidenttiy


//for authentication/authorization
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

    //for CUSTOM AUTHORIZATION POLICIES
    options.AddPolicy("NotAuthenticated", policy =>
    {
        policy.RequireAssertion(context =>
        { 
            return !context.User.Identity.IsAuthenticated;  // means user has access to action method , and if false means there is no access.*/
            /*return true;*/
        });
    });
    //for CUSTOM AUTHORIZATION POLICIES

}); // means request should have identity cookie for accessing any action method if not then its assumed as unauthorized. this will add the policy for all the actionmethod sincluding homecontrollers in controller and personscontroller

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
}); // means if user is not logfed in then automatically redirect to this url.
//for authentication/authorization

//http logging options
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders; // means only showing particular properties.
});
//http logging options

var app = builder.Build();

//HTTPS
app.UseHsts(); // hsts: http strict transport security
app.UseHttpsRedirection();
//HTTPS

//idiagnosticcontext
app.UseSerilogRequestLogging();
//idiagnosticcontext


//http logging
app.UseHttpLogging();

//http logging

//app.MapGet("/", () => "Hello World!");

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error"); // for showing error page useexceptionhandler()
    app.UseMiddleware();
}

//app.Logger.LogDebug("debug_message"); // this wont be present on console.
//app.Logger.LogInformation("information_message");
//app.Logger.LogWarning("warning_message");
//app.Logger.LogCritical("critical_message");
//app.Logger.LogError("error_message");


//for intergartion testing
if (builder.Environment.IsEnvironment("Test") == false)
{
    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");  // for loading the exe file at run tiem and converting html content  to pdf.
}

app.UseStaticFiles();

//for signin manager : this is for reading the identity cookie and can extract the user id and username form that.



//for signin manager 

app.UseRouting(); ///identifying action methods based on the route.

///for authorization
app.UseAuthentication(); //reading identity cookie
app.UseAuthorization(); //validates access permissions of the user.
//for authorization

app.MapControllers(); // executes filter pipeline (action + filters)

//conventional roiuting  : here after using this we dont need to add [route] over the controller class.
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(name: "default", pattern: "{controller}/{action}/{id?}"); // means id is optional to pass.
//});

//areas conventional routing
app.UseEndpoints(options =>
{
    options.MapControllerRoute(
        name: "areas", pattern: "{area:exists}/{controller=Home}/{action=Index}"); // area:exists means area name is requried its mandatory . exists is predefined route constraint which says this parameter (area) is mandatory. "Home" controller and "Index" action method are default .
    // eg. :Admin/Home/Index
});
//areas conventional routing

//conventional routing 

app.Run();

// for integration testing
public partial class Program { }  // make the autogenerated program  accessible programmatically. the progream 

// for integration testing