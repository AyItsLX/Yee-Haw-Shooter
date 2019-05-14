using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiqourPuller : MonoBehaviour {

    public GameObject attractedTo;
    public float strengthOfAttraction = 10.0f;

    void Start () {
		
	}

    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);

        foreach (Collider liquor in colliders)
        {
            if (liquor.gameObject.name == "Gold(Clone)")
            {
                Rigidbody liquorRB = liquor.GetComponent<Rigidbody>();

                Vector3 direction = transform.position - liquor.gameObject.transform.position;

                if (liquorRB != null)
                {
                    liquorRB.AddForce(direction * strengthOfAttraction);
                }
            }
        }

        //if (gameObject.name == "Gold(Clone)")
        //{
        //    Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);

        //    foreach (Collider liquor in colliders)
        //    {
        //        print("Execute");
        //        Vector3 direction = attractedTo.transform.position - transform.position;
        //        gameObject.GetComponent<Rigidbody>().AddForce(strengthOfAttraction * direction);
        //    }
        //}
    }
}
