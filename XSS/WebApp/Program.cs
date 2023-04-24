using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddRazorPages();

var app = builder.Build();


app.MapGet("/api/user", () => CustomUser.CurrentCustomUser);

app.MapGet("/api/update-name", () =>
{
    CustomUser.CurrentCustomUser.Name = "<script>alert('injected!')</script>";
    return "good!";
});

app.MapGet("/api/update-image", () =>
{
    CustomUser.CurrentCustomUser.Image = "https://idontexist1234.com/totally\" onerror=\"alert('injected!')\"";
    return "good!";
});


app.MapRazorPages();

app.MapHub<ChatHub>("/chat");

app.Run();


public class CustomUser
{
    public static CustomUser CurrentCustomUser = new CustomUser()
    {
        Name = "<strong>Tony</strong>",
        Image = "https://avatars.githubusercontent.com/u/16464160",
    };

    public string Name { get; set; }
    public string Image { get; set; }
}

public class ChatHub : Hub
{
    public Task SendMessage(string msg)
    {
        return Clients.All.SendAsync("accept_message", msg);
    }
}