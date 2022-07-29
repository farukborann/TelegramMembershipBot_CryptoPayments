var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

TelegramMemberShipBot_CryptoPayments.SettingsManager.LoadSettings();

TelegramMemberShipBot_CryptoPayments.Database.DatabaseContext DbContext = new TelegramMemberShipBot_CryptoPayments.Database.DatabaseContext();
DbContext.CheckDatabaseSettings();
DbContext.Dispose();

TelegramMemberShipBot_CryptoPayments.Coinpayments.Main.Setup_CPM();
TelegramMemberShipBot_CryptoPayments.Telegram.Main.StartBot();

Console.WriteLine("Welcome to Telegram Cryptopayments Membership Bot\nYou can reach the admin panel from the links below.\nYou can adjust the settings using the \"settings.json\" file in the application folder.\nYou must restart the bot for the settings changes to take effect.\nPlease use \"Ctrl+C\" keys to terminate the bot.");

app.Run();