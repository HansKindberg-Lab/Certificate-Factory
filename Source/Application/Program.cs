using System.ComponentModel;
using System.Net;
using Application.Models.ComponentModel;
using Application.Models.DependencyInjection.Extensions;
using IPNetwork = Microsoft.AspNetCore.HttpOverrides.IPNetwork;

var webApplicationBuilder = WebApplication.CreateBuilder(args);

TypeDescriptor.AddAttributes(typeof(IPAddress), new TypeConverterAttribute(typeof(IpAddressTypeConverter)));
TypeDescriptor.AddAttributes(typeof(IPNetwork), new TypeConverterAttribute(typeof(IpNetworkTypeConverter)));

webApplicationBuilder.Services.AddCertificateFactory(webApplicationBuilder.Configuration);

webApplicationBuilder.Services.Configure<ForwardedHeadersOptions>(options =>
{
	options.AllowedHosts.Clear();
	options.KnownNetworks.Clear();
	options.KnownProxies.Clear();
});

var forwardedHeadersSection = webApplicationBuilder.Configuration.GetSection("ForwardedHeaders");
webApplicationBuilder.Services.Configure<ForwardedHeadersOptions>(forwardedHeadersSection);

webApplicationBuilder.Services.AddControllersWithViews();

var webApplication = webApplicationBuilder.Build();

webApplication
	.UseDeveloperExceptionPage()
	.UseForwardedHeaders()
	.UseStaticFiles()
	.UseRouting();

webApplication.MapDefaultControllerRoute();

webApplication.Run();