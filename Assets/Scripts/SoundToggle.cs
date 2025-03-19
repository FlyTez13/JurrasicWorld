using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundToggle : MonoBehaviour
{
    public Button soundToggleButton; // Tla��tko
    public TMP_Text soundToggleText; // Text na tla��tku
    public Image soundToggleImage; // Ikona na tla��tku
    public Sprite soundOnSprite; // Obr�zek pro zapnut� zvuk
    public Sprite soundOffSprite; // Obr�zek pro vypnut� zvuk

    private bool isMuted;

    private void Start()
    {
        // Na�ten� p�edchoz�ho nastaven� zvuku
        isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;
        UpdateSound();

        // P�id�n� funkce pro kliknut� na tla��tko
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
        // Nastaven� hlasitosti
        AudioListener.volume = isMuted ? 0f : 1f;

        // Aktualizace textu
        soundToggleText.text = isMuted ? "Zvuk: OFF" : "Zvuk: ON";

        // Aktualizace obr�zku
        soundToggleImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
    }
}
