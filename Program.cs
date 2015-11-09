using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Collections;
using System.Threading;
using System.Xml;

namespace EsnoeiCrawlerPhone
{
    class Program
    {
        static void Main(string[] args)
        {
            Example();
        }
        
        static void generaFiles(List<Item> items)
        {

            StreamWriter file = new StreamWriter("output.txt");
            foreach (Item i in items)
            {

                foreach (Item j in i.subItem)
                {
                    foreach (Item k in j.subItem)
                    {
                        foreach (Item l in k.subItem)
                        {
                            file.WriteLine(i.Name + "\t" + j.Name + "\t" + k.Name + "\t" + l.Name);
                        }
                    }
                }
            }
            file.Close();
        }
        static void Example()
        {
            var siteweb = new HtmlWeb();
            var document = siteweb.Load("http://www.callcentric.com/did_lookup.php?type=check_by_location&prod_id=2&country_id=38");
            var html = document.DocumentNode;

            List<Item> countries = extractSelect(html, "country_id");

            foreach(Item country in countries)
            {
                document = siteweb.Load("http://www.callcentric.com/did_lookup.php?type=check_by_location&prod_id=2&country_id=" + country.Value);
                html = document.DocumentNode;

                country.subItem = (extractSelect(html, "state_id"));


                foreach (Item state in country.subItem)
                {
                    var documentaux = siteweb.Load("http://www.callcentric.com/did_lookup.php?type=check_by_location&prod_id=2&country_id="
                        + country.Value + "&state_id=" + state.Value);
                    var htmlstate = documentaux.DocumentNode;

                    state.subItem = extractSelect(htmlstate, "area_code");

                    foreach(Item area in state.subItem)
                    {
                        var documentArea = siteweb.Load("http://www.callcentric.com/did_lookup.php?type=check_by_location&prod_id=2&country_id="
                        + country.Value + "&state_id=" + state.Value
                        + "&area_code=" + area.Value );
                        var htmlArea = documentArea.DocumentNode;

                        area.subItem = extractSelect(htmlArea, "rate_center_id");

                    }
                }
            }

            generaFiles(countries);
        }


        private static List<Item> extractSelect(HtmlNode html, string nameSelect)
        {
            List<Item> country = new List<Item>();

            var criterioBusqueda = "//select[@name='"+ nameSelect +"']";

            foreach (HtmlNode item in html.SelectNodes(criterioBusqueda))
            {
                string[] valueText = item.InnerText.Split('\n');

                List<HtmlNode> values = item.Elements("option").ToList();
                

                for (int i = 0; i < values.Count; i++)
                {
                    var aux = values[i].GetAttributeValue("value", "");
                    if (!aux.Equals(""))
                    {
                        int parse = int.Parse(aux);

                        //tener en cuenta que en el caso de la pagina ejemplo hay un elemento vacio al inicio.
                        //puede que no pase asi con todas.
                        string value = valueText[i + 1];

                        country.Add(new Item(parse, value));
                    }
                }
            }
            return country;

        }




        /////////////////////////////////////////////////
        // Lo que esta a continuacion es porque etoy intentando recorrerlo sin interaccion con el usuario...
        /////////////////////////////////////////////////


        //private static List<Item> extractSelectMultiple(HtmlNode html, List<string> selectMultiple)
        //{
        //    List<Item> items = new List<Item>();
        //    foreach (string select in selectMultiple)
        //    {
        //        var criterioBusqueda = "//select[@name='" + select + "']";
        //        foreach (HtmlNode item in html.SelectNodes(criterioBusqueda))
        //        {
        //            string[] valueText = item.InnerText.Split('\n');
        //            List<HtmlNode> values = item.Elements("option").ToList();
        //            for (int i = 0; i < values.Count; i++)
        //            {
        //                var aux = values[i].GetAttributeValue("value", "");
        //                if (!aux.Equals(""))
        //                {
        //                    int parse = int.Parse(aux);
        //                    string value = valueText[i + 1];
        //                    Item newItem = new Item(parse, value);


        //                    newItem.subItem = extractSelectMultiple(getHTMLUrlSelect(select), copyRemoveFirst(selectMultiple));

        //                    items.Add(newItem);

        //                }
        //            }
        //        }
        //    }
        //}

        //private static HtmlNode getHTMLUrlSelect(string select)
        //{
        //    Hashtable a = new Hashtable();
        //    a.Add("country_id", "http://www.callcentric.com/did_lookup.php?type=check_by_location&prod_id=4&");
        //    a.Add(, "http://www.callcentric.com/did_lookup.php?type=check_by_location&prod_id=4&country_id=38");

        //    var siteweb = new HtmlWeb();
        //    var document = siteweb.Load("http://www.callcentric.com/did_lookup.php?type=check_by_location&prod_id=4&country_id=38");
        //    var html = document.DocumentNode;
        //}

        //private static List<string> copyRemoveFirst(List<string> selectMultiple)
        //{
        //    List<string> aux = new List<string>();
        //    if (selectMultiple == null)
        //        return new List<string>();

        //    if (selectMultiple.Count == 1)
        //        return new List<string>();

        //    for (int i = 1; i < selectMultiple.Count; i++)
        //    {
        //        aux.Add(selectMultiple[i]);
        //    }
        //    return aux;
        //}

    }

}
