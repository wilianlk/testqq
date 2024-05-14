using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exportacion.Models
{
    public class SeguimientoModel
    {
        [PrimaryKey,AutoIncrement]
        public int Id_seguimiento { get; set; }
        public string Nro_exportacion { get; set; }
        public string Fecha { get; set; }
        public string Cliente_Filial { get; set; }
        public string Hora_Llegada { get; set; }
    }
}
