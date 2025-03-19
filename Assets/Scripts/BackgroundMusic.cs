using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ✅ Hudba se nezničí při změně scény

            PlayMusic();
        }
    }

    private void PlayMusic()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // Přidání AudioSource
        AudioClip clip = Resources.Load<AudioClip>("background"); // 🎵 Načtení hudby z Resources
        if (clip == null)
        {
            Debug.LogError("❌ Chyba: Hudební soubor nebyl nalezen v Resources!");
            return;
        }

        audioSource.clip = clip;
        audioSource.loop = true; // 🔁 Smyčka hudby
        audioSource.playOnAwake = true;
        audioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f); // 🎚 Načtení hlasitosti
        audioSource.Play(); // ▶ Spuštění hudby
    }

    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = Mathf.Clamp(volume, 0f, 1f);
            PlayerPrefs.SetFloat("MusicVolume", audioSource.volume);
            PlayerPrefs.Save();
        }
    }
}
