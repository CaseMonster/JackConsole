using System.Xml;
using System.Collections;

namespace JackConsole
{
    class XML
    {
        static string XMLFILE = "whitelist.xml";

        public static ArrayList ReadXML()
        {
            ArrayList list = new ArrayList();
            XmlTextReader reader = new XmlTextReader(XMLFILE);
            while (reader.Read())
            {
                list.Add(reader.Value);
            }
            return list;
        }
    }
}
