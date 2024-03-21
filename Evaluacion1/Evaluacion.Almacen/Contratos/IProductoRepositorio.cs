using Evaluacion.Almacen.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluacion.Almacen.Contratos
{
    public interface IProductoRepositorio
    {
        public Task<bool> Insertar(Producto producto);
        public Task<bool> Modificar(Producto producto);
        public Task<bool> Eliminar(string partitiokey, string rowkey, string etag);
        public Task<List<Producto>> ObtenerTodo();
        public Task<Producto> ObtenerById(string id);
    }
}
