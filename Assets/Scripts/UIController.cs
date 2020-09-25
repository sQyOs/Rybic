using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Dropdown tangledLevels;
    public float tanglingSpeed = 0.1f;

    public void CloseApp()
    {
        Application.Quit();
    }

    public void Tangling()
    {
        if (tangledLevels.value != 0)
        {
            float standartRotateSpeed = GetComponent<RotateManager>().rotateSped;
            GetComponent<RotateManager>().rotateSped = tanglingSpeed;
            for (int i = 0; i < tangledLevels.value; i++)
            {
                GetComponent<RotateManager>().RotateCubeEdge("Y", Mathf.Sign(Random.value - 0.5f));
            }
            GetComponent<RotateManager>().rotateSped = standartRotateSpeed;
        }

    }

}
