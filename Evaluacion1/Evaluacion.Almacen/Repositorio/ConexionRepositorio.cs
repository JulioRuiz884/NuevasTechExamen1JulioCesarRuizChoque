using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluacion.Almacen.Repositorio
{
    public class ConexionRepositorio
    {
        public string cadenaConexion;
        private string tablaName;

        public ConexionRepositorio(string cadena, string tabla)
        {
            this.cadenaConexion = cadena;
            this.tablaName = tabla;
        }
    }
}
