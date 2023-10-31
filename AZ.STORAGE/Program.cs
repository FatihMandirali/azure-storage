using System.Text;
using AZ.STORAGE;
using AZ.STORAGE.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(typeof(INoSqlStorage<>), typeof(TableStorageService<>));
builder.Services.AddScoped<IBlobStorage, BlobStorageService>();

//--------
AzQueueService queue = new AzQueueService("ornekkuyruk");
//To Queue send message
//string base64message = Convert.ToBase64String(Encoding.UTF8.GetBytes("fatihmandirali"));
//queue.SendMessageAsync(base64message).Wait();

//Receive message from Queue
var message = queue.RetrieveNextMessageAsync().Result;
string text = Encoding.UTF8.GetString(Convert.FromBase64String(message.MessageText));


//Delete message FROM QUEUE
//await queue.DeleteMessage(message.MessageId, message.PopReceipt);
//----------
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