using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMouseEnterShaderOutline : MonoBehaviour
{
    [Range(0.1f, 0.9f)]
    [SerializeField] private float maxTransparency = 0.3f;

    private void OnMouseEnter()
    {
        gameObject.GetComponent<Renderer>().materials[1].SetFloat("transparency", maxTransparency);
    }

    private void OnMouseExit()
    {
        if (!gameObject.GetComponent<SelectUnit>().toggleSelect)
        {
            gameObject.GetComponent<Renderer>().materials[1].SetFloat("transparency", 0f);
        }
    }
}
