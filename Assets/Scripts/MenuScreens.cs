using UnityEngine;

public class MenuScreens : MonoBehaviour
{
    
    public void UnlockMouse()
    {
        Time.timeScale = 0f;
    }

    public void LockMouse()
    {
        Time.timeScale = 1f;
    }
}
