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
    public class ProveedorRepositorio: IProveedorRepositorio
    {
        private readonly string? cadenaConexion;
        private readonly string? tableName;
        private readonly IConfiguration configuration;

        public ProveedorRepositorio(IConfiguration conf)
        {
            configuration = conf;
            cadenaConexion = configuration.GetSection("cadenaConexion").Value;
            tableName = "Proveedor";
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

        public async Task<bool> Insertar(Proveedor proveedor)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tableName);
                await tablaCliente.UpsertEntityAsync(proveedor);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Modificar(Proveedor proveedor)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tableName);
                await tablaCliente.UpdateEntityAsync(proveedor, proveedor.ETag);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Proveedor> ObtenerById(string id)
        {
            var tablaCliente = new TableClient(cadenaConexion, tableName);
            var filtro = $"PartitionKey eq 'Proveedor' and RowKey eq '{id}'";
            await foreach (Proveedor proveedor in tablaCliente.QueryAsync<Proveedor>(filter: filtro))
            {
                return proveedor;
            }
            return null;
        }

        public async Task<List<Proveedor>> ObtenerTodo()
        {
            List<Proveedor> lista = new List<Proveedor>();
            var tablaCliente = new TableClient(cadenaConexion, tableName);
            var filtro = "$PartitionKey eq 'Proveedor'";
            // Realizar la consulta y esperar el resultado
            await foreach (var proveedor in tablaCliente.QueryAsync<Proveedor>(filter: filtro))
            {
                lista.Add(proveedor);
            }
            return lista;
        }
    }
}

