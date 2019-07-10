using System;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using jsonCreate;
using java.lang;

namespace jsonCreate
{
    public class Machine
    {
        public int PartCycleTime;
        public string Command;
        public int instanceID;
        public DateTime timestamp;
    }

}

namespace ConsoleApp3
{
    class Program
    {
        private static List<XElement> GetElements(XDocument doc, string elementName)
        {
            List<XElement> elements = new List<XElement>();

            foreach (XNode node in doc.DescendantNodes())
            {
                if (node is XElement)
                {
                    XElement element = (XElement)node;
                    if (element.Name.LocalName.Equals(elementName))
                        elements.Add(element);
                }
            }
            return elements;
        }

        static void Main(string[] args)
        {

            var filename = "Data.xml";

            var currentDirectory = Directory.GetCurrentDirectory();
            var CycleTimefilepath = Path.Combine(currentDirectory, filename);
            List<string> instanceList = new List<string>();
            List<string> startTimes = new List<string>();
            List<string> stopTimes = new List<string>();
            List<string> commandNames = new List<string>();
            List<TimeSpan> cycleTimes = new List<TimeSpan>();
            List<string> list = new List<string>();
            
          

            //Displays the xml file before conversion
            Console.WriteLine(CycleTimefilepath);

            XmlDocument doc = new XmlDocument();

            // Read with XmlDocument
            doc.Load(CycleTimefilepath);

            // Read with XDocument
            XDocument xdoc = XDocument.Load(CycleTimefilepath);
            List<XElement> elements = GetElements(xdoc, "Data");


            foreach (XElement element in elements)
            {
                instanceList.Add(element.Attribute("instanceId").Value);
                startTimes.Add(element.Attribute("timeStart").Value);
                stopTimes.Add(element.Attribute("timeStop").Value);
                commandNames.Add(element.Attribute("command").Value);
                
                DateTime startTime = DateTime.FromFileTimeUtc(long.Parse(element.Attribute("timeStart").Value));
                DateTime stopTime = DateTime.FromFileTimeUtc(long.Parse(element.Attribute("timeStop").Value));


                



                

                if (stopTime.Year < 2008)
                {

                    Console.WriteLine("Skipped one entry");
                }
                else
                {
                    
                    TimeSpan ts = stopTime.Subtract(startTime);

                    Machine cyc = new Machine();
                    cyc.PartCycleTime = (int)ts.TotalSeconds;
                    //cyc.instanceID = Integer.parseInt(element.Attribute("instanceId").Value);
                    cyc.Command = element.Attribute("command").Value;
                    long ticks = Long.parseLong(element.Attribute("timeStart").Value);
                    

                    //For Displaying the respective Semantic Data of the Instance ID and Command

                    

                    //DateTime time = new DateTime(ticks);
                    //Console.WriteLine(time.ToString("yyyy dd MM hh:mm:ss"));

                    //time = new DateTime(time.Ticks - (time.Ticks % TimeSpan.TicksPerSecond),time.Kind);

                    cyc.timestamp = startTime;

                    Console.WriteLine(cyc.timestamp);

                    string JSONresult = JsonConvert.SerializeObject(cyc, Newtonsoft.Json.Formatting.Indented);
                    Console.WriteLine(JSONresult);
                    Console.ReadLine();

                    list.Add(JSONresult);
                    cycleTimes.Add(ts);

                }

            }
  
            //Console.WriteLine(System.String.Join(", ", list).ToArray()) ;
            //string[] aray = list.ToArray();
            string combined_list = string.Join(",", list.ToArray());
            string str = System.String.Concat("[" + combined_list + "]");
            Console.WriteLine(str);
            //Console.WriteLine(System.String.Join(",", testarray));
            Console.ReadLine();
                  

            //Write stream to the file
            TextWriter tw = new StreamWriter("SavedJSON.json");

            //write lines of text to the file
            tw.WriteLine(str);

            //close the stream     
            tw.Close();


            ConsoleKeyInfo result = Console.ReadKey();
            Console.WriteLine("Press C to close");
            while (result.KeyChar != 'C' && result.KeyChar != 'c')
            {
                result = Console.ReadKey();
            }

            Console.WriteLine("\nYou have closed the program. Press any key to close the console window!");
            Console.ReadKey();



        }
    }
}

