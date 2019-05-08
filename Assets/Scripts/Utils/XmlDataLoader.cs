using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

public class XmlDataLoader : IDataLoader
{
    private readonly string _filePath;

    public XmlDataLoader(string filePath)
    {
        _filePath = filePath;
    }

    public Level LoadLevel(int number)
    {
        XDocument xDocument = XDocument.Load(_filePath);
        XElement levelNode = xDocument.Element("Levels").Elements("Level").First(e => int.Parse(e.Element("Number").Value) == number);
        
        var serializer = new XmlSerializer(typeof(Level));
        return (Level) serializer.Deserialize(levelNode.CreateReader());
    }
}
