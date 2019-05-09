using System;
using System.Collections.Generic;

[Serializable]
public class Level
{
    public Level()
    {
        
    }

    public int Number { get; set; }

    public string Description { get; set; }

    public ShapeType Type { get; set; }

    public List<Line> Lines { get; set; }

    public List<SerializableVector3> Vertices { get; set; }

    public List<SerializableVector3> Answer { get; set; }

    public List<Letter> Letters { get; set; }
}

[Serializable]
public enum ShapeType
{
    Cube,
    Tetrahedron
}