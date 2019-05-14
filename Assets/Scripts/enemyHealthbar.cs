using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHealthbar : MonoBehaviour {

    public GameObject Cam;

    //public RectTransform healthBarRect;

    //private Quaternion rotation;
    //private Vector3 position;

	void Awake ()
    {
        Cam = GameObject.FindGameObjectWithTag("MainCamera");
        transform.LookAt(Cam.transform.position);
        //rotation = transform.rotation;
        //position = transform.parent.position - transform.position;
    }
	
	void Update ()
    {
        transform.LookAt(Cam.transform.position);
        //transform.rotation = rotation;
        //transform.position = transform.parent.position - position;
	}

    public void SetHealth(float _cur, float _max)
    {
        //float value = _cur / _max;

        //healthBarRect.localScale = new Vector3(value, healthBarRect.localScale.y, healthBarRect.localScale.z);
    }
}
