using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour
{
    public IEnumerator RotateColliderAround(Collider collider ,Vector3 point, Vector3 axis, float rotateDegree, float rotateSpeed)
    {
        float step = 0.0f;
        float rate = 1.0f / rotateSpeed;
        float smoothStep = 0.0f;
        float lastStep = 0.0f;
        while (step < 1.0f)
        {
            step += Time.deltaTime * rate;
            smoothStep = Mathf.SmoothStep(0.0f, 1.0f, step);
            collider.transform.RotateAround(point, axis, rotateDegree * (smoothStep - lastStep));
            lastStep = smoothStep;
            yield return null;
        }
        if (step > 1.0)
        {
            transform.RotateAround(point, axis, rotateDegree * (1.0f - lastStep));
        }
    }
}
