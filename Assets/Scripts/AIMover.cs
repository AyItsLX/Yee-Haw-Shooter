using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMover : MonoBehaviour {

    public NavMeshAgent agent;
    public Transform Player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;

    public bool isStunned = false;
    public float StunTimer = 3f;
    public GameObject BirdiePrefab;

    void Awake () {
        agent = GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = Player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        BirdiePrefab.SetActive(false);

    }
	
	void Update ()
    {
        if (isStunned)
        {
            BirdiePrefab.SetActive(true);
            StunTimer -= Time.deltaTime;
            if (StunTimer < 0)
            {
                FindObjectOfType<MusicManager>().Stop("Confused");
                BirdiePrefab.SetActive(false);
                isStunned = false;
                agent.enabled = true;
                StunTimer = 3f;
            }
        }

        if (playerHealth.currentHealth > 0 && !isStunned)
        {
            if (agent == isActiveAndEnabled)
            {
                if (agent.isActiveAndEnabled)
                {
                    agent.SetDestination(Player.position);
                }
            }
        }
        else
        {
            agent.enabled = false;
        }
    }
}
