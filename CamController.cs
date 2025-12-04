using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class CamController : MonoBehaviour
{
    [Header("Настройки движения")]
    public Transform pivot;
    public Transform RotPivot;
    public Camera Cam;
       
    [Header("Настройки врещения")]
    public float keyboardRotateSpeed = 50f;
    public float mouseRotateSpeed = 100f;
    public float minVerticalAngle = -70f;
    public float maxVerticalAngle = 70f;
    public float minAgel = -10f;
    public float maxAgel = 10f;
   
    [Header("Настройка зум")]
    public float zoomspeed = 1f;

    [Header("Настройки движения")]
    public  float moveSpeed = 10f;
    public  float edgeScrollSize = 40f; // Размер у края экрана для перемещения курсором

    private float ScreenWidght = Screen.width;
    private float ScreenHeight = Screen.height;
    public float edgeSise = Screen.height * 0.1f;



    void Awake()
    {
      
    }
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        HorizontalMovement();
        RotationCam();
        VerticalRotateCam();
        CameraZoom();
    }
    void HorizontalMovement()
    {
        float moveHorizontal = 0;
        float moveVertical = 0;
        float speed = moveSpeed * Time.deltaTime;
        float speedCamMove = speed;
       
        float horizontalValue;
        float verticalValue;

        Vector3 mousePosition = Input.mousePosition;

        horizontalValue = (mousePosition.x / Screen.width) * 2 - 1;
        verticalValue = (mousePosition.y / Screen.height) * 2 - 1;

        if  (CamLock())
        {
            if (Input.mousePosition.x < edgeScrollSize) moveHorizontal = 2 * Mathf.Pow(horizontalValue, 3f) ;
             if (Input.mousePosition.x > Screen.width - edgeScrollSize) moveHorizontal = 1 * Mathf.Pow(horizontalValue, 3f);
             if (Input.mousePosition.y < edgeScrollSize) moveVertical = 2 * Mathf.Pow(verticalValue, 3f); ;
             if (Input.mousePosition.y > Screen.height - edgeScrollSize) moveVertical = 1 * Mathf.Pow(verticalValue, 3f); ; 
             if (Input.GetAxis("Horizontal") != 0) moveHorizontal = Input.GetAxis("Horizontal")*3;
             if (Input.GetAxis("Vertical") != 0) moveVertical = Input.GetAxis("Vertical")*3;

            Debug.Log("ускорение:" + moveHorizontal);
           


            Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
            pivot.transform.Translate(movement * speedCamMove, Space.Self);
          
        }
    }
    void RotationCam()
    {
        Vector3 Rot = pivot.transform.eulerAngles;
        
        float rotat = 0;
       
        if (CamLock())
        {
            
            if (Input.GetKey(KeyCode.Q)) rotat = -1;
            if (Input.GetKey(KeyCode.E)) rotat = 1;
        }
        else
        {
            if (Input.mousePosition.x < Screen.width/5) rotat = -1 ;
            if (Input.mousePosition.x > Screen.width-Screen.width/5) rotat = 1;
               

        }
            Rot.y += rotat * keyboardRotateSpeed * Time.deltaTime;
        
        pivot.transform.eulerAngles = Rot;
        
        
    }
    
    void VerticalRotateCam()
    {
        float currentXAngle = NormalizeAngle(RotPivot.transform.localEulerAngles.x);
       
        Vector3 RotVert = RotPivot.transform.eulerAngles;
        float rotat = 0;
        if (!CamLock())
        {
            if (Input.mousePosition.y > Screen.height - edgeSise) rotat = -1;
            if (edgeSise > Input.mousePosition.y) rotat = 1;
        }
        float newAngle = currentXAngle + rotat * mouseRotateSpeed * Time.deltaTime;
        newAngle = Mathf.Clamp(newAngle, minAgel, maxAgel);
        RotPivot.transform.localEulerAngles = new Vector3(newAngle, RotPivot.transform.localEulerAngles.y, RotPivot.transform.localEulerAngles.z);
        
        
    }
    void CameraZoom()
    {
        float MoveZ = 0;
        float MoveY = 0;
        Vector3 zoom = new Vector3(0, MoveY, MoveZ);
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            MoveZ = zoomspeed * 1;
            MoveY = zoomspeed * -1;
        }
       if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            MoveZ = zoomspeed * -1;
            MoveY = zoomspeed * 1;
        }
       Vector3 camzoom = new Vector3(0, MoveY, MoveZ);
        
        RotPivot.transform.Translate(camzoom * Time.deltaTime, Space.Self);
    }
    /// <summary>
    /// Проверка - зажато ли колесико мышки
    /// </summary>
    /// <returns></returns>
    public bool CamLock()
    {
     return !Input.GetMouseButton(2);
    }
    private float NormalizeAngle(float angle)
    {
        angle %= 360;
        if (angle > 180) angle -= 360;
        return angle;
    }
}
