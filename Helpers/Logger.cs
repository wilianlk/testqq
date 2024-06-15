using System;
using System.IO;
using System.Text;

namespace Exportacion.Helpers
{
    public static class Logger
    {
        private static readonly string LogFilePath = Path.Combine("C:\\RECAMIER", "exportacion_log.txt");
        private static readonly string ErrorLogFilePath = Path.Combine("C:\\RECAMIER", "exportacion_error_log.txt");

        public static void Log(string message)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(LogFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now}: {message}");
                }
            }
            catch (Exception ex)
            {
                HandleLoggingError(ex);
            }
        }

        public static void LogError(Exception ex)
        {
            var errorDetails = new StringBuilder();
            errorDetails.AppendLine($"Error: {ex.Message}");
            errorDetails.AppendLine($"StackTrace: {ex.StackTrace}");

            if (ex.InnerException != null)
            {
                errorDetails.AppendLine("Inner Exception:");
                errorDetails.AppendLine($"Inner Error: {ex.InnerException.Message}");
                errorDetails.AppendLine($"Inner StackTrace: {ex.InnerException.StackTrace}");
            }

            Log(errorDetails.ToString());
        }

        private static void HandleLoggingError(Exception ex)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(ErrorLogFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now}: Error al escribir en el log principal: {ex.Message}");
                    writer.WriteLine($"StackTrace: {ex.StackTrace}");
                }
            }
            catch (Exception innerEx)
            {
                try
                {
                    string criticalErrorLogFilePath = Path.Combine("C:\\RECAMIER", "critical_error_log.txt");
                    using (StreamWriter writer = new StreamWriter(criticalErrorLogFilePath, true))
                    {
                        writer.WriteLine($"{DateTime.Now}: Error crítico al intentar escribir en el archivo de log de errores: {innerEx.Message}");
                        writer.WriteLine($"StackTrace: {innerEx.StackTrace}");
                    }
                }
                catch
                {
                    // Si todo falla, no hay mucho más que podamos hacer
                }
            }
        }
    }
}
