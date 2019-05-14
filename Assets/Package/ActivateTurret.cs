using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTurret : MonoBehaviour
{
    public GameObject Turret;

	void Update ()
    {
        Turret.SetActive(false);
	}
}
