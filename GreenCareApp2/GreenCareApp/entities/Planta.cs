using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenCareApp.entities {
    public class Planta
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string nome { get; set; }
        public string especie { get; set; }
        public string localizacao {  get; set; }
        public int idPessoa { get; set; }
    }
}
