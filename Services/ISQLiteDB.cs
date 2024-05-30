using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exportacion.Services
{
    internal interface ISQLiteDB
    {
        string SQLiteLocalPath(string bancoDados);
    }
}
