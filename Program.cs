using webTest.Components;
using UserClient.Components.klient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<SocketClientService>();
builder.Services.AddHostedService<SocketClientService>(provider => provider.GetRequiredService<SocketClientService>());

var app = builder.Build();

var socketClientService = app.Services.GetRequiredService<SocketClientService>();
SocketClientHandler.Initialize(socketClientService);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();


