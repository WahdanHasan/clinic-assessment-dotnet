using clinic_assessment_redone.Helpers.Controllers;
using clinic_assessment_redone.Helpers.Controllers.Interfaces;
using clinic_assessment_redone.Data.Models.Db.Entity;
using clinic_assessment_redone.Data.Models.Db.Repo.Interfaces;
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
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
//using clinic_assessment_redone.Middleware.Authorization.Filter;
using Microsoft.AspNetCore.Mvc.Filters;
using clinic_assessment_redone.Middleware.Authorization.Handler;
using Microsoft.AspNetCore.Http.Json;
using System.Text.Json;
using clinic_assessment_redone.Helpers.Converter;

var builder = WebApplication.CreateBuilder(args);


builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.

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

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthHandler>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
        options =>
        {
            options.LoginPath = new PathString("/api/login");
            options.AccessDeniedPath = new PathString("/auth/denied");
        });

builder.Services.AddAuthentication( cfg =>
    {
        cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }
    )
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration.GetSection("AppSettings:Issuer").Value,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                builder.Configuration.GetSection("AppSettings:SecretKey").Value))
    });


//var serializeOptions = new JsonSerializerOptions
//{
//    WriteIndented = true,
//    Converters =
//    {
//        new DateOnlyConverter()
//    }
//};

// Register permissions
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Constants.PERMISSION_DOCTOR_BUSY_APT_COUNT, builder =>
    {
        builder.AddRequirements(new PermissionRequirement(Constants.PERMISSION_DOCTOR_BUSY_APT_COUNT));
    });

    options.AddPolicy(Constants.PERMISSION_DOCTOR_BUSY_MIN_HOURS, builder =>
    {
        builder.AddRequirements(new PermissionRequirement(Constants.PERMISSION_DOCTOR_BUSY_MIN_HOURS));
    });

    options.AddPolicy(Constants.PERMISSION_DOCTOR_AVAILABLE_SLOTS, builder =>
    {
        builder.AddRequirements(new PermissionRequirement(Constants.PERMISSION_DOCTOR_AVAILABLE_SLOTS));
    });

    options.AddPolicy(Constants.PERMISSION_DOCTORS_AVAILABLE_SLOTS, builder =>
    {
        builder.AddRequirements(new PermissionRequirement(Constants.PERMISSION_DOCTORS_AVAILABLE_SLOTS));
    });

    options.AddPolicy(Constants.PERMISSION_APPOINTMENT_CREATE, builder =>
    {
        builder.AddRequirements(new PermissionRequirement(Constants.PERMISSION_APPOINTMENT_CREATE));
    });

    options.AddPolicy(Constants.PERMISSION_APPOINTMENT_CANCEL, builder =>
    {
        builder.AddRequirements(new PermissionRequirement(Constants.PERMISSION_APPOINTMENT_CANCEL));
    });

    options.AddPolicy(Constants.PERMISSION_APPOINTMENT_DETAILS, builder =>
    {
        builder.AddRequirements(new PermissionRequirement(Constants.PERMISSION_APPOINTMENT_DETAILS));
    });

    options.AddPolicy(Constants.PERMISSION_PATIENT_APPOINTMENT_HISTORY, builder =>
    {
        builder.AddRequirements(new PermissionRequirement(Constants.PERMISSION_PATIENT_APPOINTMENT_HISTORY));
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

app.UseAuthorization();

app.MapControllers();

app.Run();
