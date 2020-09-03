using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private string selectableTag = "Edge";
    [SerializeField] private GameObject _Controller;
    private bool isSelected = false;
    private Transform lastSelectedObject;
    private Transform _selectedUnit;
    public Transform selection { get { return _selectedUnit; } }


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
                _selectedUnit = hit.transform;
                Vector3 normQuatSelected = _selectedUnit.transform.rotation.eulerAngles.normalized;
                Debug.Log($"{_selectedUnit.name} {normQuatSelected} {normQuatSelected.x + normQuatSelected.y + normQuatSelected.z}");

                Debug.Log($"local green axis {Mathf.RoundToInt(_selectedUnit.up.z) == 1} {Mathf.RoundToInt(_selectedUnit.up.z)} = {_selectedUnit.up.normalized.z} {_selectedUnit.up}");
                if (isSelected)
                {
                    lastSelectedObject.GetComponent<SelectUnit>().changeSelect();
                }
                lastSelectedObject = _selectedUnit;
                _selectedUnit.GetComponent<SelectUnit>().changeSelect();
                isSelected = true;
            }
            else if (isSelected)
            {
                lastSelectedObject.GetComponent<SelectUnit>().changeSelect();
                isSelected = false;
                _selectedUnit = null;
            }
        }
    }
}
