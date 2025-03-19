using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;       // Panel hlavn�ho menu
    public GameObject registrationPanel;  // Panel pro registraci

    void Start()
    {
        // Skryje oba panely p�i startu
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (registrationPanel != null) registrationPanel.SetActive(false);

        // Zkontroluje, zda je u�ivatel p�ihl�en
        if (PlayerPrefs.GetInt("IsLoggedIn", 0) == 1)
        {
            // Pokud je p�ihl�en, zobraz� hlavn� menu
            ShowMainMenuPanel();
        }
        else
        {
            // Pokud nen� p�ihl�en, zkontroluje p��znak pro zobrazen� registra�n�ho panelu
            if (PlayerPrefs.GetInt("ShowRegistrationPanel", 0) == 1)
            {
                ShowRegistrationPanel();
                PlayerPrefs.SetInt("ShowRegistrationPanel", 0); // Resetuje p��znak
                PlayerPrefs.Save();
            }
            else
            {
                // Pokud nen� p�ihl�en a nen� nastavena registrace, zobraz� hlavn� menu
                ShowMainMenuPanel();
            }
        }
    }

    public void ShowRegistrationPanel()
    {
        if (registrationPanel != null)
        {
            registrationPanel.SetActive(true); // Zobraz� registra�n� panel
        }

        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(false); // Skryje hlavn� menu
        }
    }

    public void ShowMainMenuPanel()
    {
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true); // Zobraz� hlavn� menu
        }

        if (registrationPanel != null)
        {
            registrationPanel.SetActive(false); // Skryje registra�n� panel
        }
    }
}
