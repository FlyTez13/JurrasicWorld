using UnityEngine;

public class ModelController : MonoBehaviour
{
    public Transform targetModel;    // Odkaz na model, který má být ovládán
    public float rotationSpeed = 100f; // Rychlost rotace
    public float zoomSpeed = 10f;     // Rychlost pøibližování
    public float minZoom = 5f;        // Minimální vzdálenost kamery od modelu
    public float maxZoom = 20f;       // Maximální vzdálenost kamery od modelu

    private Camera mainCamera;
    private Vector3 initialCameraPosition;
    private Quaternion initialCameraRotation;
    private Vector2 lastTouchPosition; // Pro sledování dotykù

    void Start()
    {
        mainCamera = Camera.main;

        // Uložení poèáteèní pozice kamery
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
        // Ovládání rotace myší (pro PC)
        if (Input.GetMouseButton(0)) // Držení levého tlaèítka myši
        {
            float rotationX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            targetModel.Rotate(Vector3.up, -rotationX, Space.World); // Rotace pouze kolem Y osy
        }

        // Ovládání rotace dotykem (pro mobil)
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDelta = Input.GetTouch(0).deltaPosition;
            float rotationX = touchDelta.x * rotationSpeed * Time.deltaTime;
            targetModel.Rotate(Vector3.up, -rotationX, Space.World); // Rotace pouze kolem Y osy
        }
    }

    void HandleZoom()
    {
        // Zoomování pomocí koleèka myši (pro PC)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            float distance = Vector3.Distance(mainCamera.transform.position, targetModel.position);
            distance -= scroll * zoomSpeed;
            distance = Mathf.Clamp(distance, minZoom, maxZoom);

            Vector3 direction = (targetModel.position - mainCamera.transform.position).normalized;
            mainCamera.transform.position = targetModel.position - direction * distance;
        }

        // Zoomování pomocí gest (pro mobil)
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            // Zmìna vzdálenosti mezi dvìma dotyky
            Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
            float prevMagnitude = (touch0PrevPos - touch1PrevPos).magnitude;
            float currentMagnitude = (touch0.position - touch1.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            float distance = Vector3.Distance(mainCamera.transform.position, targetModel.position);
            distance -= difference * zoomSpeed * 0.01f; // Zmìna rychlosti zoomu
            distance = Mathf.Clamp(distance, minZoom, maxZoom);

            Vector3 direction = (targetModel.position - mainCamera.transform.position).normalized;
            mainCamera.transform.position = targetModel.position - direction * distance;
        }
    }

    public void SetTargetModel(Transform newModel)
    {
        // Nastavení nového modelu pro ovládání
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
