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
    private Transform cameraTransform;

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

                //вычисляем вектор(ось матрица поворота) для определения угла движения указателя с поправкой на поворот камеры относительно куба
                //и вектор перпендикуляр для определения знака (верх, низ)
                Vector3 axisCorrection = new Vector3(Mathf.Sin(cameraRelativeAngle), -Mathf.Cos(cameraRelativeAngle));
                Vector3 axisCorrectionPerpend = new Vector3(-axisCorrection.y / axisCorrection.x, 1);

                //вычисляем через скалярное произведение знаки движения курсора и расположения камеы(инвертированно под левостороннюю систему юнити)
                float signMouseMove = Mathf.Sign(Vector3.Dot(axisCorrectionPerpend, currentMousePosition - startMousePosition));
                float signCameraPosition = -Mathf.Sign(Vector3.Dot(new Vector3(1, 0, 1) * targetCenter, cameraTransform.position - Vector3.one * targetCenter));

                //вычисляем и выводим угол движения указателя и предполагаемое вращение куба по осям относительно камеры
                //[90, 180] +X, [0, 90] +Z, [-90, 0] -X, [-180, -90] -Z 
                mouseMoveAngle = signMouseMove * Vector3.Angle(axisCorrection, currentMousePosition - startMousePosition);
                if (mouseMoveAngle > 90)
                {
                    Debug.Log($"cameraAngle: {cameraRelativeAngle * Mathf.Rad2Deg} || mouse angle: {mouseMoveAngle} || axis: {"X"} || direction: {signCameraPosition * signMouseMove} ");
                }
                else if (mouseMoveAngle > 0)
                {
                    Debug.Log($"cameraAngle: {cameraRelativeAngle * Mathf.Rad2Deg} || mouse angle: {mouseMoveAngle} || axis: {"Z"} || direction: {signCameraPosition * signMouseMove} ");
                }
                else if (mouseMoveAngle > -90)
                {
                    Debug.Log($"cameraAngle: {cameraRelativeAngle * Mathf.Rad2Deg} || mouse angle: {mouseMoveAngle} || axis: {"X"} || direction: {signCameraPosition * signMouseMove} ");
                }
                else
                {
                    Debug.Log($"cameraAngle: {cameraRelativeAngle * Mathf.Rad2Deg} || mouse angle: {mouseMoveAngle} || axis: {"Z"} || direction: {signCameraPosition * signMouseMove} ");
                }

            }

        }

    }
}
