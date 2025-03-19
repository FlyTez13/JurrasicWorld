using UnityEngine;

public class DinosaurViewer : MonoBehaviour
{
    [System.Serializable]
    public class DinosaurSpawn
    {
        public GameObject model;
        public Transform spawnPoint;
    }

    public DinosaurSpawn[] dinosaurSpawns;
    private GameObject spawnedDinosaur;

    [Header("Rotation Settings")]
    public float rotationSpeed = 100f;

    [Header("Zoom Settings")]
    public float zoomSpeed = 1f;
    public float minZoom = 0.5f;
    public float maxZoom = 3f;

    void Start()
    {
        string selectedDinosaur = PlayerPrefs.GetString("SelectedDinosaur", "Unknown").ToLower();
        Debug.Log("🦖 Vybraný dinosaurus: " + selectedDinosaur);

        foreach (DinosaurSpawn dinosaur in dinosaurSpawns)
        {
            if (dinosaur.model != null && dinosaur.model.name.ToLower().Equals(selectedDinosaur))
            {
                if (dinosaur.spawnPoint != null)
                {
                    spawnedDinosaur = Instantiate(dinosaur.model, dinosaur.spawnPoint.position, dinosaur.spawnPoint.rotation, dinosaur.spawnPoint);
                    Debug.Log($"✅ Model {selectedDinosaur} byl úspěšně nahrán!");
                }
                else
                {
                    Debug.LogError($"❌ Spawnpoint není nastaven pro model: {dinosaur.model.name}");
                }
                return;
            }
        }

        Debug.LogError("❌ Model nebo spawnpoint pro vybraného dinosaura nebyl nalezen: " + selectedDinosaur);
    }

    void Update()
    {
        if (spawnedDinosaur != null)
        {
            HandleRotation();
            HandleZoom();
        }
    }

    private void HandleRotation()
    {
        if (Input.GetMouseButton(0))
        {
            float horizontalRotation = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            spawnedDinosaur.transform.Rotate(0, -horizontalRotation, 0, Space.World);
        }

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                float horizontalRotation = touch.deltaPosition.x * rotationSpeed * Time.deltaTime;
                spawnedDinosaur.transform.Rotate(0, -horizontalRotation, 0, Space.World);
            }
        }
    }

    private void HandleZoom()
    {
        Vector3 currentScale = spawnedDinosaur.transform.localScale;

        if (Input.GetKey(KeyCode.W))
        {
            Vector3 newScale = currentScale + Vector3.one * zoomSpeed * Time.deltaTime;
            spawnedDinosaur.transform.localScale = ClampScale(newScale);
        }

        if (Input.GetKey(KeyCode.S))
        {
            Vector3 newScale = currentScale - Vector3.one * zoomSpeed * Time.deltaTime;
            spawnedDinosaur.transform.localScale = ClampScale(newScale);
        }
    }

    private Vector3 ClampScale(Vector3 scale)
    {
        scale.x = Mathf.Clamp(scale.x, minZoom, maxZoom);
        scale.y = Mathf.Clamp(scale.y, minZoom, maxZoom);
        scale.z = Mathf.Clamp(scale.z, minZoom, maxZoom);
        return scale;
    }
}
