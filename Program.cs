using clinic_assessment_redone.Helpers.Controllers;
using clinic_assessment_redone.Helpers.Controllers.Interfaces;
using clinic_assessment_redone.Data.Models.Db.Entity;
using Microsoft.EntityFrameworkCore;

using clinic_assessment_redone.Data.Models.Db.Repo;
using clinic_assessment.data;
using clinic_assessment_redone.Middleware.ErrorHandler;
using clinic_assessment.data.Models.Db.Repo.Interfaces;
using clinic_assessment.data.Models.Db.Entity;
using clinic_assessment.data.Models.Db.Repo;
using Microsoft.AspNetCore.Authorization;
using clinic_assessment_redone.Middleware.Authorization.Requirements;
using clinic_assessment_redone.Helpers.Misc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.Cookies;
using clinic_assessment_redone.Middleware.Authorization.Handler;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);


builder.Logging.ClearProviders();
builder.Logging.AddConsole();

/* Register services with net core */

builder.Services.AddControllers();

builder.Services.AddDbContext<DataContext>(
    o => o.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDB"))
    );

builder.Services.AddScoped<IRepository<Role, int>, RoleRepository>();
builder.Services.AddScoped<IRepository<ClinicUser, int>, ClinicUserRepository>();
builder.Services.AddScoped<IRepository<Employee, int>, EmployeeRepository>();
builder.Services.AddScoped<IRepository<Doctor, int>, DoctorRepository>();
builder.Services.AddScoped<IRepository<Patient, int>, PatientRepository>();
builder.Services.AddScoped<IRepository<Permission, int>, PermissionRepository>();
builder.Services.AddScoped<IRepository<Appointment, int>, AppointmentRepository>();
builder.Services.AddScoped<IAuthHelper, AuthHelper>();
builder.Services.AddScoped<IUserHelper, UserHelper>();
builder.Services.AddScoped<IDoctorHelper, DoctorHelper>();
builder.Services.AddScoped<IAppointmentHelper, AppointmentHelper>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthHandler>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

/* Add authentication schemes */
// Cookies
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
//        options =>
//        {
//            options.LoginPath = new PathString("/api/login");
//            options.AccessDeniedPath = new PathString("/auth/denied");
//        });

// JWT
//builder.Services.AddAuthentication( cfg =>
//    {
//        cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//        cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    }
//    )
//    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters()
//    {
//        ValidateIssuer = true,
//        ValidIssuer = builder.Configuration.GetSection("AppSettings:Issuer").Value,
//        ValidateAudience = false,
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
//                builder.Configuration.GetSection("AppSettings:SecretKey").Value))
//    });

// Azure AD Authentication
builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration, "AzureAd");

/* Register role permissions */
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Consts.PERMISSION_DOCTOR_BUSY_APT_COUNT, builder =>
    {
        builder.AddRequirements(new PermissionRequirement(Consts.PERMISSION_DOCTOR_BUSY_APT_COUNT));
    });

    options.AddPolicy(Consts.PERMISSION_DOCTOR_BUSY_MIN_HOURS, builder =>
    {
        builder.AddRequirements(new PermissionRequirement(Consts.PERMISSION_DOCTOR_BUSY_MIN_HOURS));
    });

    options.AddPolicy(Consts.PERMISSION_DOCTOR_AVAILABLE_SLOTS, builder =>
    {
        builder.AddRequirements(new PermissionRequirement(Consts.PERMISSION_DOCTOR_AVAILABLE_SLOTS));
    });

    options.AddPolicy(Consts.PERMISSION_DOCTORS_AVAILABLE_SLOTS, builder =>
    {
        builder.AddRequirements(new PermissionRequirement(Consts.PERMISSION_DOCTORS_AVAILABLE_SLOTS));
    });

    options.AddPolicy(Consts.PERMISSION_APPOINTMENT_CREATE, builder =>
    {
        builder.AddRequirements(new PermissionRequirement(Consts.PERMISSION_APPOINTMENT_CREATE));
    });

    options.AddPolicy(Consts.PERMISSION_APPOINTMENT_CANCEL, builder =>
    {
        builder.AddRequirements(new PermissionRequirement(Consts.PERMISSION_APPOINTMENT_CANCEL));
    });

    options.AddPolicy(Consts.PERMISSION_APPOINTMENT_DETAILS, builder =>
    {
        builder.AddRequirements(new PermissionRequirement(Consts.PERMISSION_APPOINTMENT_DETAILS));
    });

    options.AddPolicy(Consts.PERMISSION_PATIENT_APPOINTMENT_HISTORY, builder =>
    {
        builder.AddRequirements(new PermissionRequirement(Consts.PERMISSION_PATIENT_APPOINTMENT_HISTORY));
    });


});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
