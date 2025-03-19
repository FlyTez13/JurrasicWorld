using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Npgsql;

public class AuthManager : MonoBehaviour
{
    public TMP_InputField uživatelskéjménoInput;
    public TMP_InputField hesloInput;

    public Button submitButton;
    public Button switchModeButton;
    public Button logoutButton;

    public TextMeshProUGUI statusText;

    private bool isLoginMode = true;

    private void Start()
    {
        submitButton.onClick.AddListener(() => Submit());
        switchModeButton.onClick.AddListener(SwitchMode);
        logoutButton.onClick.AddListener(Logout);

        UpdateUI();
    }

    private void SwitchMode()
    {
        isLoginMode = !isLoginMode;
        UpdateUI();
    }

    private void UpdateUI()
    {
        submitButton.GetComponentInChildren<TMP_Text>().text = isLoginMode ? "Login" : "Register";
        switchModeButton.GetComponentInChildren<TMP_Text>().text = isLoginMode ? "Registration" : "Log in";

        logoutButton.gameObject.SetActive(PlayerPrefs.HasKey("UserID"));
    }

    private void Submit()
    {
        if (isLoginMode)
        {
            Login(uživatelskéjménoInput.text, hesloInput.text);
        }
        else
        {
            Register(uživatelskéjménoInput.text, hesloInput.text);
        }
    }

    public async void Login(string uživatelskéjméno, string heslo)
    {
        if (string.IsNullOrEmpty(uživatelskéjméno) || string.IsNullOrEmpty(heslo))
        {
            UpdateStatus("⚠️ Vyplňte všechny údaje!");
            return;
        }

        try
        {
            var conn = await DatabaseManager.Instance.GetConnectionAsync();
            if (conn == null)
            {
                UpdateStatus("❌ Chyba: Připojení k databázi je null!");
                return;
            }

            string query = "SELECT id FROM users WHERE uživatelskéjméno = @uživatelskéjméno AND heslohash = crypt(@heslo, heslohash);";
            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("uživatelskéjméno", uživatelskéjméno);
                cmd.Parameters.AddWithValue("heslo", heslo);

                object result = await cmd.ExecuteScalarAsync();

                if (result != null)
                {
                    int userId = Convert.ToInt32(result);
                    PlayerPrefs.SetInt("UserID", userId);
                    PlayerPrefs.SetString("uživatelskéjméno", uživatelskéjméno);
                    PlayerPrefs.Save();

                    UpdateStatus("✅ Přihlášení úspěšné!");
                    UnityEngine.SceneManagement.SceneManager.LoadScene("ExploreScene");
                }
                else
                {
                    UpdateStatus("❌ Nesprávné uživatelské jméno nebo heslo.");
                }
            }
        }
        catch (Exception ex)
        {
            UpdateStatus("❌ Chyba při přihlašování: " + ex.Message);
        }
    }

    public async void Register(string uživatelskéjméno, string heslo)
    {
        if (string.IsNullOrEmpty(uživatelskéjméno) || string.IsNullOrEmpty(heslo))
        {
            UpdateStatus("⚠️ Vyplňte všechny údaje!");
            return;
        }

        try
        {
            var conn = await DatabaseManager.Instance.GetConnectionAsync();
            if (conn == null)
            {
                UpdateStatus("❌ Chyba: Připojení k databázi je null!");
                return;
            }

            // NEVKLÁDÁME ID, databáze ho vytvoří sama
            string query = "INSERT INTO users (uživatelskéjméno, heslohash) VALUES (@uživatelskéjméno, crypt(@heslo, gen_salt('bf'))) RETURNING id;";
            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("uživatelskéjméno", uživatelskéjméno);
                cmd.Parameters.AddWithValue("heslo", heslo);

                object result = await cmd.ExecuteScalarAsync();
                if (result != null)
                {
                    int userId = Convert.ToInt32(result);
                    PlayerPrefs.SetInt("UserID", userId);
                    PlayerPrefs.SetString("uživatelskéjméno", uživatelskéjméno);
                    PlayerPrefs.Save();

                    UpdateStatus("✅ Registrace úspěšná!");
                    UnityEngine.SceneManagement.SceneManager.LoadScene("ExploreScene");
                }
                else
                {
                    UpdateStatus("❌ Registrace se nezdařila.");
                }
            }
        }
        catch (Exception ex)
        {
            UpdateStatus("❌ Chyba při registraci: " + ex.Message);
        }
    }


    private void Logout()
    {
        PlayerPrefs.DeleteKey("UserID");
        PlayerPrefs.DeleteKey("uživatelskéjméno");
        PlayerPrefs.Save();

        UpdateStatus("✅ Odhlášeno.");
        UpdateUI();
    }

    private void UpdateStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }
}
