using FluentValidation.AspNetCore;
using FluentValidation;
using Hei_Hei_Api.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Hei_Hei_Api.Validators;
using Hei_Hei_Api.Services.Infrastructure.Implementations;
using Hei_Hei_Api.Services.Infrastructure.Abstractions;
using Hei_Hei_Api.Services.Application.Abstractions;
using Hei_Hei_Api.Services.Application.Implementations;
using Hei_Hei_Api.Middlewares;
using Hei_Hei_Api.Middleware;
using Hei_Hei_Api.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter JWT token"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:Key"]
                    ?? throw new InvalidOperationException("JWT Key is not configured.")))
        };
    });

builder.Services.AddHostedService<LogUploadBackgroundService>();

builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ILoggerService, LoggerService>();
builder.Services.AddScoped<IS3Service, S3Service>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAnimatorService, AnimatorService>();
builder.Services.AddScoped<IHeroService, HeroService>();
builder.Services.AddScoped<IPackageService, PackageService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IOrderAnimatorService, OrderAnimatorService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AnimatorValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<VerifyEmailValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateHeroRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreatePackageRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateOrderStatusRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateHeroRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreatePaymentRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdatePaymentStatusRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateReviewRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateReviewRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateOrderAnimatorRequestValidator>();

var app = builder.Build();

await AdminSeeder.SeedAsync(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<LoggingMiddleware>();

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
