using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    [SerializeField] GameObject controller;
    [SerializeField] float positionDifference = 1;
    private float targetCenter = 0;
    private Vector3 startMousePosition = Vector3.zero;
    private Vector3 currentMousePosition = Vector3.zero;
    private float mouseMoveAngle = 0;
    private float cameraRelativeAngle = 0;
    private string axisForRotation = "";
    private float _sign;
    private Transform cameraTransform;
    private Transform _selectedUnit;
    public bool isRotate = false;
    private bool singleClick = false;

    // Start is called before the first frame update
    void Start()
    {
        //получаем центральную координату куба в зависимости от размерности
        targetCenter = controller.GetComponent<GenerateCubeEdge>().dimension / 2f + 0.5f;

        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //запоминаем стартовое положение курсора и начинаем расчёты движения

        if (Input.GetMouseButton(0))
        {
            _selectedUnit = controller.GetComponent<SelectionManager>().selection;
            if (Input.GetMouseButtonDown(0))
            {
                startMousePosition = Input.mousePosition;
                singleClick = true;
            }
            if (_selectedUnit != null && singleClick)
            {
                //вычисляем текущие координаты курсора
                currentMousePosition = Input.mousePosition;


                if (Vector3.Distance(startMousePosition, currentMousePosition) > positionDifference)
                {
                    //вычисляем угол поворота камеры по оси Y относительно куба с поправкой на 45 градусов, в секторах по 180, в радианах
                    cameraRelativeAngle = ((cameraTransform.eulerAngles.y + 45) % 180) * Mathf.Deg2Rad;
                    float signCameraPosition = -Mathf.Sign(Vector3.Dot(new Vector3(1, 0, 1) * targetCenter, cameraTransform.position - Vector3.one * targetCenter));

                    if (Mathf.Abs(_selectedUnit.up.normalized.y) == 1)
                    {
                        float signUpDown = Mathf.Sign(_selectedUnit.up.y);
                        Debug.Log("VERTICAL");
                        //вычисляем вектор(ось матрица поворота) для определения угла движения указателя с поправкой на поворот камеры относительно куба
                        //и вектор перпендикуляр для определения знака (верх, низ)
                        Vector3 axisCorrection = new Vector3(Mathf.Sin(cameraRelativeAngle), -Mathf.Cos(cameraRelativeAngle));
                        Vector3 axisCorrectionPerpend = new Vector3(-axisCorrection.y / axisCorrection.x, 1);

                        //вычисляем через скалярное произведение знаки движения курсора и расположения камеы
                        float signMouseMove = signUpDown* Mathf.Sign(Vector3.Dot(axisCorrectionPerpend, currentMousePosition - startMousePosition));
                        _sign = signCameraPosition * signMouseMove;

                        //вычисляем и выводим угол движения указателя и предполагаемое вращение куба по осям относительно камеры
                        //[90, 180] or [-90, 0] : X, [0, 90] or [-180, -90] : Z 
                        mouseMoveAngle = signMouseMove * Vector3.Angle(axisCorrection, currentMousePosition - startMousePosition);
                        if (mouseMoveAngle > 90 || (mouseMoveAngle < 0 && mouseMoveAngle > -90))
                        {
                            axisForRotation = "X";
                        }
                        else
                        {
                            axisForRotation = "Z";
                        }
                    }
                    else
                    {
                        float signMouseMove = Mathf.Sign(Vector3.Dot(new Vector3(1, 1), currentMousePosition - startMousePosition));
                        _sign = signCameraPosition * signMouseMove;
                        mouseMoveAngle = signMouseMove * Vector3.Angle(new Vector3(1, -1), currentMousePosition - startMousePosition);
                        if (mouseMoveAngle > 90 || (mouseMoveAngle < 0 && mouseMoveAngle > -90))
                        {
                            if (Mathf.Abs(_selectedUnit.up.normalized.z) == 1)
                            {
                                axisForRotation = "X";
                            }
                            else
                            {
                                axisForRotation = "Z";

                            }
                        }
                        else
                        {
                            axisForRotation = "Y";
                            _sign = -signMouseMove;
                        }
                    }
                    if (!isRotate && Vector3.Distance(startMousePosition, currentMousePosition) > positionDifference * 100)
                    {
                        controller.GetComponent<RotateManager>().RotateCubeEdge(axisForRotation, _sign);
                        singleClick = false;
                    }
                    Debug.Log($"cameraAngle: {cameraRelativeAngle * Mathf.Rad2Deg} || mouse angle: {mouseMoveAngle} || axis: {axisForRotation} || direction: {_sign} ");
                }
            }

        }

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if (_selectedUnit != null)
        {
            Gizmos.DrawWireCube(_selectedUnit.position, Vector3.one);
            if (Vector3.Distance(startMousePosition, currentMousePosition) > positionDifference)
            {
                Gizmos.color = Color.magenta;
                float dimension = targetCenter + 0.5f * 2;
                //Vector3.one * targetCenter + Vector3.Cross(new Vector3(1, 0, 0), _selectedUnit.position) / 2, Vector3.one
                Vector3 centerForRotation = Vector3.zero;
                Vector3 scaleForRotation = Vector3.zero;
                if (axisForRotation == "X")
                {
                    centerForRotation = new Vector3(_selectedUnit.position.x, targetCenter, targetCenter);
                    scaleForRotation = new Vector3(1, (targetCenter - 0.5f) * 2, (targetCenter - 0.5f) * 2);
                }
                if (axisForRotation == "Y")
                {
                    centerForRotation = new Vector3(targetCenter, _selectedUnit.position.y, targetCenter);
                    scaleForRotation = new Vector3((targetCenter - 0.5f) * 2, 1, (targetCenter - 0.5f) * 2);
                }
                if (axisForRotation == "Z")
                {
                    centerForRotation = new Vector3(targetCenter, targetCenter, _selectedUnit.position.z);
                    scaleForRotation = new Vector3((targetCenter - 0.5f) * 2, (targetCenter - 0.5f) * 2, 1);
                }
                Gizmos.DrawWireCube(centerForRotation, scaleForRotation);
            }
        }
    }
}
