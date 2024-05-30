using Exportacion.Models;
using Exportacion.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exportacion.Services
{
    public class SQLiteData
    {
        readonly SQLiteAsyncConnection _connectionBD;

        public UsuarioData UsuarioDataTable { get; set; }

        public SQLiteData(string path)
        {
            _connectionBD = new SQLiteAsyncConnection(path);

            _connectionBD.CreateTableAsync<UsuarioModel>().Wait();

            UsuarioDataTable = new UsuarioData(_connectionBD);
        }
    }
}
