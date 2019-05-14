using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotgunShooting : MonoBehaviour {

    [Header("Shotgun Stats")]
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;
    public float BulletKnockBack = 50f;
    public float ShotsMaxCD = 1f;
    private float ShotsCDTimer = 0f;


    float timer;
    Ray shootRay;
    RaycastHit shootHit;
    int shootableMask;
    LineRenderer gunLine;
    AudioSource[] gunAudio;
    AudioSource gunshotAudio;
    AudioSource gunreloadAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;
    [Header("ETC")]
    public int currentSGBullets = 0;
    public int totalSGBullets = 15;
    public int remainingSGAmmo = 120;

    public bool isReloading;
    public float reloadingTimer;

    public Text SGAmmoText;
    public GameObject OutOfAmmoText;
    public GameObject IAMRELOADING;
    bool outtaAmmo = false;

    public Vector3 mousePositionVector3;
    public Vector3 targetdir;

    public static bool ShotgunOn = false;

    void Awake()
    {
        #region False by Default
        OutOfAmmoText.SetActive(false);
        IAMRELOADING.SetActive(false);
        #endregion

        ShotsCDTimer = ShotsMaxCD;

        isReloading = false;
        currentSGBullets = totalSGBullets;
        shootableMask = LayerMask.GetMask("Shootable");
        gunLine = GetComponent<LineRenderer>();
        gunAudio = GetComponents<AudioSource>();
        gunshotAudio = gunAudio[0];
        gunreloadAudio = gunAudio[1];
        gunLight = GetComponent<Light>();
    }

    void Update()
    {
        #region Update Ammo UI / Clamps / Timer
        SGAmmoText.text = currentSGBullets + " / " + remainingSGAmmo;
        currentSGBullets = Mathf.Clamp(currentSGBullets, 0, 15);
        remainingSGAmmo = Mathf.Clamp(remainingSGAmmo, 0, 120);
        timer += Time.deltaTime;
        #endregion

        #region Shoot Button
        ShotsCDTimer -= Time.deltaTime;
        if (ShotsCDTimer <= 0)
        {
            if (Input.GetButton("Fire1") && timer >= timeBetweenBullets && !isReloading)
            {
                if (currentSGBullets > 0)
                {
                    Shoot();
                }
            }
            if (Input.GetButton("Fire1") && (isReloading || !isReloading) && remainingSGAmmo == 0 && currentSGBullets == 0)
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
        if (Input.GetKeyDown(KeyCode.R) && remainingSGAmmo > 0)
        {
            isReloading = true;
        }
        else if (Input.GetButton("Fire1") && !isReloading && currentSGBullets == 0 && !outtaAmmo)
        {
            isReloading = true;
        }
        else if (Input.GetButton("Fire1") && isReloading && currentSGBullets > 0 && !outtaAmmo)
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
            if (reloadingTimer > .5f)
            {
                if (currentSGBullets < 15 && remainingSGAmmo > 0)
                {
                    currentSGBullets += 1;
                    remainingSGAmmo -= 1;
                    gunreloadAudio.Play();
                }
                if (currentSGBullets == 15 || remainingSGAmmo == 0)
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
    }

    public void DisableEffects()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }

    void Shoot()
    {
        currentSGBullets -= 1;
        timer = 0f;

        gunshotAudio.Play();

        gunLight.enabled = true;

        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeSGDamage(damagePerShot, shootHit.point, BulletKnockBack);
            }


            gunLine.SetPosition(1, shootHit.point);
        }

        else
        {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }
}
