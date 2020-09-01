using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testMatScript : MonoBehaviour
{
    [Range(0.1f, 0.9f)]
    [SerializeField] private float maxTransparency = 0.3f;
    private void OnMouseEnter()
    {
        this.gameObject.GetComponent<Renderer>().materials[1].SetFloat("transparency", maxTransparency);
        //gameObject.GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value);
    }
    private void OnMouseExit()
    {
        this.gameObject.GetComponent<Renderer>().materials[1].SetFloat("transparency", 0f);
    }
}
