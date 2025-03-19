using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundToggle : MonoBehaviour
{
    public Button soundToggleButton; // Tlaèítko
    public TMP_Text soundToggleText; // Text na tlaèítku
    public Image soundToggleImage; // Ikona na tlaèítku
    public Sprite soundOnSprite; // Obrázek pro zapnutý zvuk
    public Sprite soundOffSprite; // Obrázek pro vypnutý zvuk

    private bool isMuted;

    private void Start()
    {
        // Naètení pøedchozího nastavení zvuku
        isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;
        UpdateSound();

        // Pøidání funkce pro kliknutí na tlaèítko
        soundToggleButton.onClick.AddListener(ToggleSound);
    }

    private void ToggleSound()
    {
        isMuted = !isMuted;
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
        UpdateSound();
    }

    private void UpdateSound()
    {
        // Nastavení hlasitosti
        AudioListener.volume = isMuted ? 0f : 1f;

        // Aktualizace textu
        soundToggleText.text = isMuted ? "Zvuk: OFF" : "Zvuk: ON";

        // Aktualizace obrázku
        soundToggleImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
    }
}
