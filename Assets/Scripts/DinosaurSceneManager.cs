using UnityEngine;

public class DinosaurSceneManager : MonoBehaviour
{
    [Header("Skyboxes")]
    public Material carnivoreSkybox;  // Skybox pro masožravce
    public Material herbivoreSkybox;  // Skybox pro býložravce
    public Material aquaticSkybox;    // Skybox pro vodní dinosaury
    public Material flyingSkybox;     // Skybox pro létající dinosaury

    void Start()
    {
        // 🟢 Načteme typ dinosaura z `PlayerPrefs`
        string dinosaurType = PlayerPrefs.GetString("SelectedDinosaurType", "unknown").ToLower();
        Debug.Log($"🌍 Načtený typ dinosaura: {dinosaurType}");

        // Nastavíme správný Skybox
        SetSkybox(dinosaurType);
    }

    private void SetSkybox(string type)
    {
        // 🟢 Nastavení správného skyboxu
        switch (type)
        {
            case "carnivore":
                RenderSettings.skybox = carnivoreSkybox;
                Debug.Log("🌄 Nastaven Carnivore Skybox");
                break;
            case "herbivore":
                RenderSettings.skybox = herbivoreSkybox;
                Debug.Log("🌳 Nastaven Herbivore Skybox");
                break;
            case "aquatic":
                RenderSettings.skybox = aquaticSkybox;
                Debug.Log("🌊 Nastaven Aquatic Skybox");
                break;
            case "flying":
                RenderSettings.skybox = flyingSkybox;
                Debug.Log("☁️ Nastaven Flying Skybox");
                break;
            default:
                Debug.LogWarning($"⚠️ Neznámý typ dinosaura: {type}. Používám výchozí nastavení.");
                break;
        }
    }
}
