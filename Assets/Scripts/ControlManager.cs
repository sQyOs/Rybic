using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    [SerializeField] float positionDifferenceMain = 1;
    [SerializeField] float positionDifferenceDrawGizmos = 5;
    [SerializeField] float positionDifferenceRotate = 60;
    [SerializeField] GameObject rotationControlCircle;
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
        targetCenter = GetComponent<GenerateCubeEdge>().dimension / 2f + 0.5f;

        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //запоминаем стартовое положение курсора и начинаем расчёты движения

        if (Input.GetMouseButtonUp(0))
        {
            rotationControlCircle.SetActive(false);
        }
        if (Input.GetMouseButton(0))
        {
            _selectedUnit = GetComponent<SelectionManager>().selection;
            if (Input.GetMouseButtonDown(0))
            {
                startMousePosition = Input.mousePosition;
                singleClick = true;
                rotationControlCircle.transform.position = startMousePosition;
            }
            if (_selectedUnit != null && singleClick)
            {
                //вычисляем текущие координаты курсора
                currentMousePosition = Input.mousePosition;

                //вычисляем угол поворота камеры по оси Y относительно куба с поправкой на 45 градусов, в секторах по 180, в радианах
                cameraRelativeAngle = ((cameraTransform.eulerAngles.y + 45) % 180) * Mathf.Deg2Rad;

                //активируем UI круг области поворота и поворачиваем в зависимости от выбранной грани
                //если вертикальные 45 градусов, если горизонтальные в зависимости от положения камеры)
                rotationControlCircle.SetActive(true);
                Debug.Log(rotationControlCircle.transform.eulerAngles);
                if (Mathf.Abs(_selectedUnit.up.normalized.y) == 1)
                {
                    rotationControlCircle.transform.eulerAngles = new Vector3(0, 0, _selectedUnit.up.normalized.y * cameraRelativeAngle * Mathf.Rad2Deg % 90);
                }
                else
                {
                    rotationControlCircle.transform.eulerAngles = new Vector3(0, 0, 45);
                }

                //проверяем движение мышью
                if (Vector3.Distance(startMousePosition, currentMousePosition) > positionDifferenceMain)
                {

                    float signCameraPosition = -Mathf.Sign(Vector3.Dot(new Vector3(1, 0, 1) * targetCenter, cameraTransform.position - Vector3.one * targetCenter));

                    //если локальная зеленая(up) ось смотрит вверх или вниз(+-y)
                    if (Mathf.Abs(_selectedUnit.up.normalized.y) == 1)
                    {
                        float signUpDown = Mathf.Sign(_selectedUnit.up.y);
                        Debug.Log("VERTICAL");
                        //вычисляем вектор(ось матрица поворота) для определения угла движения указателя с поправкой на поворот камеры относительно куба
                        //и вектор перпендикуляр для определения знака (верх, низ)
                        Vector3 axisCorrection = new Vector3(Mathf.Sin(_selectedUnit.up.normalized.y * cameraRelativeAngle), -Mathf.Cos(_selectedUnit.up.normalized.y * cameraRelativeAngle));
                        Vector3 axisCorrectionPerpend = new Vector3(-axisCorrection.y / axisCorrection.x, 1);

                        //вычисляем через скалярное произведение знаки движения курсора и расположения камеы
                        float signMouseMove = signUpDown * Mathf.Sign(Vector3.Dot(axisCorrectionPerpend, currentMousePosition - startMousePosition));
                        _sign = _selectedUnit.up.normalized.y * signCameraPosition * signMouseMove;

                        //вычисляем и выводим угол движения указателя и предполагаемое вращение куба по осям относительно камеры
                        //[90, 180] or [-90, 0] : X, [0, 90] or [-180, -90] : Z 
                        mouseMoveAngle = _selectedUnit.up.normalized.y * signMouseMove * Vector3.Angle(axisCorrection, currentMousePosition - startMousePosition);
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
                    if (!isRotate && Vector3.Distance(startMousePosition, currentMousePosition) > positionDifferenceRotate)
                    {
                        GetComponent<RotateManager>().RotateCubeEdge(axisForRotation, _sign);
                        singleClick = false;
                    }
                    Debug.Log($"cameraAngle: {cameraRelativeAngle * Mathf.Rad2Deg} || mouse angle: {mouseMoveAngle} || axis: {axisForRotation} || direction: {_sign} MouseDiff {Vector3.Distance(startMousePosition, currentMousePosition)}");
                }
            }

        }

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(2, 0, 0), new Vector3(4, 0.1f, 0.1f));
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(0, 2, 0), new Vector3(0.1f, 4, 0.1f));
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(new Vector3(0, 0, 2), new Vector3(0.1f, 0.1f, 4));
        Gizmos.color = Color.cyan;
        if (_selectedUnit != null)
        {
            Gizmos.DrawWireCube(_selectedUnit.position, Vector3.one / 2);
            if (Vector3.Distance(startMousePosition, currentMousePosition) > positionDifferenceDrawGizmos)
            {
                Gizmos.color = Color.magenta;
                float dimension = targetCenter + 0.5f * 2;
                //Vector3.one * targetCenter + Vector3.Cross(new Vector3(1, 0, 0), _selectedUnit.position) / 2, Vector3.one
                Vector3 centerForRotation = Vector3.zero;
                Vector3 scaleForRotation = Vector3.zero;
                if (axisForRotation == "X")
                {
                    Gizmos.color = Color.red;
                    centerForRotation = new Vector3(_selectedUnit.position.x, targetCenter, targetCenter);
                    scaleForRotation = new Vector3(1, (targetCenter - 0.5f) * 2, (targetCenter - 0.5f) * 2);
                }
                if (axisForRotation == "Y")
                {
                    Gizmos.color = Color.green;
                    centerForRotation = new Vector3(targetCenter, _selectedUnit.position.y, targetCenter);
                    scaleForRotation = new Vector3((targetCenter - 0.5f) * 2, 1, (targetCenter - 0.5f) * 2);
                }
                if (axisForRotation == "Z")
                {
                    Gizmos.color = Color.blue;
                    centerForRotation = new Vector3(targetCenter, targetCenter, _selectedUnit.position.z);
                    scaleForRotation = new Vector3((targetCenter - 0.5f) * 2, (targetCenter - 0.5f) * 2, 1);
                }
                Gizmos.DrawWireCube(centerForRotation, scaleForRotation);
            }
        }
    }
}
