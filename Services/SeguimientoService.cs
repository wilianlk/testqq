using Exportacion.Models;
using Microsoft.Maui.ApplicationModel.Communication;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exportacion.Services
{
    public class SeguimientoService : ISeguimientoService
    {
        private SQLiteAsyncConnection _dbConnection;

        public SeguimientoService()
        {
            SetUpDb();
        }

        private async void SetUpDb()
        {
            if (_dbConnection == null)
            {
                //string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Exportacion.db3");

                string baseDir = AppContext.BaseDirectory;
                string projectDir = Directory.GetParent(baseDir).Parent.Parent.Parent.Parent.Parent.FullName;
                string dbFolderPath = Path.Combine(projectDir, "DB");
                string dbPath = Path.Combine(dbFolderPath, "Exportacion.db3");

                if (!Directory.Exists(dbFolderPath))
                {
                    Directory.CreateDirectory(dbFolderPath);
                }

                _dbConnection = new SQLiteAsyncConnection(dbPath);
                await _dbConnection.CreateTableAsync<SeguimientoModel>();
            }
        }

        public Task<int> AddSeguimiento(SeguimientoModel seguimientoModel)
        {
           return _dbConnection.InsertAsync(seguimientoModel);
        }

        public Task<int> DeleteSeguimiento(SeguimientoModel seguimientoModel)
        {
            return _dbConnection.DeleteAsync(seguimientoModel);
        }

        public async Task<List<SeguimientoModel>> GetSeguimientoList()
        {
            var seguimientoList = await _dbConnection.Table<SeguimientoModel>().ToListAsync();
            return seguimientoList;
        }

        public Task<int> UpdateSeguimiento(SeguimientoModel seguimientoModel)
        {
            return _dbConnection.UpdateAsync(seguimientoModel);
        }

        Task<List<UsuarioModel>> ISeguimientoService.GetUsuarioList()
        {
            var usuariolist = _dbConnection.Table<UsuarioModel>().ToListAsync();
            return usuariolist;
        }

        Task<UsuarioModel> ISeguimientoService.GetObtenerUsuario(string email, string contrasena)
        {
            var obtenerusuario = _dbConnection.Table<UsuarioModel>()
                .Where(u => u.Email == email && u.Contrasena == contrasena)
                .FirstOrDefaultAsync();
            return obtenerusuario;
        }

        public Task<UsuarioModel> GetObtenerUsuario(Guid id)
        {
            var obtenerusuario = _dbConnection.Table<UsuarioModel>()
               .Where(u => u.Id == id)
               .FirstOrDefaultAsync();

            return obtenerusuario;
        }

        async Task<int> ISeguimientoService.AddUsuario(UsuarioModel usuarioModel)
        {
            var usuarioIsguardar = await GetObtenerUsuario(usuarioModel.Id);

            if(usuarioIsguardar == null)
            {
                return await _dbConnection.InsertAsync(usuarioModel);
            }
            else
            {
                return await _dbConnection.UpdateAsync(usuarioModel);
            }
        }

        async Task<int> ISeguimientoService.DeleteUsuario(Guid id)
        {
            return await _dbConnection.DeleteAsync(id);
        }
    }
}
