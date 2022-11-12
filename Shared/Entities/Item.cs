using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllinOrder_Shahaf_Ofir_Snir.Shared.Entities
{
    public class Item
    {

        public int ID { get; set; }
        public bool IsPic { get; set; }
        public string ItemContent  { get; set; }
        public int ItemPlace { get; set; }
        public Question ItemQuestion { get; set; }
        public int QuestionID { get; set; }
    }
}
