using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;       // Panel hlavního menu
    public GameObject registrationPanel;  // Panel pro registraci

    void Start()
    {
        // Skryje oba panely pøi startu
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (registrationPanel != null) registrationPanel.SetActive(false);

        // Zkontroluje, zda je uživatel pøihlášen
        if (PlayerPrefs.GetInt("IsLoggedIn", 0) == 1)
        {
            // Pokud je pøihlášen, zobrazí hlavní menu
            ShowMainMenuPanel();
        }
        else
        {
            // Pokud není pøihlášen, zkontroluje pøíznak pro zobrazení registraèního panelu
            if (PlayerPrefs.GetInt("ShowRegistrationPanel", 0) == 1)
            {
                ShowRegistrationPanel();
                PlayerPrefs.SetInt("ShowRegistrationPanel", 0); // Resetuje pøíznak
                PlayerPrefs.Save();
            }
            else
            {
                // Pokud není pøihlášen a není nastavena registrace, zobrazí hlavní menu
                ShowMainMenuPanel();
            }
        }
    }

    public void ShowRegistrationPanel()
    {
        if (registrationPanel != null)
        {
            registrationPanel.SetActive(true); // Zobrazí registraèní panel
        }

        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(false); // Skryje hlavní menu
        }
    }

    public void ShowMainMenuPanel()
    {
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true); // Zobrazí hlavní menu
        }

        if (registrationPanel != null)
        {
            registrationPanel.SetActive(false); // Skryje registraèní panel
        }
    }
}
