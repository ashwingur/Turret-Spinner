using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Instructions()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}
