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
        if (Input.GetMouseButton(0))
        {
            _selectedUnit = controller.GetComponent<SelectionManager>().selection;

            //вычисляем текущие координаты курсора
            currentMousePosition = Input.mousePosition;


            //запоминаем стартовое положение курсора и начинаем расчёты движения
            if (Input.GetMouseButtonDown(0))
            {
                startMousePosition = currentMousePosition;
            }
            if (Vector3.Distance(startMousePosition, currentMousePosition) > positionDifference)
            {
                //вычисляем угол поворота камеры по оси Y относительно куба с поправкой на 45 градусов, в секторах по 180, в радианах
                cameraRelativeAngle = ((cameraTransform.eulerAngles.y + 45) % 180) * Mathf.Deg2Rad;
                float signCameraPosition = -Mathf.Sign(Vector3.Dot(new Vector3(1, 0, 1) * targetCenter, cameraTransform.position - Vector3.one * targetCenter));

                if ((_selectedUnit.eulerAngles.normalized.x + _selectedUnit.eulerAngles.normalized.y + _selectedUnit.eulerAngles.normalized.z) != 1)
                {
                    //вычисляем вектор(ось матрица поворота) для определения угла движения указателя с поправкой на поворот камеры относительно куба
                    //и вектор перпендикуляр для определения знака (верх, низ)
                    Vector3 axisCorrection = new Vector3(Mathf.Sin(cameraRelativeAngle), -Mathf.Cos(cameraRelativeAngle));
                    Vector3 axisCorrectionPerpend = new Vector3(-axisCorrection.y / axisCorrection.x, 1);

                    //вычисляем через скалярное произведение знаки движения курсора и расположения камеы(инвертированно под левостороннюю систему юнити)
                    float signMouseMove = Mathf.Sign(Vector3.Dot(axisCorrectionPerpend, currentMousePosition - startMousePosition));
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
                        if (cameraRelativeAngle < 1.57f)
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
                        _sign = signMouseMove;
                    }
                }
                Debug.Log($"cameraAngle: {cameraRelativeAngle * Mathf.Rad2Deg} || mouse angle: {mouseMoveAngle} || axis: {axisForRotation} || direction: {_sign} ");
            }

        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if (_selectedUnit != null)
        {
            Gizmos.DrawWireCube(_selectedUnit.position, Vector3.one);
            //if (Vector3.Distance(startMousePosition, currentMousePosition) > positionDifference)
            //{
            //    Gizmos.color = Color.magenta;
            //    float dimension = targetCenter + 0.5f * 2;
            //    if (mouseMoveAngle > 90)//+X vector forward
            //    {
            //        Gizmos.DrawWireCube(Vector3.one * targetCenter + Vector3.Cross(new Vector3(1, 0, 0), _selectedUnit.position) / 2, Vector3.one);
            //        Debug.Log($"cameraAngle: {cameraRelativeAngle * Mathf.Rad2Deg} || mouse angle: {mouseMoveAngle} || axis: {"X"} || direction: {_sign} ");
            //    }
            //    else if (mouseMoveAngle > 0)//+Z vector right
            //    {
            //        Gizmos.DrawWireCube(Vector3.one * targetCenter + Vector3.Cross(new Vector3(0, 0, 1), _selectedUnit.position) / 2, Vector3.one);
            //        Debug.Log($"cameraAngle: {cameraRelativeAngle * Mathf.Rad2Deg} || mouse angle: {mouseMoveAngle} || axis: {"Z"} || direction: {_sign} ");
            //    }
            //    else if (mouseMoveAngle > -90)//-X
            //    {
            //        Gizmos.DrawWireCube(Vector3.one * targetCenter + Vector3.Cross(new Vector3(1, 0, 0), _selectedUnit.position) / 2, Vector3.one);
            //        Debug.Log($"cameraAngle: {cameraRelativeAngle * Mathf.Rad2Deg} || mouse angle: {mouseMoveAngle} || axis: {"X"} || direction: {_sign} ");
            //    }
            //    else//-Z
            //    {
            //        Gizmos.DrawWireCube(Vector3.one * targetCenter + Vector3.Cross(new Vector3(0, 0, 1), _selectedUnit.position) / 2, Vector3.one);
            //        Debug.Log($"cameraAngle: {cameraRelativeAngle * Mathf.Rad2Deg} || mouse angle: {mouseMoveAngle} || axis: {"Z"} || direction: {_sign} ");
            //    } 
            //}
        }
    }
}
