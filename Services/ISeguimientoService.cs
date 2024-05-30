using Exportacion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exportacion.Services
{
    public interface ISeguimientoService
    {
        public Task<List<UsuarioModel>> GetUsuarioList();
        public Task<UsuarioModel> GetObtenerUsuario(string email, string contrasena);
        public Task<List<SeguimientoModel>> GetSeguimientoList();
        Task<int> AddUsuario(UsuarioModel usuarioModel);
        public Task<int> DeleteUsuario(Guid id);
        Task<int> AddSeguimiento(SeguimientoModel seguimientoModel);
        Task<int> DeleteSeguimiento(SeguimientoModel seguimientoModel);
        Task<int> UpdateSeguimiento(SeguimientoModel seguimientoModel);
    }
}
