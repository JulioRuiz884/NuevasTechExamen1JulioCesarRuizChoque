using Evaluacion.Almacen.Contratos;
using Evaluacion.Almacen.Modelo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace Evaluacion.Almacen.Endpoints
{
    public class ProveedorFunction
    {
        private readonly ILogger<ProveedorFunction> _logger;
        private readonly IProveedorRepositorio repositorio;

        public ProveedorFunction(ILogger<ProveedorFunction> logger, IProveedorRepositorio repositorio)
        {
            _logger = logger;
            this.repositorio = repositorio;
        }

        [Function("EliminarProveedor")]
        [OpenApiOperation("EliminarProveedor", "Eliminar Proveedor", Description = "Elimina un proveedor por su clave de partición y fila.")]
        [OpenApiParameter("id", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "ID del proveedor a eliminar.")]
        public async Task<HttpResponseData> EliminarProveedor([HttpTrigger(AuthorizationLevel.Function, "delete")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var datos = await req.ReadFromJsonAsync<Proveedor>() ?? throw new Exception("Debe ingresar un Proveedor con los datos a eliminar");
                string partitiokey = datos.PartitionKey;
                string rowkey = datos.RowKey;

                bool eliminado = await repositorio.Eliminar(partitiokey, rowkey, null);

                if (eliminado)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                else
                {
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                    return respuesta;
                }
            }
            catch (Exception)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }
        [Function("InsertarProveedor")]
        [OpenApiOperation("Listarespec", "InsertarProveedor", Description = "Sirve para insertar un proveedor")]
        [OpenApiRequestBody("application/json", typeof(Proveedor), Description = "Proveedor modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
                                 bodyType: typeof(Proveedor), Description = "Mostrara el proveedor creado")]
        public async Task<HttpResponseData> InsertarProveedor([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<Proveedor>() ?? throw new Exception("Debe ingresar un Proveedor con todos sus datos");
                registro.RowKey = Guid.NewGuid().ToString();
                registro.Timestamp = DateTime.UtcNow;
                bool sw = await repositorio.Insertar(registro);
                if (sw)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                else
                {
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                    return respuesta;
                }
            }
            catch (Exception)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }
        [Function("ModificarProveedor")]
        [OpenApiOperation("ModificarProveedor", "Modificar Proveedor", Description = "Modifica un proveedor existente.")]
        [OpenApiRequestBody("application/json", typeof(Proveedor), Description = "Datos del proveedor a modificar.")]
        public async Task<HttpResponseData> ModificarProveedor([HttpTrigger(AuthorizationLevel.Function, "put")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var datos = await req.ReadFromJsonAsync<Proveedor>() ?? throw new Exception("Debe ingresar los datos de un Proveedor a modificar");

                // Aquí deberías validar que la entidad a modificar tenga una clave de partición y una clave de fila.
                if (string.IsNullOrEmpty(datos.PartitionKey) || string.IsNullOrEmpty(datos.RowKey))
                {
                    throw new Exception("El Proveedor debe tener una clave de partición y una clave de fila.");
                }

                bool modificado = await repositorio.Modificar(datos);

                if (modificado)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                else
                {
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                    return respuesta;
                }
            }
            catch (Exception)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }
        [Function("ListarProveedorById")]
        [OpenApiOperation("ListarProveedorById", "Listar Proveedor por ID", Description = "Obtiene un proveedor por su ID.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Proveedor), Description = "Muestra el proveedor encontrado.")]
        [OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "ID del proveedor a buscar.")]
        public async Task<HttpResponseData> ListarProveedorById([HttpTrigger(AuthorizationLevel.Function, "get", Route = "ListarProveedorById/{id}")] HttpRequestData req, string id)
        {
            HttpResponseData respuesta;
            try
            {
                var estudio = await repositorio.ObtenerById(id);

                if (estudio != null)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    await respuesta.WriteAsJsonAsync(estudio);
                    return respuesta;
                }
                else
                {
                    respuesta = req.CreateResponse(HttpStatusCode.NotFound);
                    return respuesta;
                }
            }
            catch (Exception)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }
        [Function("ListarProveedor")]
        [OpenApiOperation("Listarespec", "ListarProveedor", Description = "Sirve para listar todas los proveedores")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
                                 bodyType: typeof(List<Proveedor>), Description = "Mostrara una lista de proveedores")]
        public async Task<HttpResponseData> ListarProveedor([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var lista = repositorio.ObtenerTodo();
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(lista.Result);
                return respuesta;

            }
            catch (Exception)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }
    }
}
