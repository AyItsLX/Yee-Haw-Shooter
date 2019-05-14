using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextScript : MonoBehaviour {

    public GameObject Camera;

    Vector3 offset = new Vector3(0, 2, 0);
    Vector3 RandomizeIntensity = new Vector3(0.5f, 0, 0);

	void Awake ()
    {
        Destroy(gameObject, 1f);

        Camera = GameObject.FindGameObjectWithTag("MainCamera");

        transform.localPosition += offset;
        transform.localPosition += new Vector3(Random.Range(-RandomizeIntensity.x, RandomizeIntensity.x),
                                                                    Random.Range(-RandomizeIntensity.y, RandomizeIntensity.y),
                                                                    Random.Range(-RandomizeIntensity.z, RandomizeIntensity.z));

        //transform.localScale = new Vector3(transform.localScale.x,
        //                                                transform.localScale.y,
        //                                                transform.localScale.z);
    }

	void Update ()
    {
        if (transform.localScale.x > 0 || transform.localScale.y > 0)
        {
            transform.localScale -= new Vector3(Time.deltaTime * 0.5f, Time.deltaTime * 0.5f, 0);
        }

        if (transform.localScale.x < 0 || transform.localScale.y < 0)
        {
            Destroy(gameObject);
        }

        transform.LookAt(-Camera.transform.position);
	}
}
