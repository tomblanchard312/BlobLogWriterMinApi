using BlobLogWriterMinApi.Classes;

using Microsoft.Extensions.Azure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var blobServiceOptions = configuration.GetSection("BlobServiceOptions").Get<BlobServiceOptions>();

// Add services to the container.
builder.Services.AddSingleton(_ => blobServiceOptions);
builder.Services.AddSingleton<BlobService>();
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(blobServiceOptions.BlobConnectionString, preferMsi: true);
    clientBuilder.AddQueueServiceClient(blobServiceOptions.QueueConnectionString, preferMsi: true);
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Net Core Blob Log Writer Minimal API",
        Description = "An API that allows you to create/list/read log files in Azure Blob Storage",
        Contact = new OpenApiContact
        {
            Name = "Tom Blanchard",
            Email = "tomblanchard3@outlook.com"
        }
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/writelog", async (BlobService blobService, LogMessage log) =>
    {
        string logline = log.Message + "," + log.MessageType + "," + log.MessageDateTime;
        await blobService.LogAsync(logline);
        return Results.Created("/", "message created: " + log.Message);
    });

app.MapPost("/listlogs", (BlobService blobService, bool orderByDescending) =>
{
    var blobItems = blobService.ListBlobs(orderByDescending);
    // Handle and return the list of blob items as needed.
    return Results.Ok(blobItems);
});
app.MapPost("/readlog", (BlobService blobService, string blobName) =>
{
    var blobContent = blobService.ReadBlobData(blobName);
    if (blobContent != null)
    {
        return Results.Ok(blobContent);
    }
    else
    {
        return Results.NotFound(); // Return a 404 status if the blob does not exist.
    }
});
app.MapGet("/readlogserialized", (BlobService blobService, string blobName) =>
{
    var csvData = blobService.ReadBlobDataSerialized(blobName);

    if (csvData == null)
    {
        return Results.NotFound("CSV file not found");
    }

    return Results.Ok(csvData);
});
app.Run();
