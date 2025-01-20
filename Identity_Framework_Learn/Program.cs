using Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Service;
using Service.UserGroup;
using System.Reflection;
using System.Text;
using AutoMapper;
using data.viewmodel.models;
using Data.Profiles;
using MediatR;
using Service.Query.User;
using Service.Commands.Base;
using Service.RequestModel;
//using Service.Commands.UserHandler;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("myconn"));
//});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Add services to the container.
var connectionStr = builder.Configuration.GetConnectionString("DefaultConnection");
var appSetttings = builder.Configuration.GetSection("AppSettings").Get<AppSettings>() ?? new AppSettings();
builder.Services.AddSingleton(appSetttings);
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionStr, x => x.MigrationsAssembly("Data")));
builder.Services.AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddSignInManager()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>("APP");



builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromSeconds(appSetttings.RefreshTokenExpireSeconds);
});

builder.Services.AddAutoMapper(typeof(RoleMapping));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            RequireExpirationTime = true,
            ValidIssuer = appSetttings.Issuer,
            ValidAudience = appSetttings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSetttings.SecretKey)),
            ClockSkew = TimeSpan.FromSeconds(0)
        };
    });

//builder.Services.AddScoped<ApplicationDbContextInitialiser>();
builder.Services.AddScoped<GetUserDatas>();

builder.Services.AddMediatR(typeof(GetbyIdQuery));



builder.Services.AddTransient<UserService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new OpenApiInfo() { Title = "App Api", Version = "v1" });
    config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    config.AddSecurityRequirement(new OpenApiSecurityRequirement{
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
});

// Register the handler for MyCommand
//builder.Services.AddTransient<IRequestHandler<MyCommand, MyResponse>, MyCommandHandler>();


builder.Services.AddScoped<PostUserDatas>();
builder.Services.AddScoped<DeleteUserDatas>();
builder.Services.AddScoped<PutUserDatas>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //using (var scope = app.Services.CreateScope())
    //{
    //    var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
    //    await initialiser.InitialiseAsync();
    //    await initialiser.SeedAsync();
    //}
}

app.UseHttpsRedirection();

app.UseCors("MyPolicy");
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();



