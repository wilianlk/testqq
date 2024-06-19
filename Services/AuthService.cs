using Exportacion.Models;
using SQLite;
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
                await AddUser("admin", "admin123");
            }
        }
        public async Task<bool> Login(string username, string password)
        {
            var user = await _db.Table<UsuarioModel>()
                                .Where(u => u.Username == username)
                                .FirstOrDefaultAsync();
            return user != null && BCrypt.Net.BCrypt.Verify(password, user.Password);
        }
        public async Task AddUser(string username, string password)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
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
