using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMover : MonoBehaviour {

    public List<GameObject> EnemiesInRange = new List<GameObject>();
    public GameObject CurrentTarget;
    public float MaxDist = 5f;

    public Transform AmmoSpawnPoint;
    public GameObject AmmoPrefab;

    public float Maxtimer = 0.5f;
    public float timer = 0f;
    public bool isCooldown = false;
    public bool foundTarget = false;

    void Start ()
    {
        timer = Maxtimer;
    }
	
	void Update ()
    {
        if (CurrentTarget == !isActiveAndEnabled)
        {
            CurrentTarget = null;
        }

        for (int i = 0; i < EnemiesInRange.Count; i++)
        {
            if (EnemiesInRange[i] != null)
            {
                float Dist = Vector3.Distance(EnemiesInRange[i].transform.position, transform.position); // Enemy Distance to Turret Distance

                if (Dist < MaxDist) // Changes target based on closet distance
                {
                    CurrentTarget = EnemiesInRange[i];

                    Dist = MaxDist;

                    foundTarget = true;

                }
            }

            if (EnemiesInRange[i] == null)
            {
                EnemiesInRange.Remove(EnemiesInRange[i]);
            }
        }

        if (foundTarget)
        {
            Vector3 targetLook = new Vector3(CurrentTarget.transform.position.x, 1.07f, CurrentTarget.transform.position.z);

            transform.LookAt(targetLook);

            timer -= Time.deltaTime;

            if (timer < 0 && CurrentTarget != null)
            {
                isCooldown = true;
                timer = Maxtimer;
            }

            if (isCooldown)
            {
                Instantiate(AmmoPrefab, AmmoSpawnPoint.position, Quaternion.identity);
                isCooldown = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemiesInRange.Add(other.gameObject);
        }
    }
}
