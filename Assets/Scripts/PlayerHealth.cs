using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    public Slider HealthUI;
    public GameObject YeeHaw;

    public GameObject OnHitFadePrefab;
    public Image HealthOpacity;
    public GameObject SpawnTextHere;
    Animator anim;
    Rigidbody rb;
    bool isHit = false;
    public Transform hitDirection;
    RevolverShooting playerShooting;
    bool isDead;
    bool damaged;
    public GameObject FloatingText;
    public GameObject EndScreenObj;
  

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = startingHealth;
    }

    void Update()
    {
        HealthUI.value = Mathf.Lerp(HealthUI.value, currentHealth, Time.deltaTime * 5f);

        #region Health UI Color
        if (currentHealth < 80 && currentHealth > 60)
        {
            HealthOpacity.color = new Color(HealthOpacity.color.r, HealthOpacity.color.g, HealthOpacity.color.b, 0.25f);
        }
        else if (currentHealth < 60 && currentHealth > 40)
        {
            HealthOpacity.color = new Color(HealthOpacity.color.r, HealthOpacity.color.g, HealthOpacity.color.b, 0.5f);
        }
        else if (currentHealth < 40 && currentHealth > 20)
        {
            HealthOpacity.color = new Color(HealthOpacity.color.r, HealthOpacity.color.g, HealthOpacity.color.b, 0.75f);
        }
        else if (currentHealth < 20 && currentHealth < 0)
        {
            HealthOpacity.color = new Color(HealthOpacity.color.r, HealthOpacity.color.g, HealthOpacity.color.b, 1f);
        }
        #endregion

        #region Death Boolean
        if (currentHealth <= 0 && !isDead)
        {
            YeeHaw.SetActive(true);

            Death();
        }
        #endregion

        #region IsHit Boolean
        if (isHit)
        {
            isHit = false;
            OnHitFadePrefab.SetActive(true);
            rb.AddExplosionForce(2500f, hitDirection.position, 5f);
        }
        #endregion

        if (Input.GetKeyDown(KeyCode.O))
        {
            Vector3 Temp = new Vector3(-SpawnTextHere.transform.position.x, SpawnTextHere.transform.position.y, SpawnTextHere.transform.position.z);
            var go = Instantiate(FloatingText, Temp, Quaternion.identity, transform);
            go.GetComponent<TextMesh>().text = currentHealth.ToString();
        }
    }

    public void TakeDamage (int amount, Transform target)
    {
        hitDirection = target;

        FindObjectOfType<MusicManager>().Play("PlayerHurt");

        damaged = true;
        currentHealth -= amount;

        isHit = true;

        Vector3 Temp = new Vector3(-SpawnTextHere.transform.position.x, SpawnTextHere.transform.position.y, SpawnTextHere.transform.position.z);
        var go = Instantiate(FloatingText, Temp, Quaternion.identity, transform);
        go.GetComponent<TextMesh>().text = currentHealth.ToString();
    }

    void Death ()
    {
        EndScreenObj.SetActive(true);

        FindObjectOfType<MusicManager>().Play("PlayerDeath");

        isDead = true;

        if (playerShooting != null)
        {
            playerShooting.DisableEffects();
            playerShooting.enabled = false;
        }

        HealthUI.value = 0;

        Destroy(gameObject);
    }
}
