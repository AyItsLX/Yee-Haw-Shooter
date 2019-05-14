using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public float smoothing = 5f;

    Vector3 offset;

    void Start()
    {
        offset = transform.position - target.position;
    }

    private void FixedUpdate()
    {
        if (target == isActiveAndEnabled)
        {
            Vector3 CameraPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, CameraPosition, Time.deltaTime * smoothing);
        }
    }
}
