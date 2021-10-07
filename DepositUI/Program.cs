using DepositUI.AutoMapper;
using DepositUI.BLL.Interfaces;
using DepositUI.BLL.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DepositUI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<IDepositService, DepositService>();
            builder.Services.AddAutoMapper(typeof(MapperProfile));

            await builder.Build().RunAsync();
        }
    }
}
