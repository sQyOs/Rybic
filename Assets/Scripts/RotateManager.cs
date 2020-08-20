using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateManager : MonoBehaviour
{
    [SerializeField] private GameObject _Controller;
    private Transform _selectUnit;
    private int _degre = 90;
    public float rotateSped = 1;



    public void RotateCubeEdge(string plane, int directionForRotation)
    {
        Vector3 _plane = Vector3.zero;
        switch (plane)
        {
            case "X":
                _plane = new Vector3(0, 2, 2);
                break;
            case "Y":
                _plane = new Vector3(2, 0, 2);
                break;
            case "Z":
                _plane = new Vector3(2, 2, 0);
                break;
            default:
                break;
        }

        Vector3 _Axis = Vector3.up;
        switch (plane)
        {
            case "X":
                _Axis = Vector3.right;
                break;
            case "Y":
                _Axis = Vector3.up;
                break;
            case "Z":
                _Axis = Vector3.forward;
                break;
            default:
                break;
        }


        _selectUnit = _Controller.GetComponent<SelectionManager>().selection;
        Collider[] selectColliders = Physics.OverlapBox(_selectUnit.position, _plane);
        foreach (Collider collider in selectColliders)
        {
            StartCoroutine(_Controller.GetComponent<RotateCube>().RotateColliderAround(collider, Vector3.zero, _Axis, _degre * directionForRotation, rotateSped));
        }
    }
}
