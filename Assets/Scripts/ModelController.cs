using UnityEngine;

public class ModelController : MonoBehaviour
{
    public Transform targetModel;    // Odkaz na model, kter� m� b�t ovl�d�n
    public float rotationSpeed = 100f; // Rychlost rotace
    public float zoomSpeed = 10f;     // Rychlost p�ibli�ov�n�
    public float minZoom = 5f;        // Minim�ln� vzd�lenost kamery od modelu
    public float maxZoom = 20f;       // Maxim�ln� vzd�lenost kamery od modelu

    private Camera mainCamera;
    private Vector3 initialCameraPosition;
    private Quaternion initialCameraRotation;
    private Vector2 lastTouchPosition; // Pro sledov�n� dotyk�

    void Start()
    {
        mainCamera = Camera.main;

        // Ulo�en� po��te�n� pozice kamery
        if (mainCamera != null)
        {
            initialCameraPosition = mainCamera.transform.position;
            initialCameraRotation = mainCamera.transform.rotation;
        }
    }

    void Update()
    {
        if (targetModel != null)
        {
            HandleRotation();
            HandleZoom();
        }
    }

    void HandleRotation()
    {
        // Ovl�d�n� rotace my�� (pro PC)
        if (Input.GetMouseButton(0)) // Dr�en� lev�ho tla��tka my�i
        {
            float rotationX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            targetModel.Rotate(Vector3.up, -rotationX, Space.World); // Rotace pouze kolem Y osy
        }

        // Ovl�d�n� rotace dotykem (pro mobil)
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDelta = Input.GetTouch(0).deltaPosition;
            float rotationX = touchDelta.x * rotationSpeed * Time.deltaTime;
            targetModel.Rotate(Vector3.up, -rotationX, Space.World); // Rotace pouze kolem Y osy
        }
    }

    void HandleZoom()
    {
        // Zoomov�n� pomoc� kole�ka my�i (pro PC)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            float distance = Vector3.Distance(mainCamera.transform.position, targetModel.position);
            distance -= scroll * zoomSpeed;
            distance = Mathf.Clamp(distance, minZoom, maxZoom);

            Vector3 direction = (targetModel.position - mainCamera.transform.position).normalized;
            mainCamera.transform.position = targetModel.position - direction * distance;
        }

        // Zoomov�n� pomoc� gest (pro mobil)
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            // Zm�na vzd�lenosti mezi dv�ma dotyky
            Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
            float prevMagnitude = (touch0PrevPos - touch1PrevPos).magnitude;
            float currentMagnitude = (touch0.position - touch1.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            float distance = Vector3.Distance(mainCamera.transform.position, targetModel.position);
            distance -= difference * zoomSpeed * 0.01f; // Zm�na rychlosti zoomu
            distance = Mathf.Clamp(distance, minZoom, maxZoom);

            Vector3 direction = (targetModel.position - mainCamera.transform.position).normalized;
            mainCamera.transform.position = targetModel.position - direction * distance;
        }
    }

    public void SetTargetModel(Transform newModel)
    {
        // Nastaven� nov�ho modelu pro ovl�d�n�
        targetModel = newModel;
    }

    public void ResetView()
    {
        // Resetuje pozici kamery a rotaci modelu
        if (mainCamera != null)
        {
            mainCamera.transform.position = initialCameraPosition;
            mainCamera.transform.rotation = initialCameraRotation;
        }

        if (targetModel != null)
        {
            targetModel.rotation = Quaternion.identity;
        }
    }
}
