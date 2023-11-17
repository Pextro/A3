using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenCareApp.entities
{
    public class Item
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string nome { get; set; }
        public int quantidade { get; set; }
        public int idPessoa { get; set; }
    }
}
