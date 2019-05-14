using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveProp : MonoBehaviour {

    public float removeTimer = 5f;

    private Collider selfCollider;

    public bool onlyRunAfterDestroyed = false;

	void Start ()
    {
        selfCollider = GetComponent<Collider>();
	}
	
	void Update ()
    {
        if (onlyRunAfterDestroyed)
        {
            removeTimer -= Time.deltaTime;

            if (removeTimer < 0)
            {
                selfCollider.enabled = false;
            }

            Destroy(gameObject, 7f);
        }
	}
}
