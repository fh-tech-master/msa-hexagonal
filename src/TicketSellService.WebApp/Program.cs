using TicketSellService.Domain;
using TicketSellService.Domain.Ports;
using TicketSellService.Infrastructure.Adapters;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.Services.AddScoped<IAccountingService, AccountingService>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<ITicketShelfService, TicketShelfService>();
builder.Services.AddScoped<IEventPublisher, EventPublisher>();

app.MapGet("/", () => "Hello World!");

app.Run();