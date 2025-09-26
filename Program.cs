using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IEnumerable<string>? initialScopes = builder.Configuration["DownstreamApi:Scopes"]?.Split(' ');

//Adiciona e define os dados básicos do Azure AD
builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration, "AzureAd")
    .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
        .AddDownstreamApi("GraphApi", builder.Configuration.GetSection("GraphApi"))
        .AddDownstreamApi("GraphApiGroup", builder.Configuration.GetSection("GraphApiGroup"))
        .AddDownstreamApi("GraphApiUsers", builder.Configuration.GetSection("GraphApiUsers"))
        .AddDownstreamApi("GraphApiTenant", builder.Configuration.GetSection("GraphApiTenant"))
        .AddInMemoryTokenCaches();

builder.Services.AddControllersWithViews()
                .AddMicrosoftIdentityUI();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
                .AddMicrosoftIdentityConsentHandler();

WebApplication app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
});

app.Run();
