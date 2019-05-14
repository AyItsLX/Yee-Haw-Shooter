using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour {

    public GameObject Parent;

    public void DestroyOwn()
    {
        Destroy(Parent);
    }
}
