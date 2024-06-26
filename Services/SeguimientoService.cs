﻿using Exportacion.Models;
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

    }
}
