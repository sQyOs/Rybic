using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private string selectableTag = "Cube";
    [SerializeField] private GameObject _Controller;
    private bool isSelected = false;
    private Transform lastSelectedObject;
    private Transform _selection;
    public Transform selection { get { return _selection; } }

    //private enum directionList
    //{
    //    clockwise = 1,
    //    counterclockwise = 2

    //}
    //[SerializeField] private directionList direction = directionList.clockwise;
    //private int directionForRotation = -1;

    //private enum axesList
    //{
    //    X = 1,
    //    Y = 2,
    //    Z = 3
    //}

    //[SerializeField] private axesList axes = axesList.X;

    //private string dimensionForRotation = "X";


    private void Awake()
    {
        isSelected = false;
    }

    private void Update()
    {
        //switch (axes)
        //{
        //    case axesList.X:
        //        dimensionForRotation = "X";
        //        break;
        //    case axesList.Y:
        //        dimensionForRotation = "Y";
        //        break;
        //    case axesList.Z:
        //        dimensionForRotation = "Z";
        //        break;
        //    default:
        //        dimensionForRotation = "X";
        //        break;
        //}

        //switch (direction)
        //{
        //    case directionList.clockwise:
        //        directionForRotation = 1;
        //        break;
        //    case directionList.counterclockwise:
        //        directionForRotation = -1;
        //        break;
        //    default:
        //        directionForRotation = 1;
        //        break;
        //}

        //left mouse click, cast ray
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag(selectableTag))
            {
                _selection = hit.transform;
                Debug.Log(_selection.name);
                if (isSelected)
                {
                    lastSelectedObject.GetComponent<SelectUnit>().changeSelect();
                }
                lastSelectedObject = _selection;
                _selection.GetComponent<SelectUnit>().changeSelect();
                isSelected = true;

                //_Controller.GetComponent<RotateManager>().RotateCubeEdge(dimensionForRotation, directionForRotation);
            }
            else if (isSelected)
            {
                lastSelectedObject.GetComponent<SelectUnit>().changeSelect();
                isSelected = false;
            }
        }
    }
}
