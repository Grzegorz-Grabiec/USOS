using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace USOS.Models
{
    public class NewsView
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

   
        public NewsView()
        {

        }
        public NewsView(News V)
        {
            ID = V.ID;
            Text = V.Text;
            Date = V.Date;
        }
    }
}
