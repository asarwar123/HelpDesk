using Azure.Storage.Blobs;
using HelpDesk.api.DBContexts;
using HelpDesk.api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson()
  .AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// builder.Services.AddDbContext<DataBaseContext>(
    // dbContextOptions => dbContextOptions.UseSqlite(builder.Configuration["ConnectionStrings:HelpDeskSQLiteConectionString"]));
	
builder.Services.AddDbContext<DataBaseContext>(
    dbContextOptions => dbContextOptions.UseSqlServer(builder.Configuration["ConnectionStrings:HelpDeskAzureConectionString"]));

builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped(_ =>
{
    return new BlobServiceClient(builder.Configuration["Azure:AzureBlobStorage:ConnectionString"]);
});

builder.Services.AddScoped<IFileUpload, FileUpload>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
