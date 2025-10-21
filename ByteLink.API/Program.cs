using ByteLink.API.Mapper.Profiles;
using ByteLink.API.Validators;
using ByteLink.Application.Comparators;
using ByteLink.Application.Generators;
using ByteLink.Application.HostedServices;
using ByteLink.Application.Mediator.Commands;
using ByteLink.Domain.Comparators;
using ByteLink.Domain.Entities;
using ByteLink.Domain.Enums;
using ByteLink.Domain.Exceptions;
using ByteLink.Domain.Generators;
using ByteLink.Domain.Settings;
using ByteLink.Infrastructure;
using ByteLink.Infrastructure.Persistence.Context.Application;
using ByteLink.Infrastructure.Persistence.Context.Base;
using ByteLink.Infrastructure.Persistence.Context.Tenant;
using ByteLink.Infrastructure.Persistence.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//Login
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("Auth");
    var key = jwtSettings["Key"] ?? throw new MissingConfigurationException("Key");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});

builder.Services.AddHttpContextAccessor();

// Validators
builder.Services.AddScoped<IValidator<RegisterUserCommand>, RegisterUserValidator>();
builder.Services.AddFluentValidationAutoValidation();

// Database Contexts
builder.Services.AddDbContext<TenantDbContext>();
builder.Services.AddDbContext<ApplicationDbContext>();

builder.Services.AddScoped<IAsyncDbContextFactory<ApplicationDbContext>, ApplicationDbContextFactory>();

// Repositories
builder.Services.AddScoped<ITenantRepository, TenantRepository>();
builder.Services.AddScoped<IUrlRepository, UrlRepository>();
builder.Services.AddScoped<IUrlVisitRepository, UrlVisitRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddSingleton<IApplicationHttpContext, ApplicationHttpContext>();

// Generators
builder.Services.AddKeyedTransient<IGenerator<string, string>, ShortCodeGenerator>(GeneratorKeyedServices.ShortCodeGenerator);
builder.Services.AddKeyedTransient<IGenerator<Url, string>, ShortCodeUrlGenerator>(GeneratorKeyedServices.ShortCodeUrlGenerator);
builder.Services.AddKeyedTransient<IGenerator<string, string>, PasswordHashGenerator>(GeneratorKeyedServices.PasswordHashGenerator);
builder.Services.AddKeyedTransient<IGenerator<string, string>, JwtTokenGenerator>(GeneratorKeyedServices.JwtTokenGenerator);
builder.Services.AddKeyedTransient<IGenerator<ApplicationUser, string>, UserDatabaseConnectionStringGenerator>(GeneratorKeyedServices.UserDatabaseConnectionStringGenerator);
builder.Services.AddKeyedTransient<IGenerator<string, string>, DatabaseNameGenerator>(GeneratorKeyedServices.DatabaseNameGenerator);
builder.Services.AddKeyedTransient<IGenerator<string, string>, DatabasePwdGenerator>(GeneratorKeyedServices.DatabasePwdGenerator);
builder.Services.AddKeyedTransient<IGenerator<string, string>, DatabaseUserNameGenerator>(GeneratorKeyedServices.DatabaseUserNameGenerator);
builder.Services.AddKeyedTransient<IGenerator<string, long>, UserIdGenerator>(GeneratorKeyedServices.UserIdGenerator);
builder.Services.AddKeyedTransient<IGenerator<long, string>, UserSqidGenerator>(GeneratorKeyedServices.UserSqidGenerator);

// Validators
builder.Services.AddKeyedTransient<IComparator<string>, PasswordValidator>(ComparatorKeyedServices.PasswordValidator);

// Services
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("ByteLink.Application")));
builder.Services.AddAutoMapper(typeof(UrlProfile), typeof(UserProfile)); // TODO: Change to Assembly loading instead.
builder.Services.AddMemoryCache();

// App Settings
builder.Services.Configure<ByteLinkAppSettings>(builder.Configuration.GetRequiredSection("ByteLinkSettings"));
builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IOptions<ByteLinkAppSettings>>().Value);

builder.Services.Configure<ByteLinkAuthSettings>(builder.Configuration.GetRequiredSection("Auth"));
builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IOptions<ByteLinkAuthSettings>>().Value);

// Background Services
builder.Services.AddSingleton<IVisitUrlCommandQueue, VisitInsertHostedService>();
builder.Services.AddHostedService<VisitUrlCommandHostedService>();

var app = builder.Build();

app.UseAuthentication();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
