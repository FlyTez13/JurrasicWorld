using UnityEngine;

public class RotatePlanet : MonoBehaviour
{
    public float rotationSpeed = 10f; // Rychlost automatické rotace
    public float dragSpeed = 15f; // Zvýšená citlivost otáèení
    private bool isDragging = false;
    private float lastMouseX;

    void Update()
    {
        // Automatická rotace, pokud uživatel neotáèí
        if (!isDragging)
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
        }

        // Otáèení myší (PC)
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastMouseX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButton(0))
        {
            float deltaX = Input.mousePosition.x - lastMouseX;
            transform.Rotate(Vector3.up * -deltaX * dragSpeed * Time.deltaTime, Space.World);
            lastMouseX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // Otáèení dotykem (pro mobil)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                isDragging = true;
                lastMouseX = touch.position.x;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                float deltaX = touch.position.x - lastMouseX;
                transform.Rotate(Vector3.up * -deltaX * dragSpeed * Time.deltaTime, Space.World);
                lastMouseX = touch.position.x;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isDragging = false;
            }
        }
    }
}
