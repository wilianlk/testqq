using Exportacion.Platforms.Android;
using Exportacion.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(SQLiteBD))]
namespace Exportacion.Platforms.Android
{
    
    public class SQLiteBD : ISQLiteDB
    {
        public string SQLiteLocalPath(string bancoDados)
        {
            var path = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, bancoDados);
        }
    }
}
