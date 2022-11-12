using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllinOrder_Shahaf_Ofir_Snir.Shared.Entities
{
   public class Question
    {
        public int ID { get; set; }
        public string FirstText { get; set; }
        public string LastText { get; set; }
        public string QuestionPic { get; set; }
        public string QuestionText { get; set; }
        public Game QuestionGame { get; set; }
        public List<Item> QuestionItems { get; set; }
        public int GameID { get; set; }
       
        public void clearQ()
        {
            ID = 0;
            FirstText = null;
            LastText = null;
            QuestionPic = null;
            QuestionText = null;
            QuestionItems = new List<Item>();
            AddemptyItems();
        }

        //הוספת פריטים ריקים
        public void AddemptyItems()
        {
            if (QuestionItems == null)
            {
                QuestionItems = new List<Item>();
            }
            for (int i = QuestionItems.Count; i < 3; i++)
            {
                Item NewItem = new Item();
                NewItem.ItemPlace = i + 1;
                QuestionItems.Add(NewItem);
            }

        }

    }

}
