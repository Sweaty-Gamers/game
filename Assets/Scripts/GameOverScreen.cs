
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MenuScreens
{

    public string titleScreen;

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(titleScreen);
    }
}
