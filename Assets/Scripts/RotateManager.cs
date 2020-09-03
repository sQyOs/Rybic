using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateManager : MonoBehaviour
{
    private Transform _selectedUnit;
    private int _degre = 90;
    [Range(0.1f,1.0f)]
    public float rotateSped = 1;
    private float targetCenter;

    public void RotateCubeEdge(string plane, float directionForRotation)
    {
        GetComponent<ControlManager>().isRotate = true;
        targetCenter = GetComponent<GenerateCubeEdge>().dimension / 2f + 0.5f;
        Vector3 _plane = Vector3.zero;
        switch (plane)
        {
            case "X":
                _plane = new Vector3(0, targetCenter, targetCenter);
                break;
            case "Y":
                _plane = new Vector3(targetCenter, 0, targetCenter);
                break;
            case "Z":
                _plane = new Vector3(targetCenter, targetCenter, 0);
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
                _Axis = Vector3.back;
                break;
            default:
                break;
        }


        _selectedUnit = GetComponent<SelectionManager>().selection;

        Vector3 centerForRotation = Vector3.zero;
        Vector3 scaleForRotation = Vector3.zero;
        if (plane == "X")
        {
            centerForRotation = new Vector3(_selectedUnit.position.x, targetCenter, targetCenter);
            scaleForRotation = new Vector3(1, (targetCenter - 0.5f) * 2, (targetCenter - 0.5f) * 2);
        }
        if (plane == "Y")
        {
            centerForRotation = new Vector3(targetCenter, _selectedUnit.position.y, targetCenter);
            scaleForRotation = new Vector3((targetCenter - 0.5f) * 2, 1, (targetCenter - 0.5f) * 2);
        }
        if (plane == "Z")
        {
            centerForRotation = new Vector3(targetCenter, targetCenter, _selectedUnit.position.z);
            scaleForRotation = new Vector3((targetCenter - 0.5f) * 2, (targetCenter - 0.5f) * 2, 1);
        }

        Collider[] selectColliders = Physics.OverlapBox(centerForRotation, scaleForRotation / 2);
        foreach (Collider collider in selectColliders)
        {
            StartCoroutine(GetComponent<RotateCube>().RotateColliderAround(collider, Vector3.one * targetCenter, _Axis, _degre * directionForRotation, rotateSped));
        }
        
    }
}
