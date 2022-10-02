using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("UI stuff")]
    public float gameTimer;
    public bool scatter;
    [SerializeField] Slider timeSlider;
    [SerializeField] Animator timerAnim;
    [SerializeField] TMP_Text bulletText;
    [SerializeField] thoughts _thoughts;

    [Header("level feedback")]
    [SerializeField] Color chaseColor;
    [SerializeField] Color scatterColor;
    [SerializeField] MeshRenderer groundColor;
    [SerializeField] Animator playerGraphicsAnim;

    [Header("references")]
    [SerializeField] Audio gameAudio;
    [SerializeField] Vector2 _inputMovement = Vector2.zero;
    [SerializeField] Rigidbody rb;
    [SerializeField] float movementSpeed = 5;
    [SerializeField] float rotSpeed = 500;
    Vector3 moveDir;
    public int bullets;
    [SerializeField] GameObject bulletPrefab;
   [SerializeField]  Transform gunEnd;
   [SerializeField] float shootSpeed;
    [SerializeField] float shootCoolDown = 0.2f;
    float shootTimer;
    

    public bool playerInputEnabled;
    [Header("death stuff")]
    [SerializeField] GameObject deathPrefab;
    [SerializeField] Transform fxSpawnPoint;
    [SerializeField] GameObject playerGraphics;
    [SerializeField] GameObject winCam;

    // Start is called before the first frame update

    public static Player instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        
    }
    void Start()
    {
        bullets = 0;
        UpdateBulletUI();
        playerInputEnabled = true;

        movementSpeed += PlayerPersistance.instance.difficulty;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > 24) rb.MovePosition(new Vector3(  -23.9f, transform.position.y, transform.position.z ) );
        if (transform.position.x < -24) rb.MovePosition(new Vector3(24, transform.position.y, transform.position.z));
        gameTimer -= Time.deltaTime;
        timeSlider.value = gameTimer / 10;
        if (gameTimer <= 0)
        {
            scatter = !scatter;
            timerAnim.Play("Flip");

            if (scatter)
            {
                _thoughts.DisplayPositiveThought();
                gameAudio.FadeToScatter();
                enemies.instance.PickScatterPoints();
                groundColor.materials[0].SetColor("_BaseColor", scatterColor) ;
                foreach (var e in enemies.instance.activeEnemies)
                {
                    e.SwitchEnemyState(EnemyState.scatter);
                }
            }

            if (!scatter)
            {
                playerGraphicsAnim.Play("Chased");
                _thoughts.DisplayNegativeThought();
                gameAudio.FadeToChase();
                groundColor.materials[0].SetColor("_BaseColor", chaseColor);
                foreach (var e in enemies.instance.activeEnemies)
                {
                e.SwitchEnemyState(EnemyState.chase);
                }
            }
            gameTimer = 10;
        }

        if (playerInputEnabled)
        {
            HandleGameplayInputs();
        }
        else moveDir = Vector3.zero;

 

        //directional rotation
        if (_inputMovement != Vector2.zero)
        {
          Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
         transform.rotation = Quaternion.RotateTowards(transform.rotation,
             toRotation, rotSpeed * Time.deltaTime);
        }

        //set animation
        //  if (!_inputMovement.Equals(Vector2.zero)) _animator.SetInteger(ANIM_STATE, ANIM_MOVE_FORWARD);
        //apply input to velocity
    }

    private void HandleGameplayInputs()
    {
        if (!scatter && Input.GetKeyDown(KeyCode.Space) || bullets == 0 && Input.GetKeyDown(KeyCode.Space))
        {
            gameAudio.PlayShootFail();
        }
        if (scatter && Input.GetKeyDown(KeyCode.Space) && bullets > 0 && Time.time > shootTimer)
        {
            Shoot();
        }
    
        moveDir = Vector3.zero;
        _inputMovement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveDir = new Vector3(_inputMovement.x, 0, _inputMovement.y).normalized;
    }

    private void Shoot()
    {
        bullets -= 1;
        UpdateBulletUI();
        shootTimer = Time.time + shootCoolDown;
        var bullet = Instantiate(bulletPrefab, gunEnd.position, gunEnd.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(gunEnd.forward * shootSpeed, ForceMode.Impulse);
    }

    public void Die()
    {
        PlayerPersistance.instance.difficulty = 0; //reset difficulty on death
        playerInputEnabled = false;
        rb.velocity = Vector3.zero;
        playerGraphics.SetActive(false);
        //player graphics off
        Instantiate(deathPrefab, fxSpawnPoint.position , transform.rotation);
        StartCoroutine(ResetScene());
    }

    public void Win()
    {
        winCam.SetActive(true); 
        StartCoroutine(WinPause());
    }

    IEnumerator WinPause()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(2);
    }
    IEnumerator ResetScene()
    {
        Time.timeScale = 0.3f;
        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 1;
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);

    }
    public void UpdateBulletUI()
    {
        bulletText.SetText(": " + bullets);
    }
    void FixedUpdate()
    {
        rb.velocity = moveDir * movementSpeed;
    }
}
