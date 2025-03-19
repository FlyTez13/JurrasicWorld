using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Npgsql;

public class DataLoader : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform content;

    // Ikony pro různé typy dinosaurů
    public Sprite carnivoreIcon;
    public Sprite herbivoreIcon;
    public Sprite aquaticIcon;
    public Sprite flyingIcon;

    private async void Start()
    {
        Debug.Log("🔄 Spouštím DataLoader...");

        while (!DatabaseManager.Instance.IsConnected())
        {
            await Task.Delay(500);
        }

        Debug.Log("✅ Databáze připojena! Načítám data...");
        await LoadFilteredData("Explore"); // 🟢 Defaultně zobrazit všechny dinosaury
    }

    public async Task LoadFilteredData(string filter)
    {
        try
        {
            var conn = await DatabaseManager.Instance.GetConnectionAsync();
            if (conn == null)
            {
                Debug.LogError("❌ Chyba: Připojení k databázi je null!");
                return;
            }

            string query = "";

            // 🟢 Určení SQL dotazu podle filtru
            switch (filter)
            {
                case "Explore":
                    query = "SELECT id, jméno, typ FROM dinosaurs ORDER BY jméno ASC;";
                    break;
                case "Favorite":
                    int userId = PlayerPrefs.GetInt("UserID", 0);
                    if (userId == 0)
                    {
                        Debug.LogWarning("⚠️ Uživatel není přihlášen! Nelze načíst oblíbené.");
                        return;
                    }
                    query = $"SELECT d.id, d.jméno, d.typ FROM dinosaurs d INNER JOIN user_favorites uf ON d.id = uf.dinosaur_id WHERE uf.user_id = {userId};";
                    break;
                case "MovieStars":
                    query = "SELECT id, jméno, typ FROM dinosaurs WHERE id IN (1, 2, 4, 16, 22, 10, 31, 42, 49, 22);"; // 🟡 Určité ID dinosaurů
                    break;
                default:
                    Debug.LogWarning("⚠️ Neznámý filtr, načítám všechny dinosaury.");
                    query = "SELECT id, jméno, typ FROM dinosaurs ORDER BY jméno ASC;";
                    break;
            }

            // 🟢 Smazání existujících tlačítek před novým načtením
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }

            using (var cmd = new NpgsqlCommand(query, conn))
            {
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (!reader.HasRows)
                    {
                        Debug.LogWarning("⚠️ Dotaz nevrátil žádná data.");
                        return;
                    }

                    while (reader.Read())
                    {
                        int dinosaurId = reader.GetInt32(0);
                        string dinosaurName = reader.GetString(1);
                        string dinosaurType = reader.GetString(2);

                        UnityMainThreadDispatcher.Instance().Enqueue(() =>
                        {
                            CreateButton(dinosaurId, dinosaurName, dinosaurType);
                        });
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("❌ Chyba při načítání dat: " + ex.Message);
        }
    }

    private void CreateButton(int dinosaurId, string name, string type)
    {
        if (buttonPrefab == null || content == null)
        {
            Debug.LogError("❌ Chyba: buttonPrefab nebo content není přiřazen!");
            return;
        }

        GameObject newButton = Instantiate(buttonPrefab, content);
        TMP_Text buttonTMP = newButton.GetComponentInChildren<TMP_Text>();
        if (buttonTMP != null)
        {
            buttonTMP.text = name;
        }
        else
        {
            Debug.LogError($"⚠️ Tlačítko pro {name} nemá TMP_Text!");
        }

        // 🟢 Přidání ikonky typu dinosaura
        Image typeIcon = newButton.transform.Find("TypeIcon")?.GetComponent<Image>();
        if (typeIcon != null)
        {
            typeIcon.sprite = GetDinosaurTypeIcon(type);
        }
        else
        {
            Debug.LogWarning($"⚠️ Chybí objekt TypeIcon pro {name}!");
        }

        Button buttonComponent = newButton.GetComponent<Button>();
        if (buttonComponent != null)
        {
            buttonComponent.onClick.AddListener(() => OnDinosaurSelected(name, type)); 
        }
    }

    private Sprite GetDinosaurTypeIcon(string type)
    {
        switch (type.ToLower())
        {
            case "carnivore": return carnivoreIcon;
            case "herbivore": return herbivoreIcon;
            case "aquatic": return aquaticIcon;
            case "flying": return flyingIcon;
            default: return null;
        }
    }

    private void OnDinosaurSelected(string name, string type)
    {
        Debug.Log($"🦖 Kliknuto na: {name} (Typ: {type})");

        PlayerPrefs.SetString("SelectedDinosaur", name);
        PlayerPrefs.SetString("SelectedDinosaurType", type); // ✅ Uložit typ dinosaura!
        PlayerPrefs.Save();

        UnityEngine.SceneManagement.SceneManager.LoadScene("DinosaurScene");
    }


}
