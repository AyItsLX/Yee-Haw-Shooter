using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dynamite : MonoBehaviour {

    public float delay = 3f;
    public float radius = 5f;
    public float force = 700f;
    public float dynaDamage = 50;
    public GameObject explosionEffect;
    public AudioSource FuseSound;
    public AudioSource ExplodeSound;
    public AudioClip[] explosionSound;

    float countdown;
    bool hasExploded = false;
    float timer;

	void Start ()
    {
        ExplodeSound.clip = explosionSound[Random.Range(0, 4)];
        countdown = delay;
	}
	
	void Update ()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
	}

    void Explode()
    {
        Destroy(FuseSound);
        ExplodeSound.Play();

        Instantiate(explosionEffect, transform.position, transform.rotation);

        Collider [] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            EnemyHealth banditHealth = nearbyObject.GetComponent<EnemyHealth>();
            AIMover AIs = nearbyObject.GetComponent<AIMover>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);
            }
            if (banditHealth != null)
            {
                banditHealth.currentHealth -= dynaDamage;
            }
            if (AIs != null)
            {
                AIs.isStunned = true;
                FindObjectOfType<MusicManager>().Play("Confused");
            }
        }

        Destroy(GetComponent<MeshRenderer>());
        Destroy(gameObject.transform.parent.gameObject, ExplodeSound.clip.length);
        
    }
}
