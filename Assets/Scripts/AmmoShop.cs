using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoShop : MonoBehaviour {

    public CharController charLiquor;
    public RevolverShooting Rev;
    public ShotgunShooting SG1;
    public ShotgunShooting SG2;

    public GameObject NotEnoughText;

    public AudioSource KaChing;

    void Awake()
    {
        NotEnoughText.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            OnRevAmmoPurchased();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            OnSGAmmoPurchased();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            OnDynaAmmoPurchased();
        }

    }

    public void OnDynaAmmoPurchased()
    {
        if (charLiquor.PlayerLiquor > 10f)
        {
            KaChing.Play();
            charLiquor.currentDynamite += 1;
            charLiquor.PlayerLiquor -= 10f;
        }
        else if (charLiquor.PlayerLiquor < 5f)
        {
            StartCoroutine(WaitFor2Sec());
        }
    }

    public void OnRevAmmoPurchased()
    {
        if (charLiquor.PlayerLiquor > 5f)
        {
            KaChing.Play();
            Rev.remainingAmmo += 30;
            charLiquor.PlayerLiquor -= 5f;
        }
        else if (charLiquor.PlayerLiquor < 5f)
        {
            StartCoroutine(WaitFor2Sec());
        }
    }

    public void OnSGAmmoPurchased()
    {
        if (charLiquor.PlayerLiquor > 5f)
        {
            KaChing.Play();
            SG1.remainingSGAmmo += 30;
            SG2.remainingSGAmmo += 30;
            charLiquor.PlayerLiquor -= 5f;
        }
        else if (charLiquor.PlayerLiquor < 5f)
        {
            StartCoroutine(WaitFor2Sec());
        }
    }

    IEnumerator WaitFor2Sec()
    {
        NotEnoughText.SetActive(true);
        yield return new WaitForSeconds(1f);
        NotEnoughText.SetActive(false);
    }
}
