using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TitleScreen : MonoBehaviour
{

    public string startGameScene;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        startGameScene = "New Scene";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //
    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(startGameScene);
    }

    // Selected when exit game is clicked.
    public void ExitGame()
    {
        Application.Quit();
    }
}
