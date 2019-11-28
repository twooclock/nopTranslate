using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Web;

namespace nopTranslate
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Arguments: -split | -join");
                Console.WriteLine(" 1. -split takes language_pack.xml and splits it into en.txt files adds an ID attribute to each Value node (needed later) and saves language_pack_out.xml");
                Console.WriteLine("");
                Console.WriteLine(" 2. You should translate en.txt files using Google translate document, copy translations and combine them into translations.xml. Add Language tags (first and last line). Correct all XML errors (try loading xml to your browser!)");
                Console.WriteLine("");
                Console.WriteLine(" 3. -join takes translations.xml and language_pack_out.xml, combines them and saves as language_pack_translated.xml. Check Language Name attribute and import into nopCommerce.");
            }
            else
            {
                if (args[0].ToLower() == "split" || args[0].ToLower() == "-split") { SplitXML(); }
                if (args[0].ToLower() == "join" || args[0].ToLower() == "-join") { JoinXML(); }
            }
            //1st step:            SplitXML();
            //2nd step translate txt files and join then into one xml (load into browser and correct any errors)
            //3rd step:            JoinXML();
        }



        public static void JoinXML()
        {
            int i = 1;

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode myvalueNode;
            if (File.Exists(System.AppContext.BaseDirectory + @"\translations.xml") == false)
            { Console.WriteLine("translations.xml is missing!"); }
            else
            {
                xmlDoc.Load(System.AppContext.BaseDirectory + @"\translations.xml");
                Dictionary<int, string> dict = new Dictionary<int, string>();

                foreach (XmlNode xmlNode in xmlDoc.DocumentElement.ChildNodes)
                {
                    dict.Add(int.Parse(xmlNode.Attributes["ID"].Value), xmlNode.InnerXml);
                    i++;
                }

                if (File.Exists(System.AppContext.BaseDirectory + @"\language_pack_out.xml") == false)
                { Console.WriteLine("language_pack_out.xml is missing!"); }
                else
                {
                    xmlDoc.Load(System.AppContext.BaseDirectory + @"\language_pack_out.xml");

                    foreach (KeyValuePair<int, string> entry in dict)
                    {
                        myvalueNode = xmlDoc.SelectSingleNode("//Value[@ID='" + entry.Key.ToString() + "']");
                        myvalueNode.InnerXml = entry.Value.Trim();
                        myvalueNode.Attributes.RemoveAll();
                    }

                    xmlDoc.Save(System.AppContext.BaseDirectory + @"\language_pack_translated.xml");

                    Console.WriteLine("Finished!");
                }
            }
        }

        public static void SplitXML()
        {
            int i = 1;
            int prva = 0;
            var vrstice = "";

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode myvalueNode;
            XmlAttribute myIDAttr;
            if (File.Exists(System.AppContext.BaseDirectory + @"\language_pack.xml") == false)
            { Console.WriteLine("language_pack.xml is missing!"); }
            else
            {
                xmlDoc.Load(System.AppContext.BaseDirectory + @"\language_pack.xml");
                foreach (XmlNode xmlNode in xmlDoc.DocumentElement.ChildNodes)
                {

                    myvalueNode = xmlNode.ChildNodes[0];
                    myIDAttr = xmlDoc.CreateAttribute("ID");
                    myIDAttr.Value = i.ToString();
                    myvalueNode.Attributes.Append(myIDAttr);
                    i++;

                    var line = myvalueNode.OuterXml.Replace("<Value", "<V").Replace("/Value>", "/V>");

                    // Process line
                    vrstice += line + Environment.NewLine;
                    if (vrstice.Length > 30000)
                    {
                        //save to file
                        File.WriteAllText(System.AppContext.BaseDirectory + @"\en" + prva.ToString() + ".txt", vrstice);
                        prva++;
                        vrstice = "";
                    }
                }
                //save reminder of lines
                File.WriteAllText(System.AppContext.BaseDirectory + @"\en" + prva.ToString() + ".txt", vrstice);
                xmlDoc.Save(System.AppContext.BaseDirectory + @"\language_pack_out.xml");
                Console.WriteLine("Finished!");
            }
        }
    }


}
