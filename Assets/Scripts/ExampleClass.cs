using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Input.mouseScrollDelta example
//
// Create a sphere moved by a mouse scrollwheel or two-finger
// slide on a Mac trackpad.

public class ExampleClass : MonoBehaviour
{
    [SerializeField] private GameObject CubeUnit;
    private Transform sphere;
    private float scale;
    public Vector3 _startpos = new Vector3(0, 0, 0);

    void Awake()
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere = go.transform;

        // create a yellow quad
        //go = GameObject.CreatePrimitive(PrimitiveType.Quad);
        //go.transform.Rotate(new Vector3(90.0f, 0.0f, 0.0f));
        //go.transform.localScale = new Vector3(4.0f, 4.0f, 4.0f);
        //go.GetComponent<Renderer>().material.color = new Color(0.75f, 0.75f, 0.0f, 0.5f);

        //create cub

        for (int x = -1; x <= 1; x++)
        {
            for (int y = 1; y >= -1; y--)
            {
                for (int z = -1; z <= 1; z++)
                {
                    //go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    go = Instantiate(CubeUnit) as GameObject;
                    go.transform.position = new Vector3(x, y, z) /*+ _startpos*/;
                    go.transform.localScale = new Vector3(45f, 45f, 45f);
                    go.name = $"Cube {x} {y} {z}";
                    go.tag = ("Cube");
                }  
            }
        }

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray mouseClick = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit
        //}

        // change the camera color and position
        Camera.main.clearFlags = CameraClearFlags.SolidColor;
        Camera.main.transform.position = new Vector3(2, 1, 10);
        Camera.main.transform.Rotate(0, -160, 0);

        scale = 0.1f;
    }

    void OnGUI()
    {
        Vector3 pos = sphere.position;
        pos.y += Input.mouseScrollDelta.y * scale;
        sphere.position = pos;
    }
}