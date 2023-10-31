using UnityEngine;
using UnityEngine.SceneManagement;


public class TitleScreen : MenuScreens
{

    public string startGameScene;

    // Start is called before the first frame update
    void Start()
    {
        startGameScene = "New Scene";
    }

    //
    public void StartGame()
    {
        SceneManager.LoadScene(startGameScene);
    }

    // Selected when exit game is clicked.
    public void ExitGame()
    {
        Application.Quit();
    }
}
