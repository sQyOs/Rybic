using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectUnit : MonoBehaviour
{
    public bool toggleSelect = false;

    public void changeSelect()
    {
        toggleSelect = !toggleSelect;
        float toggleTransparency = toggleSelect ? 0.3f : 0f;
        Material _material = gameObject.GetComponent<Renderer>().materials[1];
        _material.SetFloat("transparency", toggleTransparency);
    }
}
