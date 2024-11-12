using Azure.Storage.Blobs;
using Azure.Storage.Files.Shares;
using Azure.Storage.Queues;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ABC_Retail.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure service clients for Azure Blob, File Share, and Queue services
builder.Services.AddSingleton(sp =>
{
    var blobServiceClient = new BlobServiceClient(builder.Configuration.GetConnectionString("AzureBlobStorage"));
    var shareServiceClient = new ShareServiceClient(builder.Configuration.GetConnectionString("AzureFileShare"));
    var queueServiceClient = new QueueServiceClient(builder.Configuration.GetConnectionString("AzureQueueStorage"));
    return new StorageService(blobServiceClient, shareServiceClient, queueServiceClient);
});

// Add controllers and views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
