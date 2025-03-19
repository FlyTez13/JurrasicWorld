using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour 
{
    private string nextScene;
    [SerializeField]
    Animator transitionAnim;

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    public void LoadMainScene()
    {
        nextScene = "MainScene";
        transitionAnim.SetTrigger("End");
    }


    public void QuitApplication()
    {
        Debug.Log("Aplikace byla ukonèena.");
        Application.Quit();
    }
}
