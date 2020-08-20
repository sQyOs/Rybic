using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private string selectableTag = "Cube";
    private bool isSelected = false;
    private Transform lastSelectedObject;
    // Start is called before the first frame update
    private void Awake()
    {
        isSelected = false;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag(selectableTag))
            {
                Transform selection = hit.transform;
                Debug.Log(selection.name);
                if (isSelected)
                {
                    lastSelectedObject.GetComponent<SelectUnit>().changeSelect();
                }
                lastSelectedObject = selection;
                selection.GetComponent<SelectUnit>().changeSelect();
                isSelected = true;

                //StartCoroutine(testCoroutine(selection));

                Collider[] colliders = Physics.OverlapBox(selection.transform.position, new Vector3(2, 0, 2));
                for (int i = 0; i < 10; i++)
                {
                    foreach (Collider item in colliders)
                    {
                        item.transform.RotateAround(Vector3.zero, Vector3.up, 90);
                        //item.transform.RotateAround(Vector3.zero, Vector3.up, 9.0f * Time.deltaTime);
                    } 
                }
            }
            else
            {
                lastSelectedObject.GetComponent<SelectUnit>().changeSelect();
                isSelected = false;
            }
        }
    }
    private IEnumerator testCoroutine(Transform selection)
    {
        for (int i = 0; i < 10; i++)
        {
            selection.transform.RotateAround(Vector3.zero, Vector3.up, 9.0f * Time.deltaTime);
            yield return null; 
        }
    }
}
