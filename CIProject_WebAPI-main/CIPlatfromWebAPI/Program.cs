using Business_logic_Layer;
using Data_Access_Layer.JWTService;
using Data_Access_Layer;
using Data_Access_Layer.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization: 'Bearer {your JWT token}'",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    Array.Empty<string>()
                }
            });
});

// Other service configurations


builder.Services.AddDbContext<AppDbContext>(db => db.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "localhost",
        ValidAudience = "localhost",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Key"])),
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddScoped<BALLogin>();
builder.Services.AddScoped<DALLogin>();
builder.Services.AddScoped<BALAdminUser>();
builder.Services.AddScoped<DALAdminUser>();
builder.Services.AddScoped<BALMissionSkill>();
builder.Services.AddScoped<DALMissionSkill>();
builder.Services.AddScoped<BALMissionTheme>();
builder.Services.AddScoped<DALMissionTheme>();
builder.Services.AddScoped<JwtService>();

builder.Services.AddCors(option =>
{
    option.AddPolicy("MyPolicy", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
/*builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200/")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});*/


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
app.UseCors("MyPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

