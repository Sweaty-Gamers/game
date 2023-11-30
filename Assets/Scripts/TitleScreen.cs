using UnityEngine;
using UnityEngine.SceneManagement;


public class TitleScreen : MenuScreens
{

    public string startGameScene;
    public string customGameScene;
    // Start is called before the first frame update
    void Start()
    {
        startGameScene = "Official Map";
        customGameScene = "Custom";
    }

    //
    public void StartGame()
    {
        SceneManager.LoadScene(startGameScene);
    }
    public void CustomGame()
    {
        SceneManager.LoadScene(customGameScene);
    }

    // Selected when exit game is clicked.
    public void ExitGame()
    {
        Application.Quit();
    }
}
