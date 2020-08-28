using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCubeEdge : MonoBehaviour
{
    [SerializeField] private GameObject CubeEdge;
    public int dimension = 3;


    void Awake()
    {
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
                        go.name = $"Edge {x} {y} {z} {go.transform.rotation.eulerAngles}";
                    }
                    for (int i = 90; i < 360; i += 180)
                    {
                        go = Instantiate(CubeEdge, new Vector3(x, y, z), Quaternion.AngleAxis(i, Vector3.forward));
                        go.name = $"Edge {x} {y} {z} {go.transform.rotation.eulerAngles}";
                    }
                }
            }
        }
        float center = dimension / 2f + 0.5f;
        Collider[] deletedObj = Physics.OverlapBox(Vector3.one * center, Vector3.one * center - Vector3.one);
        foreach (Collider item in deletedObj)
        {
            if (item.tag == "Edge")
            {
                Destroy(item.gameObject);
            }
        }
    }
}
