using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShop : MonoBehaviour {

    public GameObject TurretUIOpen;
    public GameObject TurretChoice;

    public GameObject[] Turrets;
    public GameObject[] TurretButtons;

    public GameObject NotEnoughUI;
    public CharController Player;
    
    public void OnTurretShopOpen()
    {
        TurretChoice.SetActive(true);
    }

    public void OnTurretShopClose()
    {
        TurretUIOpen.SetActive(true);
        TurretChoice.SetActive(false);
    }

    public void OnFirstTurret()
    {
        if (Player.PlayerLiquor >= 100)
        {
            Turrets[0].SetActive(true);
            TurretButtons[0].SetActive(false);

            Player.PlayerLiquor -= 100;
        }
        else
        {
            StartCoroutine(WaitForSecs());
        }
    }

    public void OnSecondTurret()
    {
        if (Player.PlayerLiquor >= 100)
        {
            Turrets[1].SetActive(true);
            TurretButtons[1].SetActive(false);

            Player.PlayerLiquor -= 100;
        }
        else
        {
            StartCoroutine(WaitForSecs());
        }
    }

    public void OnThirdTurret()
    {
        if (Player.PlayerLiquor >= 100)
        {
            Turrets[2].SetActive(true);
            TurretButtons[2].SetActive(false);

            Player.PlayerLiquor -= 100;
        }
        else
        {
            StartCoroutine(WaitForSecs());
        }
    }

    public void OnFourthTurret()
    {
        if (Player.PlayerLiquor >= 100)
        {
            Turrets[3].SetActive(true);
            TurretButtons[3].SetActive(false);

            Player.PlayerLiquor -= 100;
        }
        else
        {
            StartCoroutine(WaitForSecs());
        }
    }

    IEnumerator WaitForSecs()
    {
        NotEnoughUI.SetActive(true);
        yield return new WaitForSeconds(1f);
        NotEnoughUI.SetActive(false);
    }
}
