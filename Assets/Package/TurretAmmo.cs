using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAmmo : MonoBehaviour {

    public TurretMover turretMover;
    public Transform Target;

    public int Damage = 1;

    void Start()
    {
        turretMover = GameObject.FindGameObjectWithTag("Turret").GetComponent<TurretMover>();
        Target = turretMover.CurrentTarget.transform;

        Destroy(gameObject, 2f);
    }

    void Update()
    {
        if (Target == null)
        {
            Destroy(gameObject);
        }
        Vector3 a = transform.position - Target.position;
        a.Normalize();
        transform.position = transform.position - a * Time.deltaTime * 10f;

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyHealth>().currentHealth -= Damage;
            Destroy(gameObject);
        }
    }
}
