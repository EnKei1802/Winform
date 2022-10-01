using ExcerciseTwo.ProductForm;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ExcerciseTwo.Sevices;
using ExcerciseTwo.Repositorys;

namespace ExcerciseTwo
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            //ApplicationConfiguration.Initialize();
            //Application.Run(new ProductDetail());

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var host = CreateHostBuilder().Build();
            serviceProvider = host.Services;

            Application.Run(serviceProvider.GetRequiredService<ListProduct>());
        }

        public static IServiceProvider serviceProvider { get; private set; }
        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) => {
                services.AddTransient<ICategoryService, CategoryService>();
                services.AddTransient<ICategoryRepository, CategoryRepository>();
                services.AddTransient<IProductService, ProductService>();
                services.AddTransient<IProductRepository, ProductRepository>();
                services.AddTransient<ProductDetail>();
                services.AddTransient<ListProduct>();
            });
        }
    }
}