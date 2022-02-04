using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private PlayerShoot playerShoot;
    [SerializeField] private GameObject hud;
    [HideInInspector]
    public bool gameOver { get; set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("StartMenu");
        }
    }

    public void SetGameOverScreen()
    {
        gameOver = true;
        scoreText.text = string.Format("Score: {0}", playerShoot.GetScore());
        hud.SetActive(false);
    }
}
