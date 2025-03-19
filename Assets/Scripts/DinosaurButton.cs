using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DinosaurButton : MonoBehaviour
{
    public TMP_Text buttonText;           // Komponenta Text tlaèítka
    public Image icon;                // Komponenta Image pro ikonu

    public Sprite carnivoreIcon;      // Ikona pro masožravce
    public Sprite herbivoreIcon;      // Ikona pro býložravce
    public Sprite aquaticIcon;        // Ikona pro vodní dinosaury
    public Sprite flyingIcon;         // Ikona pro létající dinosaury

    // Metoda pro nastavení tlaèítka
    public void Initialize(string name, string type)
    {
        // Nastavení textu tlaèítka
        if (buttonText != null)
        {
            buttonText.text = name;
        }

        // Nastavení ikony na základì typu
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
                    Debug.LogWarning("Neznámý typ dinosaura: " + type);
                    break;
            }
        }
    }
}
