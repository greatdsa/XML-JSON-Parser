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
        public string Unitname;
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
             //var filename = "2018_06_08_13_18_41_Adhoc.xml";
             //var filename = "2018_06_08_13_23_47_Adhoc.xml";
            //var filename = "2018_06_08_13_27_00_Adhoc.xml";
            //var filename = "2018_06_08_13_30_23_Adhoc.xml";
            //var filename = "2018_06_08_13_32_47_Adhoc.xml";
            //var filename = "2018_06_08_13_44_25_Adhoc.xml";
            var filename = "2018_06_08_13_45_33_Adhoc.xml";

            var currentDirectory = Directory.GetCurrentDirectory();
            var CycleTimefilepath = Path.Combine(currentDirectory, filename);
            List<string> instanceList = new List<string>();
            List<string> startTimes = new List<string>();
            List<string> stopTimes = new List<string>();
            List<string> commandNames = new List<string>();
            List<TimeSpan> cycleTimes = new List<TimeSpan>();
            List<string> list = new List<string>();
            // List<DateTime> Timestamp = new List<DateTime>();
            List<string> unitname = new List<string>();

            //Displays the xml file before conversion
            Console.WriteLine(CycleTimefilepath);

            XmlDocument doc = new XmlDocument();

            // Read with XmlDocument
            doc.Load(CycleTimefilepath);

            // Read with XDocument
            XDocument xdoc = XDocument.Load(CycleTimefilepath);
            List<XElement> elements = GetElements(xdoc, "Data");


            //xdoc.Descendants("Data").Where(x => (double)x.Attribute("timeStop") == 0).Remove();

            

            foreach (XElement element in elements)
            {
                instanceList.Add(element.Attribute("instanceId").Value);
                startTimes.Add(element.Attribute("timeStart").Value);
                stopTimes.Add(element.Attribute("timeStop").Value);
                commandNames.Add(element.Attribute("command").Value);
                
                DateTime startTime = DateTime.FromFileTimeUtc(long.Parse(element.Attribute("timeStart").Value));
                DateTime stopTime = DateTime.FromFileTimeUtc(long.Parse(element.Attribute("timeStop").Value));


                



                //Removes the Data which has timestop == 0

                if (stopTime.Year < 2018)
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

                    switch (element.Attribute("instanceId").Value)
                    {
                        case "1":
                            Console.WriteLine("Case One");
                            cyc.Unitname = "Station";
                            cyc.Command = "One Part";
                            break;
                        case "2":
                            Console.WriteLine("Case Two");
                            cyc.Unitname = "Conveyer";
                            switch (element.Attribute("command").Value)
                            {
                                case "1":
                                    Console.WriteLine("Move to Position A");
                                    cyc.Command = ("MoveToPosA");
                                    break;
                                case "2":
                                    Console.WriteLine("Move to Position B");
                                    cyc.Command = ("MoveToPosB");
                                    break;
                                case "3":
                                    Console.WriteLine("Move to Position C");
                                    cyc.Command = ("MoveToPosC");
                                    break;
                                case "4":
                                    Console.WriteLine("Move to Position D");
                                    cyc.Command = ("MoveToPosD");
                                    break;
                            }
                            break;
                        case "3":
                            Console.WriteLine("Case Three");
                            cyc.Unitname ="InletTest";
                            switch (element.Attribute("command").Value)
                            {
                                case "1":
                                    Console.WriteLine("Move Base: move inlet test up");
                                    cyc.Command = ("MoveBase");
                                    break;
                                case "2":
                                    Console.WriteLine("Move Work");
                                    cyc.Command = ("MoveWork");
                                    break;
                            }
                            break;
                        case "4":
                            Console.WriteLine("Case Four");
                            cyc.Unitname = "Press";
                            switch (element.Attribute("command").Value)
                            {
                                case "1":
                                    Console.WriteLine("Pressing");
                                    cyc.Command = ("Pressing");
                                    break;
                                case "2":
                                    Console.WriteLine("Homing");
                                    cyc.Command = ("Homing");
                                    break;
                            }
                            break;
                        case "5":
                            Console.WriteLine("Case Five");
                            cyc.Unitname = "Feeder";
                            switch (element.Attribute("command").Value)
                            {
                                case "1":
                                    Console.WriteLine("Move Base: move feeder out");
                                    cyc.Command = ("MoveFeederOut");
                                    break;
                                case "2":
                                    Console.WriteLine("Move Work: move feeder in");
                                    cyc.Command = ("MoveFeederIn");
                                    break;
                            }
                            break;
                        case "6":
                            Console.WriteLine("Case Six");
                            cyc.Unitname = "PressCylinder";
                            switch (element.Attribute("command").Value)
                            {
                                case "1":
                                    Console.WriteLine("Move Base: move press cylinder up");
                                    cyc.Command = ("MovePressCylinderUp");
                                    break;
                                case "2":
                                    Console.WriteLine("Move Work: move press cylinder down");
                                    cyc.Command = ("MovePressCylinderDown");
                                    break;
                            }
                            break;
                        case "7":
                            Console.WriteLine("Case Seven");
                            cyc.Unitname = "Robot";
                            switch (element.Attribute("command").Value)
                            {
                                case "1":
                                    Console.WriteLine("Take Part");
                                    cyc.Command = ("TakePart");
                                    break;
                                case "2":
                                    Console.WriteLine("Gripper To Box");
                                    cyc.Command = ("GripperToBox");
                                    break;
                            }
                            break;
                        case "8":
                            Console.WriteLine("Case Eight");
                            cyc.Unitname = "Lift";
                            switch (element.Attribute("command").Value)
                            {
                                case "1":
                                    Console.WriteLine("Move Base: move lift down");
                                    cyc.Command = ("MoveLiftDown");
                                    break;
                                case "2":
                                    Console.WriteLine("Move Work: move lift up");
                                    cyc.Command = ("MoveLiftUp");
                                    break;
                            }
                            break;
                        case "9":
                            Console.WriteLine("Case Nine");
                            cyc.Unitname = "Rotor";
                            switch (element.Attribute("command").Value)
                            {
                                case "1":
                                    Console.WriteLine("Move Base: rotor to box");
                                    cyc.Command = ("MoveRotorToBox");
                                    break;
                                case "2":
                                    Console.WriteLine("Move Work: move rotor to conveyer");
                                    cyc.Command = ("MoveRotorToConveyer");
                                    break;
                            }
                            break;
                        case "10":
                            Console.WriteLine("Case Ten");
                            cyc.Unitname = "Gripper";
                            switch (element.Attribute("command").Value)
                            {
                                case "1":
                                    Console.WriteLine("Move Base: open gripper");
                                    cyc.Command = ("Open Gripper");
                                    break;
                                case "2":
                                    Console.WriteLine("Move Work: close gripper");
                                    cyc.Command = ("Close Gripper");
                                    break;
                            }
                            break;
                        case "16":
                            Console.WriteLine("Case Sixteen");
                            cyc.Unitname = "Reader";
                            switch (element.Attribute("command").Value)
                            {
                                case "1":
                                    Console.WriteLine("Read");
                                    cyc.Command = ("Read");
                                    break;
                                case "2":
                                    Console.WriteLine("send config");
                                    cyc.Command = ("Set");
                                    break;
                            }
                            break;
                        default:
                            Console.WriteLine("Error");
                            break;
                    }

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

            //Only prints the Data string with instance ID 16
            // XmlNodeList nodeList = doc.SelectNodes("/AtdRecordExport/StationVersion/Records/Record/Datas/Data[@instanceId='16']");

            //Prints the Data records
           /* XmlNodeList elemList = doc.SelectNodes("/AtdRecordExport/StationVersion/Records/Record/Datas/Data");
            foreach (XmlNode node in elemList)
            {
                var datac = node.OuterXml;

                //string command = node.GetAttribute("command");
                Console.WriteLine(datac);
              //  Console.WriteLine(datac.GetType());
                Console.ReadLine();
            }*/

        }
    }
}

