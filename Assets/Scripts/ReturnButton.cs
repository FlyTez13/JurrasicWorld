using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnButton : MonoBehaviour
{
    public string mainMenuSceneName = "MainMenu";
    public string exploreSceneName = "ExploreScene";

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void GoToExplore()
    {
        SceneManager.LoadScene(exploreSceneName);
    }
}
