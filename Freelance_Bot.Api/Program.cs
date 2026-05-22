//using Freelance_Bot.Infrastruction;
//using Freelance_Bot.Infrastruction.DB;
//using Freelance_bot.Application;
//using Microsoft.EntityFrameworkCore;

//var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddDbContext<FreelancerDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
//);

//builder.Services.AddControllers();



//#region Dependency Injection
//builder.Services.AddModuleInfrastructureDepedences();
//builder.Services.AddModuleApplicationDependancyInjection();
//#endregion



//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();


//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();       // ✅
//    app.UseSwaggerUI();     // ✅
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();
using Freelance_Bot.Infrastruction;
using Freelance_Bot.Infrastruction.DB;
using Freelance_bot.Application;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FreelancerDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

#region Dependency Injection

builder.Services.AddModuleInfrastructureDepedences();

builder.Services.AddModuleApplicationDependancyInjection();

#endregion

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();