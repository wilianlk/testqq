using System;
using System.IO;

namespace Exportacion.Helpers
{
    public static class DatabaseHelper
    {
        public static string GetDatabasePath()
        {
            string dbFileName = "Exportacion.db3";
            string dbFolderPath = @"C:\Recamier\DBExportacion";

            try
            {
#if ANDROID
                dbFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
#else
                // Asegúrate de que la carpeta de la base de datos exista
                if (!Directory.Exists(dbFolderPath))
                {
                    Directory.CreateDirectory(dbFolderPath);
                    Logger.Log($"Created Database Folder Path: {dbFolderPath}");
                }
#endif

                string dbPath = Path.Combine(dbFolderPath, dbFileName);
                Logger.Log($"Database path determined: {dbPath}");

                // Asegúrate de que la base de datos exista
                if (!File.Exists(dbPath))
                {
                    Logger.Log("Database does not exist. Creating new database.");
                    File.Create(dbPath).Close();  // Crea el archivo de base de datos
                }

                return dbPath;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw new Exception("Failed to determine or create the database path. Please check the logs for more details.", ex);
            }
        }
    }
}
