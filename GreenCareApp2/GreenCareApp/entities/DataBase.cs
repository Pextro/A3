using GreenCareApp.entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenCareApp.entities {
    public class DataBase
    {
        readonly SQLiteAsyncConnection _database;

        public DataBase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Planta>().Wait();
            _database.CreateTableAsync<Pessoa>().Wait();
            _database.CreateTableAsync<Item>().Wait();
        }

        public Task<List<Planta>> GetPlantsAsync()
        {
            return _database.Table<Planta>().ToListAsync();
        }

        public Task<int> SavePlantaAsync(Planta Planta)
        {
            return _database.InsertAsync(Planta);
        }
        public Task<List<Pessoa>> GetPessoaAsync()
        {
            return _database.Table<Pessoa>().ToListAsync();
        }

        public Task<int> SavePessoaAsync(Pessoa Pessoa)
        {
            return _database.InsertAsync(Pessoa);
        }

        public Task<List<Item>> GetItemAsync() {
            return _database.Table<Item>().ToListAsync();
        }

        public Task<int> SaveItemAsync(Item Item) {
            return _database.InsertAsync(Item);
        }
    }
}
