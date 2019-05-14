using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldScript : MonoBehaviour {

    public float GoldAmount = 0.5f;

    public AudioSource PickUpSound;

    public GameObject BeerPrefab;

    public Collider triggerCollider;

	void Awake ()
    {
        Destroy(gameObject, 30f);
    }

    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PickUpSound.Play();
            Destroy(triggerCollider);
            Destroy(BeerPrefab);

            other.gameObject.GetComponent<PlayerController>().PlayerLiquor += GoldAmount;
            GameManager.LiquorCollected += GoldAmount;

            Destroy(transform.parent.gameObject, 1f);
        }
    }
}
