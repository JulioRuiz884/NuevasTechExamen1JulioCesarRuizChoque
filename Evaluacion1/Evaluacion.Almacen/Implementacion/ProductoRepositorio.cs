using Azure.Data.Tables;
using Evaluacion.Almacen.Contratos;
using Evaluacion.Almacen.Modelo;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluacion.Almacen.Implementacion
{
    public class ProductoRepositorio: IProductoRepositorio
    {
        private readonly string? cadenaConexion;
        private readonly string? tableName;
        private readonly IConfiguration configuration;

        public ProductoRepositorio(IConfiguration conf)
        {
            configuration = conf;
            cadenaConexion = configuration.GetSection("cadenaConexion").Value;
            tableName = "Producto";
        }

        public async Task<bool> Eliminar(string partitiokey, string rowkey, string etag)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tableName);
                await tablaCliente.DeleteEntityAsync(partitiokey, rowkey);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Insertar(Producto producto)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tableName);
                await tablaCliente.UpsertEntityAsync(producto);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Modificar(Producto producto)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tableName);
                await tablaCliente.UpdateEntityAsync(producto, producto.ETag);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Producto> ObtenerById(string id)
        {
            var tablaCliente = new TableClient(cadenaConexion, tableName);
            var filtro = $"PartitionKey eq 'Productos' and RowKey eq '{id}'";
            await foreach (Producto producto in tablaCliente.QueryAsync<Producto>(filter: filtro))
            {
                return producto;
            }
            return null;
        }

        public async Task<List<Producto>> ObtenerTodo()
        {
            List<Producto> lista = new List<Producto>();
            var tablaCliente = new TableClient(cadenaConexion, tableName);
            var filtro = "$PartitionKey eq 'Productos'";
            // Realizar la consulta y esperar el resultado
            await foreach (var producto in tablaCliente.QueryAsync<Producto>(filter: filtro))
            {
                lista.Add(producto);
            }
            return lista;
        }
    }
}
