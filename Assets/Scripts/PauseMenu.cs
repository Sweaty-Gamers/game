using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{   
    public GameObject settings;
    public GameObject PausePanel;
    public GameObject confirmation;
    public string titleScreen = "Title";
    public void Resume(){
        settings.SetActive(false);
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void Settings(){
        PausePanel.SetActive(false);
        settings.SetActive(true);
    }
    public void Confirm(){
        confirmation.SetActive(true);
        PausePanel.SetActive(false);
    }
    public void Exit(){
        SceneManager.LoadScene("Title");
    }
    public void Back(){
        PausePanel.SetActive(true);
        settings.SetActive(false);
        confirmation.SetActive(false);
    }
}
