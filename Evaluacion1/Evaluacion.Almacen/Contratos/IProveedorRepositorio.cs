using Evaluacion.Almacen.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluacion.Almacen.Contratos
{
    public interface IProveedorRepositorio
    {
        public Task<bool> Insertar(Proveedor proveedor);
        public Task<bool> Modificar(Proveedor proveedor);
        public Task<bool> Eliminar(string partitiokey, string rowkey, string etag);
        public Task<List<Proveedor>> ObtenerTodo();
        public Task<Proveedor> ObtenerById(string id);
    }
}
