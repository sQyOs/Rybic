using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    //_Controller.GetComponent<RotateManager>().RotateCubeEdge(dimensionForRotation, directionForRotation);
    public void RotateHorizontal_Right()
    {
        GetComponent<RotateManager>().RotateCubeEdge("Y", -1);
    }

    public void RotateHorizontal_Left()
    {
        GetComponent<RotateManager>().RotateCubeEdge("Y", 1);
    }

    public void RotateVerticalUp_X()
    {
        GetComponent<RotateManager>().RotateCubeEdge("X", 1);
    }

    public void RotateVerticalUp_Z()
    {
        GetComponent<RotateManager>().RotateCubeEdge("Z", -1);
    }

    public void RotateVerticalDown_X()
    {
        GetComponent<RotateManager>().RotateCubeEdge("X", -1);
    }

    public void RotateVerticalDown_Z()
    {
        GetComponent<RotateManager>().RotateCubeEdge("Z", 1);
    }
}
