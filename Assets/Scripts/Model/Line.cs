using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line 
{
    public Line(Vector3 startPoint, Vector3 endPoint)
    {
        StartPoint = startPoint;
        EndPoint = endPoint;
    }

    public Vector3 StartPoint { get; set; }

    public Vector3 EndPoint { get; set; }
}
