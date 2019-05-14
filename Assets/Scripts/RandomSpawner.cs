using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomSpawner : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public GameObject Bandit;
    public Transform[] spawnPoints;
    public int spawnPointIndex;
    public bool WaveStart, Wave1 = false, Wave2 = false, Wave3 = false;
    public int Wave1Length, Wave2Length, Wave3Length;
    public GameObject Player;
    public GameObject WinScreenObj;

    private float secondsBetweenSpawn = 0.5f;
    private float elapsedTime = 0.0f;
    private int wave1Counter = 0;
    private int wave2Counter = 0;
    private int wave3Counter = 0;

    RevolverShooting playerShooting;
    public GameObject WaveCounterObj;
    public GameObject[] WaveText;
    public GameObject WaveNumObj;
    public Slider WaveTimer;
    public Text WaveTimerText;
    public Slider WaveNumSlider;
    public int curAmountOfEnemy = 0;
    public float Timer;
    private bool TimerBool = false;
    bool OnlyTriggerAfterStart = false;

    #region Awake & Start
    void Awake()
    {
        WaveText[0].SetActive(false);
        WaveText[1].SetActive(false);
        WaveText[2].SetActive(false);
        secondsBetweenSpawn = 1f;
        Wave1Length = 50;
        Wave2Length = 100;
        Wave3Length = 200;
    }
    #endregion

    void Update()
    {
        WaveNumSlider.value = Mathf.Lerp(WaveNumSlider.value, curAmountOfEnemy, Time.deltaTime * 5f);
        WaveTimer.value = Mathf.Lerp(WaveTimer.value, Timer, Time.deltaTime * 5f);
        WaveTimerText.text = "Time Left : " + (int)Timer;

        if (TimerBool)
        {
            Timer -= Time.deltaTime;
            if (Timer <= 0)
            {
                TimerBool = false;
            }
        }

        if (Player == null)
        {
            Destroy(gameObject);
        }

        spawnPointIndex = Random.Range(0, 4);

        if (WaveStart)
        {
            WaveCounterObj.SetActive(true);
            WaveNumObj.SetActive(true);
            WaveStart = false;

            StartCoroutine(Waves());
        }

        if (Wave1)
        {
            WaveText[0].SetActive(true);
            if (wave1Counter < Wave1Length)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= secondsBetweenSpawn)
                {
                    Instantiate(Bandit, spawnPoints[spawnPointIndex].position, Quaternion.identity);
                    wave1Counter += 1;
                    elapsedTime = 0f;
                }
            }
            else
            {
                Wave1 = false;
            }
        }
        if (Wave2)
        {
            WaveText[1].SetActive(true);
            if (wave2Counter < Wave2Length)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= secondsBetweenSpawn)
                {
                    Instantiate(Bandit, spawnPoints[spawnPointIndex].position, Quaternion.identity);
                    wave2Counter += 1;
                    elapsedTime = 0f;
                }
            }
            else
            {
                Wave2 = false;
            }
        }
        if (Wave3)
        {
            WaveText[2].SetActive(true);
            if (wave3Counter < Wave3Length)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= secondsBetweenSpawn)
                {
                    Instantiate(Bandit, spawnPoints[spawnPointIndex].position, Quaternion.identity);
                    wave3Counter += 1;
                    elapsedTime = 0f;
                }
            }
            else
            {
                WaveText[2].SetActive(false);
                Wave3 = false;
            }
        }
        if (curAmountOfEnemy == 0 && OnlyTriggerAfterStart)
        {
            EnemiesDeath();
        }
    }

    IEnumerator Waves()
    {
        Timer = 60f;
        TimerBool = true;
        curAmountOfEnemy += Wave1Length;
        WaveNumSlider.maxValue = Wave1Length;

        Wave1 = true;
        yield return new WaitForSeconds(60f);

        Timer = 60f;
        TimerBool = true;
        WaveText[0].SetActive(false);
        WaveNumSlider.maxValue = Wave2Length + curAmountOfEnemy;
        curAmountOfEnemy += Wave2Length;

        Wave2 = true;
        yield return new WaitForSeconds(60f);

        Timer = 60f;
        TimerBool = true;
        WaveText[1].SetActive(false);
        WaveNumSlider.maxValue = Wave3Length + curAmountOfEnemy;
        curAmountOfEnemy += Wave3Length + curAmountOfEnemy;
        OnlyTriggerAfterStart = true;

        Wave3 = true;
    }

    void EnemiesDeath()
    {
            WinScreenObj.SetActive(true);
    }
}
