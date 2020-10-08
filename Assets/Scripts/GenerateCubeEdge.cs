using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCubeEdge : MonoBehaviour
{
    [SerializeField] private GameObject CubeEdge;
    public int dimension = 3;
    private Vector3 center = Vector3.zero;
    [Range(1, 2)]
    public int generateMethod = 2;

    void Awake()
    {
        center = Vector3.one * (dimension / 2f + 0.5f);
        GameObject go;
        Dictionary<Color, Vector3> colorsDirections = new Dictionary<Color, Vector3>
        {
            {Color.white  , Vector3.down    },
            {Color.black  , Vector3.right   },
            {Color.green  , Vector3.left    },
            {Color.red    , Vector3.up      },
            {Color.blue   , Vector3.forward },
            {Color.yellow , Vector3.back    }
        };
        //create edge

        if (generateMethod == 1)
        {
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

            foreach (var item in colorsDirections)
            {
                paint(item.Key, item.Value);
            }
        }

        if (generateMethod == 2)
        {
            foreach (var item in colorsDirections)
            {
                Vector3 direction = item.Value;
                float x, y, z = 0;
                float sign = Mathf.Sign(direction.x + direction.y + direction.z);
                float constCorrect = center.x + sign * dimension / 2f - sign * 0.5f;
                Vector3 zOrientation = Vector3.zero;


                for (int first = 1; first <= dimension; first++)
                {
                    for (int second = 1; second <= dimension; second++)
                    {
                        if (direction.x != 0)
                        {
                            x = constCorrect;
                            y = first;
                            z = second;
                            zOrientation = new Vector3(0, 0, 1 * sign);
                        }
                        else if (direction.y != 0)
                        {
                            x = first;
                            y = constCorrect;
                            z = second;
                            zOrientation = new Vector3(1 * sign, 0, 0);
                        }
                        else
                        {
                            x = first;
                            y = second;
                            z = constCorrect;
                            zOrientation = new Vector3(0, 1 * sign, 0);
                        }
                        go = Instantiate(CubeEdge, new Vector3(x, y, z), Quaternion.LookRotation(zOrientation, direction));
                        go.name = $"Edge {x} {y} {z}";
                        go.GetComponent<Renderer>().material.color = item.Key;
                    }
                }
            }

            //go = Instantiate(CubeEdge, new Vector3(x, y, z), Quaternion.AngleAxis(i, Vector3.right));

            //go = Instantiate(CubeEdge, new Vector3(4, 4, 4), Quaternion.LookRotation(Vector3.up));
            //go.name = "up";
            //Debug.Log(Vector3.Cross(Vector3.up, Vector3.up));
            //go = Instantiate(CubeEdge, new Vector3(4, 4, 5), Quaternion.LookRotation(Vector3.back));
            //go.name = "back";
            //Debug.Log(Vector3.Cross(Vector3.up, Vector3.back));
            //go = Instantiate(CubeEdge, new Vector3(4, 4, 6), Quaternion.LookRotation(Vector3.forward));
            //go.name = "forward";
            //Debug.Log(Vector3.Cross(Vector3.up, Vector3.forward));

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
