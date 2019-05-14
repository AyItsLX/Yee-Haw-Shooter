using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float startingHealth = 100;
    public float currentHealth;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public AudioClip deathClip;
    public Slider healthBar;
    public GameObject healthBarGameObject;
    public float Force = 5;
    public GameObject GoldPrefab;
    public int GoldAmount = 0;
    public int posRandomizer;
    public GameObject[] GoldSpawnPos;
    bool runOnce = false;

    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    bool isDead;

    public GameObject Player;
    Vector3 temp;
    bool run1 = false;
    public RandomSpawner RNGSpawner;
    bool TurnOffHealthSlider = false;
    float SliderTimer = 5f;
    public GameObject DestroyDuckies;
    [Header("Animation")]
    public Animator EnemyAnimator;
    public int RandomDeath = 0;

    void Awake ()
    {
        RNGSpawner = GameObject.Find("SpawnManager").GetComponent<RandomSpawner>();
        healthBarGameObject.SetActive(false);

        GoldAmount = Random.Range(3, 7);

        enemyAudio = GetComponent<AudioSource>();
        hitParticles = GetComponentInChildren<ParticleSystem>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        currentHealth = startingHealth;
        healthBar.value = currentHealth;

        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        RandomDeath = Random.Range(1, 4);
    }

    void Update ()
    {
        healthBar.value = Mathf.Lerp(healthBar.value, currentHealth, Time.deltaTime * 5f);

        if (Input.GetKeyDown(KeyCode.N))
        {
            GameObject.Find("FatJoseph").GetComponent<EnemyHealth>().currentHealth -= 10000f;
        }

        if (TurnOffHealthSlider)
        {
            SliderTimer -= Time.deltaTime;
            if (SliderTimer < 0)
            {
                healthBarGameObject.SetActive(false);
                TurnOffHealthSlider = false;
                SliderTimer = 5f;
            }
        }

        if (currentHealth < 1)
        {
            while (GoldAmount > 0)
            {
                posRandomizer = Random.Range(0, 4);
                Instantiate(GoldPrefab, GoldSpawnPos[posRandomizer].transform.position, Quaternion.identity);
                GoldAmount -= 1;
            }
            if (!runOnce)
            {
                runOnce = false;

                Destroy(DestroyDuckies);

                if (gameObject.name == "Gold(Clone)")
                {
                    Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);

                    foreach (Collider nearbyObject in colliders)
                    {
                        Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            rb.AddExplosionForce(Force, transform.position, 5f);
                        }
                    }
                }
            }

            #region Death Randomizer
            if (RandomDeath == 1)
            {
                EnemyAnimator.SetBool("Die1", true);
            }
            else if (RandomDeath == 2)
            {
                EnemyAnimator.SetBool("Die2", true);
            }
            else if (RandomDeath == 3)
            {
                EnemyAnimator.SetBool("Die3", true);
            }
            #endregion

            if (!run1)
            {
                run1 = true;
                GameManager.Score += 1;
                RNGSpawner.curAmountOfEnemy -= 1;
                FindObjectOfType<MusicManager>().Play("BanditDeath");
            }

            Destroy(GetComponent<BoxCollider>());
            Destroy(GetComponent<SphereCollider>());
            Destroy(GetComponent<CapsuleCollider>());
            Destroy(GetComponent<Rigidbody>());
            Destroy(GetComponent<NavMeshAgent>());
            Destroy(healthBarGameObject);
        }

        //if (isSinking)
        //{
        //    transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
        //}
    }

    #region Revolver Damage
    public void TakeDamage(int amount, Vector3 hitPoint, float knockBack)
    {
        healthBarGameObject.SetActive(true);
        TurnOffHealthSlider = true;
        Rigidbody RB = GetComponent<Rigidbody>();
        if (isDead)
            return;

        enemyAudio.Play();

        currentHealth -= amount;

        temp = new Vector3(Player.transform.forward.x, 0, Player.transform.forward.z);
        temp.Normalize();
        RB.AddForce(temp * knockBack, ForceMode.Impulse);

        //hitParticles.transform.position = hitPoint;

        //hitParticles.Play();

        //if (currentHealth <= 0)
        //{
        //    Death();
        //}
    }
    #endregion

    #region Shotgun Damage
    public void TakeSGDamage(int amount, Vector3 hitPoint, float knockBack)
    {
        healthBarGameObject.SetActive(true);
        TurnOffHealthSlider = true;
        Rigidbody RB = GetComponent<Rigidbody>();
        if (isDead)
            return;

        enemyAudio.Play();

        currentHealth -= amount;

        temp = new Vector3(Player.transform.forward.x, 0, Player.transform.forward.z);
        temp.Normalize();
        RB.AddForce(temp * knockBack, ForceMode.Impulse);

        //hitParticles.transform.position = hitPoint;

        //hitParticles.Play();

        //if (currentHealth <= 0)
        //{
        //    Death();
        //}
    }
    #endregion

    //void Death()
    //{
    //    isDead = true;

    //    capsuleCollider.isTrigger = true;

    //    GetComponent<NavMeshAgent>().enabled = false;
    //    GetComponent<Rigidbody>().isKinematic = true;
    //    //isSinking = true;
    //    Destroy(gameObject, 5f);

    //    //ScoreManager.score += scoreValue;

    //    //anim.SetTrigger("Dead");

    //    //enemyAudio.clip = deathClip;
    //    //enemyAudio.Play();
    //}
}

