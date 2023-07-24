using Application;
using Infrastructure;
using Mapster;
using MapsterMapper;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

{
    builder.Services.AddHttpContextAccessor();
}

{
    var config = TypeAdapterConfig.GlobalSettings;
    config.Scan(typeof(Program).Assembly);

    builder.Services.AddSingleton(config);
    builder.Services.AddScoped<IMapper, ServiceMapper>();
}

{
    builder.Services.AddInfrastructure(builder.Configuration)
         .AddApplication();
}


var app = builder.Build();


app.UseExceptionHandler("/error");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers().RequireAuthorization();

app.Run();
