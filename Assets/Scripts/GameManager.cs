using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    [Header("Turn Off at Start")]
    public GameObject Menu;
    public GameObject YeeHaw;
    public GameObject WaveCounterObj;
    [Header("Score System")]
    public GameObject VictoryUI;
    public Text ekText;
    public Text lcText;
    public Text ekText1;
    public Text lcText1;
    public static int Score = 0;
    public static float LiquorCollected = 0f;
    [Header("Pause System")]
    public GameObject Tutorial;
    public GameObject Player;

	void Awake ()
    {
        Tutorial.SetActive(false);
        Menu.SetActive(true);
        YeeHaw.SetActive(false);
        WaveCounterObj.SetActive(false);
    }
	
	void Update ()
    {
        if (YeeHaw.activeSelf)
        {
            ekText.text = "Enemies Killed : " + Score;
            lcText.text = "Liquor Collected : " + LiquorCollected;
        }
        if (VictoryUI.activeSelf)
        {
            ekText1.text = "Enemies Killed : " + Score;
            lcText1.text = "Liquor Collected : " + LiquorCollected;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!Tutorial.activeInHierarchy)
            {
                Time.timeScale = 0;
                Player.GetComponent<AudioSource>().enabled = false;
                Player.GetComponent<CharController>().enabled = false;
                Tutorial.SetActive(true);
            }
            else if (Tutorial.activeInHierarchy)
            {
                Time.timeScale = 1;
                Player.GetComponent<AudioSource>().enabled = true;
                Player.GetComponent<CharController>().enabled = true;
                Tutorial.SetActive(false);
            }
        }
    }
}
