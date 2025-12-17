using EdgIQ.Inventory.Infrastructure;
using EdgIQ.Inventory.Infrastructure.Data;
using EdgIQ.Inventory.Infrastructure.Identity;
using EdgIQ.Inventory.WebAPI.Authentication;
using EdgIQ.Inventory.WebAPI.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure data protection, setup the application discriminator
// so that the data protection keys can be shared between the BFF and this API
builder.Services.AddDataProtection(o => o.ApplicationDiscriminator = "EdgIQInventoryAPI");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

    // Default for interactive logins
    options.DefaultScheme = Microsoft.AspNetCore.Identity.IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = Microsoft.AspNetCore.Identity.IdentityConstants.ExternalScheme;
})
//.AddBearerToken(Microsoft.AspNetCore.Identity.IdentityConstants.BearerScheme) 
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]!)),
        ClockSkew = TimeSpan.Zero
    };
})
.AddCookie(Microsoft.AspNetCore.Identity.IdentityConstants.ApplicationScheme)
.AddCookie(Microsoft.AspNetCore.Identity.IdentityConstants.ExternalScheme)
.AddMicrosoftAccount(options =>
{
    options.SignInScheme = Microsoft.AspNetCore.Identity.IdentityConstants.ExternalScheme;
    options.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"]!;
    options.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"]!;
})
.AddGoogle(options =>
{
    options.SignInScheme = Microsoft.AspNetCore.Identity.IdentityConstants.ExternalScheme;
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
});

//builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
//builder.Services.AddScoped<TokenGenerator>();

builder.Services.AddAuthorizationBuilder().AddCurrentUserHandler();

// Configure identity
builder.Services.AddIdentityCore<ApplicationUser>(options =>
options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<InventoryDbContext>()
    .AddApiEndpoints();

builder.Services.AddPostgreSQLServices(builder.Configuration);

// State that represents the current user from the database *and* the request
builder.Services.AddCurrentUser();

// Configure Open API
builder.Services.AddOpenApi(options => options.AddBearerTokenAuthentication());

// Configure rate limiting
builder.Services.AddRateLimiting();

builder.Services.AddHttpLogging(o =>
{
    if (builder.Environment.IsDevelopment())
    {
        o.CombineLogs = true;
        o.LoggingFields = HttpLoggingFields.ResponseBody | HttpLoggingFields.ResponseHeaders;
    }
});

// Add a CORS policy for the client
builder.Services.AddCors(options => options.AddPolicy("KonnektCORS",
        policy => policy.WithOrigins(builder.Configuration["FrontendUrl"]!)
        //policy => policy.WithOrigins()
            .AllowAnyMethod()
            .AllowAnyHeader()));

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

var app = builder.Build();

// Apply migrations
await app.ApplyMigrations();

// Seed demo user
await app.SeedDemoUserAsync();

app.UseHttpLogging();
app.UseRateLimiter();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference(options =>
    {
        options.Servers = [];
        options.Authentication = new() { PreferredSecuritySchemes = [Microsoft.AspNetCore.Identity.IdentityConstants.BearerScheme] };
    });
}

// Activate the CORS policy
app.UseCors("KonnektCORS");
app.UseResponseCompression();
app.MapOpenApi();

//app.MapDefaultEndpoints();
app.AddEndpoints();
app.Map("/", () => Results.Redirect("/scalar/v1"));

app.Run();

