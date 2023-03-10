using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope3Controller : MonoBehaviour
{
    public GameObject cube, rope, pole; // Para asignar Skycam, cuerda y poste desde el inspector
    private Vector3 rightTopVertex, ropeEnd, poleTop;
    private float ropeLength;
    private LineRenderer lineRenderer;
void Start()
    {
        lineRenderer = rope.GetComponent<LineRenderer>();
    }

    void Update()
    {
        // Update the rope's end position to match the left top vertex of the cube
        rightTopVertex = cube.transform.TransformPoint(new Vector3(0.25f, 0.25f, 0.25f));
        
        Bounds bounds = pole.GetComponent<Renderer>().bounds;
        poleTop = bounds.max;
        Vector3 ropePole = new Vector3 (x: 9.8f, y: 6f, z: 4.84f);
        lineRenderer.SetPosition(0, rightTopVertex);
        lineRenderer.SetPosition(1, ropePole);
        
        // Update the rope length as the cube moves
        ropeLength = Vector3.Distance(rightTopVertex, rope.transform.position);
    }
}
