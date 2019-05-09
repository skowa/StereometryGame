using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

public class XmlDataLoader : IDataLoader
{
    private readonly string _xmlText;

    public XmlDataLoader(string xmlText)
    {
        _xmlText = xmlText;
    }

    public Level LoadLevel(int number)
    {
        XDocument xDocument = XDocument.Parse(_xmlText);
        XElement levelNode = xDocument.Element("Levels").Elements("Level").First(e => int.Parse(e.Element("Number").Value) == number);
        
        var serializer = new XmlSerializer(typeof(Level));
        return (Level) serializer.Deserialize(levelNode.CreateReader());
    }
}
