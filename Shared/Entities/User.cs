using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllinOrder_Shahaf_Ofir_Snir.Shared.Entities
{
    public class User
    {

        public int ID { get; set; }
        public string UserEmail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Game> UserGames { get; set; }
    }
}
