using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    public float rotSpeed = 5f;
    private Vector3 target = new Vector3(0, 0, 0);
    private Vector3 _offset;
    private float _rotY;
    private float _rotX;
    // Start is called before the first frame update
    void Start()
    {
        _rotY = transform.eulerAngles.y;
        _rotX = transform.eulerAngles.x;
        _offset = new Vector3(0, 0, 0) - transform.position;
    }

    
    private void LateUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            _rotY += Input.GetAxis("Mouse X") * rotSpeed;
            _rotX += Input.GetAxis("Mouse Y") * rotSpeed;
            Quaternion rotation = Quaternion.Euler(_rotX, _rotY, 0);
            transform.position = target - (rotation * _offset);
        }
        if (Input.GetMouseButtonDown(2))
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Cube");
            foreach (GameObject gameObject in gameObjects)
            {
                GameObject.Destroy(gameObject);
            }
            print("Materials " + Resources.FindObjectsOfTypeAll(typeof(Material)).Length);
        }
        transform.LookAt(target);
    }
}
