using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour
{
    public IEnumerator RotateColliderAround(Collider collider ,Vector3 point, Vector3 axis, float rotateAmount, float rotateTime)
    {
        float step = 0.0f;
        float rate = 1.0f / rotateTime;
        float smoothStep = 0.0f;
        float lastStep = 0.0f;
        while (step < 1.0f)
        {
            step += Time.deltaTime * rate;
            smoothStep = Mathf.SmoothStep(0.0f, 1.0f, step);
            collider.transform.RotateAround(point, axis, rotateAmount * (smoothStep - lastStep));
            lastStep = smoothStep;
            yield return null;
        }
        if (step > 1.0)
        {
            transform.RotateAround(point, axis, rotateAmount * (1.0f - lastStep));
        }
    }
}
