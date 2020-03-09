using System;

[Serializable]
public class Line 
{
    public Line()
    {
        
    }

    public Line(SerializableVector3 startPoint, SerializableVector3 endPoint)
    {
        StartPoint = startPoint;
        EndPoint = endPoint;
    }

    public SerializableVector3 StartPoint { get; set; }

    public SerializableVector3 EndPoint { get; set; }
}
