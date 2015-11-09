using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsnoeiCrawlerPhone
{
    class Item
    {
        public Item(int Value, string Name)
        {
            this.Name = Name;
            this.Value = Value;
        }
        public int Value { get; set; }
        public string Name { get; set; }
        public List<Item> subItem { get; set; }

        public override String ToString()
        {
            return Name + "\t" + Value + "\n";
        }
        public static String ItemListtoString(List<Item> items)
        {
            string aux = "";
            foreach(Item item in items)
            {
                aux += item.ToString();
                if(item.subItem != null)
                {
                    aux += ItemListtoString(item.subItem);
                }
            }
            
            return aux;
        }
        
    }
}
