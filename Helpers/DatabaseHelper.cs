using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exportacion.Helpers
{
    public static class DatabaseHelper
    {
        public static string GetDatabasePath()
        {
            string dbFileName = "Exportacion.db3";
            string dbFolderPath = string.Empty;

#if WINDOWS
    string baseDir = AppContext.BaseDirectory;
    string projectDir = Directory.GetParent(baseDir).Parent.Parent.Parent.Parent.Parent.FullName;
    dbFolderPath = Path.Combine(projectDir, "DB");
#elif ANDROID
    dbFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
#else
            throw new PlatformNotSupportedException("Platform not supported");
#endif

            return Path.Combine(dbFolderPath, dbFileName);
        }


    }
}
