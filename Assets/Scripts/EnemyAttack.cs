using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;
    public Animator anim;
    public AudioSource hitSound;
    public AIMover Mover;

    Ray cameraRay;
    RaycastHit cameraRayHit;
    GameObject player;
    GameObject obj;
    PlayerHealth playerHeatlh;
    EnemyHealth enemyHealth;
    public bool playerInRange;
    public float timer;


	void Awake ()
    {
        obj = GameObject.FindGameObjectWithTag("Obj");
        player = GameObject.FindGameObjectWithTag("Player");
        playerHeatlh = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
	}

    void Update()
    {
        timer += Time.deltaTime;

        if (playerHeatlh == null)
        {
            anim.SetBool("Dance", true);

            Destroy(GetComponent<AudioSource>());
        }

        if (timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
        {
            Attack();
            hitSound.Play();
        }
    }

    void Attack()
    {
        timer = 0f;

        if (playerHeatlh.currentHealth > 0)
        {
            playerHeatlh.TakeDamage(attackDamage, transform);
        }
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
	}

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
