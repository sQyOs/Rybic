using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateManager : MonoBehaviour
{
    [SerializeField] private GameObject _controller;
    private Transform _selectedUnit;
    private int _degre = 90;
    public float rotateSped = 1;
    public int catchRadius = 3;
    private float targetCenter;

    public void RotateCubeEdge(string plane, float directionForRotation)
    {
        _controller.GetComponent<ControlManager>().isRotate = true;
        targetCenter = _controller.GetComponent<GenerateCubeEdge>().dimension / 2f + 0.5f;
        Vector3 _plane = Vector3.zero;
        switch (plane)
        {
            case "X":
                _plane = new Vector3(0, catchRadius, catchRadius);
                break;
            case "Y":
                _plane = new Vector3(catchRadius, 0, catchRadius);
                break;
            case "Z":
                _plane = new Vector3(catchRadius, catchRadius, 0);
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


        _selectedUnit = _controller.GetComponent<SelectionManager>().selection;

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
            StartCoroutine(_controller.GetComponent<RotateCube>().RotateColliderAround(collider, Vector3.one * targetCenter, _Axis, _degre * directionForRotation, rotateSped));
        }
        
    }
}
