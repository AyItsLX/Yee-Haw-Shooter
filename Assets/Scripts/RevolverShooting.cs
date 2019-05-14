using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class RevolverShooting : MonoBehaviour {

    [Header("Revolver Stats")]
    public int damagePerShot = 100;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;
    public float BulletKnockBack = 25f;
    public float ShotsMaxCD = 0.4f;
    private float ShotsCDTimer = 0f;


    Ray cameraRay;
    RaycastHit cameraRayHit;
    float timer;
    Ray shootRay;
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource[] gunAudio;
    AudioSource gunshotAudio;
    AudioSource gunreloadAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;
    [Header("ETC")]
    public int currentBullets = 0;
    public int totalBullets = 30;
    public int remainingAmmo = 120;

    public bool isReloading;
    public float reloadingTimer;

    public Text RevAmmoText;
    public GameObject OutOfAmmoText;
    public GameObject IAMRELOADING;
    bool outtaAmmo = false;

    public Vector3 mousePositionVector3;
    public Vector3 targetdir;

    public static bool RevolverOn = false;

    public GameObject popUpDamage;

    void Awake()
    {
        #region False by Default
        OutOfAmmoText.SetActive(false);
        IAMRELOADING.SetActive(false);
        #endregion

        ShotsCDTimer = ShotsMaxCD;

        isReloading = false;
        currentBullets = totalBullets;
        shootableMask = LayerMask.GetMask("Shootable");
        gunLine = GetComponent<LineRenderer>();
        gunAudio = GetComponents<AudioSource>();
        gunshotAudio = gunAudio[0];
        gunreloadAudio = gunAudio[1];
        gunLight = GetComponent<Light>();
    }

    void Update()
    {
        damagePerShot = Random.Range(90, 100);

        #region Update Ammo UI / Clamps / Timer
        RevAmmoText.text = currentBullets + " / " + remainingAmmo;
        currentBullets = Mathf.Clamp(currentBullets, 0, 30);
        remainingAmmo = Mathf.Clamp(remainingAmmo, 0, 120);
        timer += Time.deltaTime;
        #endregion

        #region Shoot Button
        ShotsCDTimer -= Time.deltaTime;
        if (ShotsCDTimer <= 0)
        {
            if (Input.GetButton("Fire1") && timer >= timeBetweenBullets && !isReloading && !outtaAmmo)
            {
                if (currentBullets > 0)
                {
                    Shoot();
                }
            }
            if (Input.GetButton("Fire1") && (isReloading || !isReloading) && remainingAmmo == 0 && currentBullets == 0)
            {
                isReloading = false;
                OutOfAmmoText.SetActive(true);
                IAMRELOADING.SetActive(false);

                if (!outtaAmmo)
                    outtaAmmo = true;
            }
            else
            {
                outtaAmmo = false;
                OutOfAmmoText.SetActive(false);
            }
            ShotsCDTimer = ShotsMaxCD;
        }
        #endregion

        #region Reload Button
        if (Input.GetKeyDown(KeyCode.R) && remainingAmmo > 0)
        {
            isReloading = true;
        }
        else if (Input.GetButton("Fire1") && !isReloading && currentBullets == 0 && !outtaAmmo)
        {
            isReloading = true;
        }
        else if (Input.GetButton("Fire1") && isReloading && currentBullets > 29 && !outtaAmmo)
        {
            isReloading = false;
            IAMRELOADING.SetActive(false);
        }
        #endregion

        #region Check if Reloading
        if (isReloading)
        {
            IAMRELOADING.SetActive(true);
            reloadingTimer += Time.deltaTime;
            if (reloadingTimer > .05f)
            {
                if (currentBullets < 30 && remainingAmmo > 0)
                {
                    currentBullets += 1;
                    remainingAmmo -= 1;
                    gunreloadAudio.Play();
                }
                if (currentBullets == 30 || remainingAmmo == 0)
                {
                    IAMRELOADING.SetActive(false);
                    isReloading = false;
                }
                reloadingTimer = 0f;
            }
        }
        #endregion

        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects();
        }

        cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        //if (Physics.Raycast(cameraRay, out cameraRayHit))
        //{
        //    if (cameraRayHit.transform.tag == "Enemy" || cameraRayHit.transform.tag == "Shootable" || cameraRayHit.transform.tag == "Ground" || cameraRayHit.transform.tag == "Untagged" || cameraRayHit.transform.tag == "Obj" || cameraRayHit.transform.tag == "MainCamera")
        //    {
        //        Vector3 targetPosition = new Vector3(cameraRayHit.point.x, transform.position.y, cameraRayHit.point.z);
        //        transform.LookAt(targetPosition);
        //    }
        //}
    }

    public void DisableEffects()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }

    void Shoot()
    {
        currentBullets -= 1;
        timer = 0f;

        gunshotAudio.Play();

        gunLight.enabled = true;

        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        //if (Physics.Raycast(cameraRay, out cameraRayHit))
        //{
        //    if (cameraRayHit.transform.tag == "Enemy" || cameraRayHit.transform.tag == "Shootable" || cameraRayHit.transform.tag == "Ground" || cameraRayHit.transform.tag == "Untagged" || cameraRayHit.transform.tag == "Obj" || cameraRayHit.transform.tag == "MainCamera")
        //    {
        //        Vector3 targetPosition = new Vector3(cameraRayHit.point.x, transform.position.y, cameraRayHit.point.z);
        //        transform.LookAt(targetPosition);
        //    }
        //}

        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            if (shootHit.collider.gameObject.name == "Crate")
            {
                DestructibleProp destroyProp = shootHit.collider.GetComponent<DestructibleProp>();

                if (destroyProp != null)
                {
                    destroyProp.PropDamage(20f, shootHit.point);
                }
            }
            else
            {
                EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();

                if (enemyHealth != null)
                {
                    GameObject obj = Instantiate(popUpDamage, shootHit.collider.gameObject.transform.position + new Vector3(0,5,0), Quaternion.identity, shootHit.collider.gameObject.transform);
                    obj.GetComponent<TextMesh>().text = damagePerShot.ToString();

                    enemyHealth.TakeDamage(damagePerShot, shootHit.point, BulletKnockBack);
                }
            }

            gunLine.SetPosition(1, shootHit.point);
        }

        else
        {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }
}

