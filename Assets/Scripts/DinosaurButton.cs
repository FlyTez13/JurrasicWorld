using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DinosaurButton : MonoBehaviour
{
    public TMP_Text buttonText;           // Komponenta Text tla��tka
    public Image icon;                // Komponenta Image pro ikonu

    public Sprite carnivoreIcon;      // Ikona pro maso�ravce
    public Sprite herbivoreIcon;      // Ikona pro b�lo�ravce
    public Sprite aquaticIcon;        // Ikona pro vodn� dinosaury
    public Sprite flyingIcon;         // Ikona pro l�taj�c� dinosaury

    // Metoda pro nastaven� tla��tka
    public void Initialize(string name, string type)
    {
        // Nastaven� textu tla��tka
        if (buttonText != null)
        {
            buttonText.text = name;
        }

        // Nastaven� ikony na z�klad� typu
        if (icon != null)
        {
            switch (type.ToLower())
            {
                case "carnivore":
                    icon.sprite = carnivoreIcon;
                    break;
                case "herbivore":
                    icon.sprite = herbivoreIcon;
                    break;
                case "aquatic":
                    icon.sprite = aquaticIcon;
                    break;
                case "flying":
                    icon.sprite = flyingIcon;
                    break;
                default:
                    Debug.LogWarning("Nezn�m� typ dinosaura: " + type);
                    break;
            }
        }
    }
}
