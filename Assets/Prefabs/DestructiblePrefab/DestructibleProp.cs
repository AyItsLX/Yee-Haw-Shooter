using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleProp : MonoBehaviour {

    public float propHealth = 100f;

    private Collider selfCollider;

    void Start()
    {
        selfCollider = GetComponent<Collider>();
    }

    void Update ()
    {
        propHealth = Mathf.Clamp(propHealth, 0f, 100f);

        if (Input.GetKeyDown(KeyCode.Y))
        {
            propHealth -= 20f;
        }

        if (propHealth < 1)
        {
            selfCollider.enabled = false;

            Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);

            foreach (Collider nearbyObject in colliders)
            {
                Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                RemoveProp setTrue = nearbyObject.GetComponent<RemoveProp>();

                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.AddExplosionForce(5f, transform.position, 5f);
                }

                if (setTrue != null)
                {
                    setTrue.onlyRunAfterDestroyed = true;
                }
            }

            Destroy(gameObject, 8f);
        }
	}

    public void PropDamage(float amount, Vector3 hitPoint)
    {
        propHealth -= amount;
    }
}
