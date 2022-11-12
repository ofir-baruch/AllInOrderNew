using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllinOrder_Shahaf_Ofir_Snir.Shared.Entities
{
    public class Game
    {
        public int ID { get; set; }
        public int GameCode { get; set; }
        public bool IsPublish { get; set; }
        public DateTime ModDate { get; set; }
        public string GameName { get; set; }
        public int QuestionTime { get; set; }
        public List<Question> GameQuestions { get; set; }
        public User GameUser { get; set; }
        public int UserID { get; set; }
        
    }

}

   
    

