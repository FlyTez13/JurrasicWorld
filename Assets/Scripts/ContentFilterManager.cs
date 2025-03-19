using UnityEngine;
using UnityEngine.UI;

public class ContentFilterManager : MonoBehaviour
{
    public Button exploreButton;
    public Button favoriteButton;
    public Button movieStarsButton;

    private DataLoader dataLoader;

    private void Start()
    {
        dataLoader = FindObjectOfType<DataLoader>();

        if (dataLoader == null)
        {
            Debug.LogError("❌ DataLoader nebyl nalezen ve scéně!");
            return;
        }

        exploreButton.onClick.AddListener(() => SetFilter("Explore"));
        favoriteButton.onClick.AddListener(() => SetFilter("Favorite"));
        movieStarsButton.onClick.AddListener(() => SetFilter("MovieStars"));
    }

    private void SetFilter(string filter)
    {
        Debug.Log($"🔍 Filtr nastaven na: {filter}");
        dataLoader.LoadFilteredData(filter);
    }
}
