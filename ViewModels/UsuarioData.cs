using Exportacion.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exportacion.ViewModels
{
    public class UsuarioData
    {
        private SQLiteAsyncConnection _connectionBD;
        public UsuarioData(SQLiteAsyncConnection connectionBD) 
        { 
            _connectionBD = connectionBD;
        }
        public Task<List<UsuarioModel>> ListaUsuarios() {
            var lista = _connectionBD
                .Table<UsuarioModel>()
                .ToListAsync();
            return lista;
        }
        public Task<UsuarioModel> ObtenerUsuario(string email,string contrasena)
        {
            var usuario = _connectionBD
                .Table<UsuarioModel>()
                .Where(x => x.Email == email && x.Contrasena == contrasena)
                .FirstOrDefaultAsync();
            return usuario;
        }
        public Task<UsuarioModel> ObtenerUsuario(Guid id)
        {
            var usuario = _connectionBD
                .Table<UsuarioModel>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            return usuario;
        }
        public async Task<int> GuardarUsuario(UsuarioModel usuario)
        {
            var usuarioisguardar = await ObtenerUsuario(usuario.Id);

            if (usuarioisguardar == null)
            {
                return await _connectionBD.InsertAsync(usuario);
            }
            else
            {
                return await _connectionBD.UpdateAsync(usuario);
            }
        }
        public async Task<int> DeleteUsuario(Guid id)
        {
            return await _connectionBD.DeleteAsync(id);
        }
    }
}
