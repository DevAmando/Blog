using System.Text.Json.Serialization;
using System.Text;
using Blog.Data;
using Blog.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Blog;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;  


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddDbContext<BlogDataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddTransient<TokenService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(builder.Configuration["JwtKey"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });


builder.Services.AddAuthorization();

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options => { 

    options.SuppressModelStateInvalidFilter = true;

})
.AddJsonOptions(x => { 
    
    x.JsonSerializerOptions.ReferenceHandler =ReferenceHandler.IgnoreCycles;
    x.JsonSerializerOptions.DefaultIgnoreCondition =JsonIgnoreCondition.WhenWritingDefault;

});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

LoadConfiguration(app);

app.UseRouting();          
app.UseStaticFiles();      
app.UseAuthentication();   
app.UseAuthorization();    
app.MapRazorPages();       
app.MapControllers();      


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BlogDataContext>();
    DataSeeder.Seed(db);
}

if (app.Environment.IsDevelopment())
{
    Console.WriteLine("Desenvolvimento");
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

void LoadConfiguration(WebApplication app)
{
    Configuration.JwtKey = app.Configuration.GetValue<string>("JwtKey");
    Configuration.ApiKeyName = app.Configuration.GetValue<string>("ApiKeyName");
    Configuration.ApiKey = app.Configuration.GetValue<string>("ApiKey");

    var smtpConfig = new Configuration.SmtpConfiguration();
    app.Configuration.GetSection("SmtpConfiguration").Bind(smtpConfig);
    Configuration.Smtp = smtpConfig;
}
