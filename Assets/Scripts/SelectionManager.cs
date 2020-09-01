using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private string selectableTag = "Edge";
    [SerializeField] private GameObject _Controller;
    private bool isSelected = false;
    private Transform lastSelectedObject;
    private Transform _selection;
    public Transform selection { get { return _selection; } }


    private void Awake()
    {
        isSelected = false;
    }

    private void Update()
    {

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
            }
            else if (isSelected)
            {
                lastSelectedObject.GetComponent<SelectUnit>().changeSelect();
                isSelected = false;
            }
        }
    }
}
