using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;  // Musí být pro práci s UI komponentami
using Npgsql;

public class FavoriteManager : MonoBehaviour
{
    public Sprite favoriteIconFilled;  // ⭐ Ikona pro oblíbené
    public Sprite favoriteIconEmpty;   // ⚪ Ikona pro neoblíbené
    public Button favoriteButton;      // Tlačítko pro přidání do oblíbených

    public async void ToggleFavorite(int dinosaurId, Toggle favoriteToggle)
    {
        if (!DatabaseManager.Instance.IsConnected())
        {
            Debug.LogError("❌ Databáze není připojena! Nelze změnit oblíbené.");
            return;
        }

        int userId = PlayerPrefs.GetInt("UserID", 0);
        if (userId == 0)
        {
            Debug.LogWarning("⚠ Uživatel není přihlášen! Přesměrování na přihlášení.");
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
            return;
        }

        try
        {
            using (var conn = await DatabaseManager.Instance.GetConnectionAsync())
            {
                bool isFav = await IsFavorite(dinosaurId);
                string query = favoriteToggle.isOn
                    ? "INSERT INTO user_favorites (user_id, dinosaur_id) VALUES (@userId, @dinosaurId)"
                    : "DELETE FROM user_favorites WHERE user_id = @userId AND dinosaur_id = @dinosaurId";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("userId", userId);
                    cmd.Parameters.AddWithValue("dinosaurId", dinosaurId);
                    await cmd.ExecuteNonQueryAsync();
                }

                Debug.Log($"✅ {(favoriteToggle.isOn ? "Přidán do" : "Odebrán z")} oblíbených dinosaurus s ID {dinosaurId}");

                // ✅ Po změně obnovíme ikonu
                UpdateFavoriteToggle(favoriteToggle, dinosaurId);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("❌ Chyba při přidávání do oblíbených: " + ex.Message);
        }
    }

    public async void UpdateFavoriteToggle(Toggle favoriteToggle, int dinosaurId)
    {
        if (favoriteToggle == null)
        {
            Debug.LogError("❌ Chyba: FavoriteToggle není přiřazen!");
            return;
        }

        bool isFav = await IsFavorite(dinosaurId);
        favoriteToggle.isOn = isFav;  // Toggle se změní podle stavu oblíbenosti

        Debug.Log($"🔄 Aktualizace toggle pro {dinosaurId} - {(isFav ? "Oblíbené" : "Neoblíbené")}");
    }

    private async Task<bool> IsFavorite(int dinosaurId)
    {
        using (var conn = await DatabaseManager.Instance.GetConnectionAsync())
        {
            string query = "SELECT COUNT(*) FROM user_favorites WHERE user_id = @userId AND dinosaur_id = @dinosaurId";
            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("userId", PlayerPrefs.GetInt("UserID", 0));
                cmd.Parameters.AddWithValue("dinosaurId", dinosaurId);
                return Convert.ToInt32(await cmd.ExecuteScalarAsync()) > 0;
            }
        }
    }

    // Pokud byste potřebovali funkci pro přidání dinosaura do oblíbených bez toggle (mimo UI)
    public async void AddToFavorites(int dinosaurId)
    {
        if (!DatabaseManager.Instance.IsConnected())
        {
            Debug.LogError("❌ Databáze není připojena! Nelze přidat do oblíbených.");
            return;
        }

        int userId = PlayerPrefs.GetInt("UserID", 0);
        if (userId == 0)
        {
            Debug.LogWarning("⚠ Uživatel není přihlášen! Přesměrování na přihlášení.");
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
            return;
        }

        try
        {
            using (var conn = await DatabaseManager.Instance.GetConnectionAsync())
            {
                bool isFav = await IsFavorite(dinosaurId);
                if (!isFav)
                {
                    string query = "INSERT INTO user_favorites (user_id, dinosaur_id) VALUES (@userId, @dinosaurId)";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("userId", userId);
                        cmd.Parameters.AddWithValue("dinosaurId", dinosaurId);
                        await cmd.ExecuteNonQueryAsync();
                    }

                    Debug.Log($"✅ Dinosaur s ID {dinosaurId} přidán do oblíbených!");
                }
                else
                {
                    Debug.Log("⚠ Dinosaur je již v oblíbených!");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("❌ Chyba při přidávání do oblíbených: " + ex.Message);
        }
    }
}
