using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public GameObject GameScene;
    public GameObject GameplayUI;

    public GameObject StartMenu;

    public GameManager Manager;

    void Awake()
    {
        if (gameObject.name == "StartMenuUI")
        {
            GameScene.SetActive(false);
            GameplayUI.SetActive(false);
            StartMenu.SetActive(true);
        }
    }

    public void OnStartPressed()
    {
        //FindObjectOfType<MusicManager>().Stop("Opening");
        //FindObjectOfType<MusicManager>().Play("GameplayMusic");

        Time.timeScale = 0;
        Manager.Player.GetComponent<AudioSource>().enabled = false;
        Manager.Player.GetComponent<CharController>().enabled = false;
        Manager.Tutorial.SetActive(true);

        GameScene.SetActive(true);
        GameplayUI.SetActive(true);
        StartMenu.SetActive(false);
    }

    public void OnQuitPressed()
    {
        Application.Quit();
    }

    public void OnRestartPresed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
