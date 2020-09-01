using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCubeEdge : MonoBehaviour
{
    [SerializeField] private GameObject CubeEdge;
    public int dimension = 3;
    private Vector3 center = Vector3.zero;


    void Awake()
    {
        center = Vector3.one * (dimension / 2f + 0.5f);
        GameObject go;

        //create edge
        for (int x = 1; x <= dimension; x++)
        {
            for (int y = 1; y <= dimension; y++)
            {
                for (int z = 1; z <= dimension; z++)
                {

                    for (int i = 0; i < 360; i += 90)
                    {
                        go = Instantiate(CubeEdge, new Vector3(x, y, z), Quaternion.AngleAxis(i, Vector3.right));
                        go.name = $"Edge {x} {y} {z} {go.transform.rotation.eulerAngles.normalized}";
                    }
                    for (int i = 90; i < 360; i += 180)
                    {
                        go = Instantiate(CubeEdge, new Vector3(x, y, z), Quaternion.AngleAxis(i, Vector3.forward));
                        go.name = $"Edge {x} {y} {z} {go.transform.rotation.eulerAngles.normalized}";
                    }
                }
            }
        }
        Collider[] deletedObj = Physics.OverlapBox(center, center - Vector3.one);
        foreach (Collider item in deletedObj)
        {
            if (item.tag == "Edge")
            {
                Destroy(item.gameObject);
            }
        }

        Dictionary<Color, Vector3> colorsDirections = new Dictionary<Color, Vector3>
        {
            {Color.white  , Vector3.down    },
            {Color.black  , Vector3.right   },
            {Color.green  , Vector3.left    },
            {Color.red    , Vector3.up      },
            {Color.blue   , Vector3.forward },
            {Color.yellow , Vector3.back    }
        };

        foreach (var item in colorsDirections)
        {
            paint(item.Key, item.Value);
        }
    }

    private void paint(Color color, Vector3 direction)
    {
        Collider[] colliders = Physics.OverlapBox(center + direction * center.y - direction, center - Vector3.one);
        foreach (Collider item in colliders)
        {
            item.gameObject.GetComponent<Renderer>().material.color = color;
        }
    }
}
