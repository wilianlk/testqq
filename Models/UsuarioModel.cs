using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exportacion.Models
{
    public class UsuarioModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Contrasena { get; set; }

        public UsuarioModel() 
        {
            Id = Guid.NewGuid();
        }
    }
}
