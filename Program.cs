using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudySync;
using StudySync.Contexts;
using StudySync.Services;

var builder = WebApplication.CreateBuilder(args);;

// Add services to the container.

builder.Services.AddCors(o => { o.AddPolicy("AllowAny", p => { p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin(); }); });

var key = builder.Configuration["JWT:Key"] ?? throw new Exception("NULL JWT KEY.");
builder.Services.AddAuthentication(o => {
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(o => {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer      = Constants.API_URL,
            ValidAudience    = Constants.API_URL,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

builder.Services.AddDbContext<AppDbCtx>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection")));

// builder.Services.AddScoped<SvcBlogItem>();
builder.Services.AddScoped<SvcCalendar>();
builder.Services.AddScoped<SvcTimeRecord>();
builder.Services.AddScoped<SvcUser>();

builder.Services.AddControllers();
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

app.UseHttpsRedirection();

app.UseCors("AllowAny");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
