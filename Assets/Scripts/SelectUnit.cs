using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectUnit : MonoBehaviour
{
    public bool toggleSelect = false;
    [SerializeField] private Material selectMaterial;
    [SerializeField] private Material unselectMaterial;
    private Material _material;

    //public void OnMouseDown()
    //{
    //    Debug.Log(gameObject.name);
    //    _material = gameObject.GetComponent<Renderer>().materials[5];
    //    _material.SetInt("Boolean_832D321A", 1);
    //    print("Materials " + Resources.FindObjectsOfTypeAll(typeof(Material)).Length);
    //    toggleSelect = true;
    //}

    public void changeSelect()
    {
        toggleSelect = !toggleSelect;
        _material = gameObject.GetComponent<Renderer>().materials[5];
        _material.SetInt("Boolean_832D321A", Convert.ToByte(toggleSelect));
    }
}
