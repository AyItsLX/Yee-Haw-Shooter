using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    [Header("Player Stats")]
    public float player_Movement_Speed = 10f;
    public float speed = 5f;
    [Header("Weapons")]
    public GameObject revolver;
    public GameObject shotgun;
    public GameObject SGEnd;
    public GameObject RevolverEnd;
    public GameObject SpawnDynaPos;
    public GameObject dynamitePrefab;
    [Header("Animators")]
    public Animator anim;
    private float H;
    private float V;
    [Header("Ammo Count")]
    public int totalDynamite = 5;
    public int currentDynamite = 0;
    public int remainingDynamite = 5;
    public Text dynaAmmo;
    public AudioSource walkSound;

    public float PlayerLiquor = 0f;
    public Text PlayerLiquorText;
    public Slider PlayerLiquorSlider;
    public Text SliderText;
    public float PlayerUpdatedLiquor = 0f;

    #region Start & Awake
    void Start ()
    {
        revolver.SetActive(true);
        shotgun.SetActive(false);
        RevolverEnd.SetActive(true);
        SGEnd.SetActive(false);
    }
    #endregion

    void Update()
    {
        #region Movement
        //if (Input.GetKey(KeyCode.W))
        //{
        //    transform.position += Vector3.forward * Time.deltaTime * player_Movement_Speed;
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    transform.position -= Vector3.forward * Time.deltaTime * player_Movement_Speed;
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    transform.position += Vector3.right * Time.deltaTime * player_Movement_Speed;
        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    transform.position -= Vector3.right * Time.deltaTime * player_Movement_Speed;
        //}

        if (Input.GetKey(KeyCode.W))
        {
            Vector3 temp = new Vector3(1, 0, 1);
            transform.position += temp * Time.deltaTime * player_Movement_Speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 temp = new Vector3(1, 0, 1);
            transform.position -= temp * Time.deltaTime * player_Movement_Speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 temp = new Vector3(-1, 0, 1);
            transform.position -= temp * Time.deltaTime * player_Movement_Speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 temp = new Vector3(-1, 0, 1);
            transform.position += temp * Time.deltaTime * player_Movement_Speed;
        }
        #endregion

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

        #region Rotation
        // Generate a plane that intersects the transform's position with an upwards normal.
        Plane playerPlane = new Plane(Vector3.up, transform.position);

        // Generate a ray from the cursor position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Determine the point where the cursor ray intersects the plane.
        // This will be the point that the object must look towards to be looking at the mouse.
        // Raycasting to a Plane object only gives us a distance, so we'll have to take the distance,
        //   then find the point along that ray that meets that distance.  This will be the point
        //   to look at.
        float hitdist = 0.0f;
        // If the ray is parallel to the plane, Raycast will return false.
        if (playerPlane.Raycast(ray, out hitdist))
        {
            // Get the point along the ray that hits the calculated distance.
            Vector3 targetPoint = ray.GetPoint(hitdist);

            // Determine the target rotation.  This is the rotation if the transform looks at the target point.
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);

            // Smoothly rotate towards the target point.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
        }
        #endregion
    }
}
