using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharController : MonoBehaviour
{
    [Header("Player Stats")]
    public float PlayerLiquor = 0f;
    public Text PlayerLiquorText;
    public Slider PlayerLiquorSlider;
    public Text SliderText;
    public float PlayerUpdatedLiquor = 0f;
    public float moveSpeed = 10f;
    public string FireAxis = "Fire1";
    public float ReloadDelay = 0.3f;
    public bool CanFire = true;
    public Transform[] RevolverTransforms;
    [SerializeField]
    Rigidbody playerRigidbody;
    RevolverShooting shooting;
    Ray cameraRay;
    RaycastHit cameraRayHit;
    public GameObject revolver;
    public GameObject shotgun;
    public GameObject SGEnd;
    public GameObject RevolverEnd;
    public GameObject SpawnDynaPos;
    public GameObject dynamitePrefab;
    Vector3 forward, right;
    [Header("Animations")]
    public Animator anim;
    private float H;
    private float V;
    int shootableMask;
    public int totalDynamite = 5;
    public int currentDynamite = 0;
    public int remainingDynamite = 5;
    public Text dynaAmmo;
    Vector3 movement;
    public AudioSource walkSound;

    #region Awake & Start
    void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable");
        revolver.SetActive(true);
        shotgun.SetActive(false);
        RevolverEnd.SetActive(true);
        SGEnd.SetActive(false);
        GetComponentInChildren<RevolverShooting>();
        playerRigidbody = GetComponent<Rigidbody>();
        revolver.SetActive(true);
        shotgun.SetActive(false);
        currentDynamite = totalDynamite;
    }

    void Start()
    {
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;

    }
    #endregion

    void Update()
    {
        #region Play Walking Sound
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if (!walkSound.isPlaying)
            {
                walkSound.Play();
            }
        }
        else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            if (walkSound.isPlaying)
            {
                walkSound.Stop();
            }
        }
        #endregion

        #region Update Liquor Text / Slider
        PlayerLiquor = Mathf.Clamp(PlayerLiquor, 0f, 100f);
        PlayerLiquorText.text = ": Litre";
        PlayerLiquorSlider.value = Mathf.Lerp(PlayerLiquorSlider.value, PlayerLiquor, Time.deltaTime * 5f);
        PlayerUpdatedLiquor = Mathf.Lerp(PlayerUpdatedLiquor, PlayerLiquor, Time.deltaTime * 5f);
        SliderText.text = PlayerUpdatedLiquor.ToString("0.0");
        currentDynamite = Mathf.Clamp(currentDynamite, 0, 5);
        remainingDynamite = Mathf.Clamp(remainingDynamite, 0, 5);
        dynaAmmo.text = currentDynamite + " / " + remainingDynamite;
        #endregion

        #region Animations
        H = Input.GetAxis("Horizontal");
        V = Input.GetAxis("Vertical");
        anim.SetFloat("H", H);
        anim.SetFloat("V", V);
        #endregion

        #region Swap Between Revolver or Shotgun
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            revolver.SetActive(true);
            shotgun.SetActive(false);
            RevolverEnd.SetActive(true);
            SGEnd.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            revolver.SetActive(false);
            shotgun.SetActive(true);
            RevolverEnd.SetActive(false);
            SGEnd.SetActive(true);
        }
        #endregion

        #region Instantiate Dynamite
        if (Input.GetKeyDown(KeyCode.G) && currentDynamite > 0)
        {
            currentDynamite -= 1;

            Instantiate(dynamitePrefab, SpawnDynaPos.transform.position, SpawnDynaPos.transform.rotation);
        }
        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    Instantiate(dynamitePrefab, SpawnDynaPos.transform.position, SpawnDynaPos.transform.rotation);
        //}
        #endregion

        #region Rotate when not Moving
        cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(cameraRay, out cameraRayHit))
        {
            if (cameraRayHit.transform.tag == "Enemy" || cameraRayHit.transform.tag == "Shootable" || cameraRayHit.transform.tag == "Ground" || cameraRayHit.transform.tag == "Untagged" || cameraRayHit.transform.tag == "Obj" || cameraRayHit.transform.tag == "MainCamera" || cameraRayHit.transform.tag == "Turret")
            {
                Vector3 targetPosition = new Vector3(cameraRayHit.point.x, transform.position.y, cameraRayHit.point.z);
                transform.LookAt(targetPosition);
            }
        }
        #endregion

        #region Commented
        //if (Physics.Raycast(cameraRay, out cameraRayHit))
        //{
        //    if (cameraRayHit.transform.tag == "Enemy" || cameraRayHit.transform.tag == "Shootable" || cameraRayHit.transform.tag == "Ground" || cameraRayHit.transform.tag == "Untagged" || cameraRayHit.transform.tag == "Obj" || cameraRayHit.transform.tag == "MainCamera")
        //    {
        //        Vector3 targetPosition = new Vector3(cameraRayHit.point.x, 1, cameraRayHit.point.z);
        //        transform.LookAt(targetPosition);
        //    }
        //}
        #endregion

        Move();
    }

    #region Move Method & Rotate while Moving
    void Move()
    {
        Vector3 direction = new Vector3(Input.GetAxis("HorizontalKey"), 0, Input.GetAxis("VerticalKey"));
        Vector3 rightMovement = right * moveSpeed * Time.deltaTime * Input.GetAxis("HorizontalKey");
        Vector3 upMovement = forward * moveSpeed * Time.deltaTime * Input.GetAxis("VerticalKey");

        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

        if (heading != Vector3.zero)
        {
            transform.forward = heading;
            transform.position += rightMovement;
            transform.position += upMovement;

            playerRigidbody.MovePosition(transform.position + direction);
        }

        cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(cameraRay, out cameraRayHit))
        {
            if (cameraRayHit.transform.tag == "Enemy" || cameraRayHit.transform.tag == "Shootable" || cameraRayHit.transform.tag == "Ground" || cameraRayHit.transform.tag == "Untagged" || cameraRayHit.transform.tag == "Obj" || cameraRayHit.transform.tag == "MainCamera" || cameraRayHit.transform.tag == "Turret")
            {
                Vector3 targetPosition = new Vector3(cameraRayHit.point.x, transform.position.y, cameraRayHit.point.z);
                transform.LookAt(targetPosition);
            }
        }
    }
    #endregion
}
