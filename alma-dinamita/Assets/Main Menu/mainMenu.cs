using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public void StartMap1()
    {
        SceneManager.LoadScene("TestScene");
    }
    public void LoadMapSelection()
    {
        SceneManager.LoadScene("LocalSelectMap");
    }

    public void Settings()
    {
        Debug.Log("Settings");
    }
    // Exit the game
    public void ExitGame()
    {
        #if UNITY_EDITOR
        Debug.Log("UnityEditor will be stopped");
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
