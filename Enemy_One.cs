using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Enemy_One : MonoBehaviour
{
    [SerializeField] private Text eFirstMessage;
    private CharacterController eController;
    private Animator eAnimator;
    private GameObject player;
    public static float distance;
    private float hdistance;
    private Vector3 lookAt;
    [SerializeField] private float radius;
    private float enemySpeed;
    private float enemyWaitTime;
    private float enemyWaitTimer;
    private bool isWaitingTime;
    [SerializeField] private GameObject enemyEye;
    [SerializeField] public Slider enemyHealghtSlider;
    private GameObject enemySlider;

    //===================

    public float turnSmoothTime = 0.1f;
    float turnSmoothValocity;

    private Vector3 direction;
    private Vector3 moveDir;
    private float targetAngle;
    private float angle;

    private Vector3 hdirection;
    private Vector3 hmoveDir;
    private float htargetAngle;

    private bool loockPlayer;
    private bool isplayerEnemyFaceToFace;

    private Vector3 enemyFirstPlace;       

    //=================== Graavity ====
    private Vector3 velocity;
    [SerializeField] private float gravity;
    private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] LayerMask groundMask;

    //=================== Combat =======
    private bool isFighted;
    public static bool isEnemyDefenced;
    [SerializeField] Text playerHealth;
    private float eHealth;
    private float eMaxHealth;

    public static float copyEnemyHealth;
    public static float copyEnemyMaxHealth;

    public static bool isEnemyInPlayerHitArea;
    public static bool isEnemyInPlayerHitAreaBack;
    private bool isPlayerArmedHerSword;
    //private float hitSwordTime;
    //private float hitSwordTimer;

    //public Transform enemyPrefab;

    [SerializeField] GameObject body;
    private BoxCollider enemyBox;
    private float hitTime;
    private float hitTimer;
    public static bool isEnemyShoted;
    private float shotTime;
    private float shotTimer;
    private float SwordShotTime;
    private float SwordShotTimer;
    void Start()
    {
        isEnemyShoted = false;
        hitTime = 1.06f;
        shotTime = 1.5f;
        SwordShotTime = 1.2f;
        enemyBox = GetComponent<BoxCollider>();
        body = GameObject.Find("Enemy_Idel");
        //Instantiate(enemyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        player = GameObject.Find("Player");
        eController = GetComponent<CharacterController>();
        eAnimator = GetComponentInChildren<Animator>();
        enemyEye = GameObject.Find("eEye");
        enemySlider = GameObject.Find("eSlider");
        enemyFirstPlace = transform.position;
        enemyWaitTime = 3f;
        loockPlayer = false;
        isplayerEnemyFaceToFace = false;

        isFighted = false;
        isEnemyDefenced = false;

        eFirstMessage.text = "";
        //isStandFight1 = false;
        //isStandFight2 = true;
        //hitSwordTime = 1f;
        eHealth = 300f;
        eMaxHealth = 300f;
        enemyHealghtSlider.value = eHealth / eMaxHealth;        
        enemySlider.SetActive(false);
        isEnemyInPlayerHitArea = false;
        isPlayerArmedHerSword = false;
        //print(eHealth);
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "PlayerHitArea")
        {
            isEnemyInPlayerHitArea = true;
            //print("fromt player");
        }
        if (other.gameObject.tag == "PlayerHitAreaBack")
        {
            isEnemyInPlayerHitAreaBack = true;
            //print("back player");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        isEnemyInPlayerHitArea = false;
        isEnemyInPlayerHitAreaBack = false;
    }
    // Update is called once per frame
    
    void Update()
    {
        if (isEnemyShoted)
        {
            shotTimer += Time.deltaTime;
            if (shotTimer > shotTime)
            {
                isEnemyShoted = false;
                shotTimer = 0f;
            }
        }
                
        if (PlayerControler.isSwordArmed)
        {
            //print("Sword is On Enemy");
            enemyBox.center = new Vector3(0f, 5.92f, -5.1f);
            enemyBox.size = new Vector3(11.77f, 11.71f, 10.62f);
        }
        if (PlayerControler.isPistoldArmed)
        {
            //print("Pistol is On Enemy");
            enemyBox.center = new Vector3(-0.0135498f, 1.032873f, 0.06293249f);
            enemyBox.size = new Vector3(0.6341209f, 1.924762f, 0.2825727f);
        }        
        
        enemyHealghtSlider.value = eHealth / eMaxHealth;

        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        FallingGravity();

        // Calculation distance between Enemy && Enemy's First Place Standing ==========================
        hdistance = Vector3.Distance(transform.position, enemyFirstPlace);

        if (eHealth > 1)
        {
            // Calculation distance between Enemy && Player ==========================
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            distance = Vector3.Distance(transform.position, player.transform.position);
        }
        
        if (eHealth > 1 && PlayerControler.pHealth > 0f)
        {
            if (distance < radius && !isEnemyShoted)
            {
                direction = (player.transform.position - transform.position).normalized;
                targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothValocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                loockPlayer = false;

                if (distance < 25f)
                {
                    eAnimator.SetBool("IdeolEnemy", true);
                }

                if (distance < 18f && !isplayerEnemyFaceToFace)
                {
                    enemySpeed = 5f;
                    eAnimator.SetFloat("eMovment", 1f, 0.1f, Time.deltaTime);
                }

                if (distance < 5f && !isplayerEnemyFaceToFace)
                {
                    if (PlayerControler.isSwordArmed)
                    {
                        isPlayerArmedHerSword = true;
                    }
                    enemySlider.SetActive(true);
                    if (PlayerControler.isSwordArmed)
                    {
                        enemySpeed = 1.6f;
                    }
                    if (!PlayerControler.isSwordArmed)
                    {
                        enemySpeed = 2.8f;
                    }

                    eAnimator.SetFloat("eMovment", 0.5f, 0.1f, Time.deltaTime);
                }

                if (distance < 2f)
                {
                    if (PlayerControler.isSwordArmed)
                    {                        
                        isPlayerArmedHerSword = true;
                    }
                    isplayerEnemyFaceToFace = true;
                    enemySpeed = 0f;
                    eAnimator.SetFloat("eMovment", 0.0f, 0.1f, Time.deltaTime);
                    FightSowrd();
                }

                if (distance > 2.1f)
                {
                    isplayerEnemyFaceToFace = false;
                    eAnimator.SetBool("SSF1", false);
                    eAnimator.SetBool("eDefa", false);
                }

                ///if (distance < 5 && !isFighted && !isDefenced)
                ///{
                ///eFirstMessage.text = "You ... Stranger !!                 Get Out Of This Zone Right Now !!!";
                ///}
                if (distance > 5)
                {
                    enemySlider.SetActive(false);
                    ///eFirstMessage.text = "";
                }

                enemyWaitTimer = 0;
                if (!isplayerEnemyFaceToFace)
                {
                    eController.Move(moveDir.normalized * enemySpeed * Time.deltaTime);
                }
            }
            else if(!isEnemyShoted)
            {
                Back_Home();
            }
        }
        else
        {
            if (PlayerControler.pHealth < 10f)
            {
                eAnimator.SetBool("SSF1", false);
                eAnimator.SetBool("eDefa", false);
                Back_Home();
            }
            else
            {                
                eAnimator.SetTrigger("eDethe");
                Destroy(gameObject, 5f);
            }
        }
        velocity.y += gravity * Time.deltaTime;
        eController.Move(velocity * Time.deltaTime);
    }   

    public void FightSowrd()
    {
        if (PlayerControler.pHealth > 0)
        {            
            if(isEnemyInPlayerHitArea || isEnemyInPlayerHitAreaBack)
            {
                enemySlider.SetActive(true);
                
                if (!PlayerControler.isXposition && isPlayerArmedHerSword)
                {
                    isEnemyDefenced = false;
                    isFighted = true;
                    enemySpeed = 0F;
                    eAnimator.SetBool("eDefa", false);
                    eAnimator.SetBool("SSF1", true);
                    Player_Hited_Sword_main();
                }
                if (PlayerControler.isXposition && isPlayerArmedHerSword)
                {
                    enemySpeed = 0F;
                    eAnimator.SetBool("SSF1", false);
                    eAnimator.SetBool("eDefa", true);
                    //isEnemyDefenced = true;
                }                
            }
            else
            {
                eAnimator.SetBool("eDefa", false); isEnemyDefenced = false;
            }
        }
        else
        {
            eAnimator.SetBool("SSF1", false);
            eAnimator.SetBool("eDefa", false);
        }
    }  

    public void FallingGravity()
    {
        if (transform.position.y > 5f && !isGrounded && velocity.y < -3.5f)
        {
            eAnimator.SetBool("flotingAct", true);  
        }
        else if (transform.position.y < 5f && transform.position.y > 4f && !isGrounded && velocity.y < -3.5f)
        {
            eAnimator.SetBool("landingAct", true);
        }
        else
        {            
            eAnimator.SetBool("flotingAct", false);
            eAnimator.SetBool("landingAct", false);
        }
    }

    public void Back_Home()
    {
        if (hdistance > 0.5f)
        {
            enemyWaitTimer += Time.deltaTime;
            if (enemyWaitTimer > enemyWaitTime)
            {
                hdirection = (enemyFirstPlace - transform.position).normalized;
                htargetAngle = Mathf.Atan2(hdirection.x, hdirection.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, htargetAngle, 0f);
                hmoveDir = Quaternion.Euler(0f, htargetAngle, 0f) * Vector3.forward;
                if (hdistance > 7f)
                {
                    if (PlayerControler.pHealth < 0)
                    {
                        enemySpeed = 2f;
                        eAnimator.SetFloat("eMovment", 0.5f, 0.1f, Time.deltaTime);
                    }
                    else
                    {
                        enemySpeed = 7f;
                        eAnimator.SetFloat("eMovment", 1f, 0.1f, Time.deltaTime);
                    }

                }
                if (hdistance < 6f)
                {
                    enemySpeed = 3f;
                    eAnimator.SetFloat("eMovment", 0.5f, 0.1f, Time.deltaTime);
                }

                eController.Move(hmoveDir.normalized * enemySpeed * Time.deltaTime);
                loockPlayer = true;
            }
            else
            {
                enemySpeed = 0f;
                eAnimator.SetFloat("eMovment", 0.0f, 0.1f, Time.deltaTime);
            }

        }
        else
        {
            if (PlayerControler.pHealth < 10)
            {
                eAnimator.SetBool("SSF1", false);
                eAnimator.SetBool("eDefa", false);
            }
            enemySpeed = 0f;
            eAnimator.SetBool("IdeolEnemy", false);
            eAnimator.SetFloat("eMovment", 0.0f, 0.1f, Time.deltaTime);
            enemyWaitTimer = 0f;
            if (loockPlayer)
            {
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                loockPlayer = false;
            }

        }
    }
    
    public void HitEnemy(int Damage)
    {
        if (eHealth > 0)
        {
            if (PlayerControler.isSwordArmed)
            {
                //eHealth -= (Damage);
                //enemyHealghtSlider.value = eHealth / eMaxHealth;
                if (!isEnemyDefenced && isEnemyInPlayerHitArea)
                {
                    eHealth -= (Damage);
                    enemyHealghtSlider.value = eHealth / eMaxHealth;
                }
            }            

            if (PlayerControler.isPistolFight)
            {
                isEnemyShoted = true;
                eAnimator.SetTrigger("ShotT");
                eHealth -= Damage;
                enemyHealghtSlider.value = eHealth / eMaxHealth;                               
            }
        }
    }
    public void Player_Hited_Sword_main()
    {
        hitTimer += Time.deltaTime;
        if (hitTimer > hitTime)
        {
            if (!PlayerControler.isDefenceActive && isEnemyInPlayerHitArea && !isEnemyDefenced)
            {
                player.gameObject.GetComponent<PlayerControler>().playerDirectHit(10);
            }
            if (isEnemyInPlayerHitAreaBack && !isEnemyDefenced)
            {
                player.gameObject.GetComponent<PlayerControler>().playerDirectHit(10);
            }
            hitTimer = 0f;
        }
        
    }
}