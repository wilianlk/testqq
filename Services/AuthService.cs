using Exportacion.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exportacion.Services
{
    public class AuthService
    {
        private SQLiteAsyncConnection _db;

        public AuthService(string dbPath)
        {
            _db = new SQLiteAsyncConnection(dbPath);
            Initialize();
        }
        private async void Initialize()
        {
            await SetUpDb();
        }
        private async Task SetUpDb()
        {
            await _db.CreateTableAsync<UsuarioModel>();

            var adminExists = await _db.Table<UsuarioModel>().Where(u => u.Username == "admin").CountAsync() > 0;
            if (!adminExists)
            {
                await AddUser("admin", HashPassword("admin123"));
            }
        }
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        public async Task<bool> Login(string username, string password)
        {
            var user = await _db.Table<UsuarioModel>()
                                .Where(u => u.Username == username)
                                .FirstOrDefaultAsync();

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return true;
            }

            return false;
        }
        public async Task AddUser(string username, string password)
        {
            var hashedPassword = HashPassword(password);
            var newUser = new UsuarioModel { Username = username, Password = hashedPassword };
            await _db.InsertAsync(newUser);
        }
        public async Task<List<UsuarioModel>> GetAllUsers()
        {
            return await _db.Table<UsuarioModel>().ToListAsync();
        }
        public async Task UpdateUser(UsuarioModel user)
        {
            await _db.UpdateAsync(user);
        }
        public async Task DeleteUser(int userId)
        {
            var user = await _db.FindAsync<UsuarioModel>(userId);
            if (user != null)
            {
                await _db.DeleteAsync(user);
            }
        }
    }
}
