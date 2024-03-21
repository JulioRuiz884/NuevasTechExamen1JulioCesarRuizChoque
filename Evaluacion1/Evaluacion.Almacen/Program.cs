using Evaluacion.Almacen.Contratos;
using Evaluacion.Almacen.Implementacion;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddScoped<IProductoRepositorio, ProductoRepositorio>();
    })
    .Build();

host.Run();
