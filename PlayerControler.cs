using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControler : MonoBehaviour
{
    //[SerializeField] AudioSource audioSource;
    
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    public Animator animator;
    //[SerializeField] Text enemyHealth;

    public CharacterController controller;
    public float speed = 6.0f;

    public Transform cam;
    public GameObject cameraMain;
    public Vector3 cameraEnd;    

    public float turnSmoothTime = 0.1f;
    float turnSmoothValocity;

    private Vector3 direction;
    private Vector3 moveDir;
    private float targetAngle;
    private float angle;

    [SerializeField] private float gravity;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;

    [SerializeField] private float jumpHeight;

    public GameObject playerBackFront;

    private Vector3 velocity;

    private float moveZ;
    private float moveX;

    private bool zMove;
    private bool zForwardMove;
    private bool targetDown;
    private bool targetForward;
    private float saveTargetDown;
    private float saveTargetForward;
    private bool xLeftOrRightMovment;

    // SWORD VARIABLES & REFERENCES !!
    //[SerializeField] private GameObject sword;
    //[SerializeField] private GameObject swordBackIdelHolder;

    public static bool isSwordArmed;
    public static bool isPistoldArmed;
    private GameObject mainSwordFixed;
    private GameObject swordBackpag;
    private GameObject mainPistolFixed;
    private GameObject pistolOnLeg;
    private GameObject crossHair;
    private bool isSwordChangin;
    private bool isStandJump;
    private bool isWalkJump;
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    // Sword Fight =====================================
    public static bool isSwordChoosed;
    private bool isSwordFight;
    private bool isStandFight1;
    private bool isStandFight2;
    private bool isWalkFight1;
    private bool isWalkFight2;
    public static bool isXposition;
    public static bool isChanging;
    // Pistol Fight =====================================
    [SerializeField] private GameObject aimDrone;
    private bool isPistoldChangin;
    public static bool isPistolChoosed;
    public static bool isPistolFight;
    public static float PistolX;
    public static float PistolY;
    public static float AngleX;
    public static float AngleY;
    public static bool isPistolReadyToShoot;
    public static float bulletDistance;
    [SerializeField] Text bulletDistanceDisplay;
    [SerializeField] Text enemyHealth;
    [SerializeField] GameObject playerRightHand;
    [SerializeField] GameObject playerLeftHand;
    private float rHand;
    public static bool isCon;
    // Commbat Commands =============================
    public static bool isEnemyDetacted;
    public static bool isDefenceActive;
    private float deathTime;
    private float deathTimer;
    [SerializeField] private Text gameOver;
    [SerializeField] Slider playerHealthSlider;
    public static float pHealth;
    public static float maxPlayerHealth;
    //public Transform enemyPrefab;
    GameObject enemy;
    [SerializeField] GameObject swordlazer;
    [SerializeField] GameObject body;
    [SerializeField] AudioSource audiosource; 
    [SerializeField] AudioClip gunShotAudioClip;

    [SerializeField] AudioSource lowAudioSource;
    //[SerializeField] AudioClip playerBrithing;
    void Start()
    {
        body = GameObject.Find("BodyEnemySword");
        swordlazer = GameObject.Find("SwordLazer");
        enemy = GameObject.Find("Enemy");
        pHealth = 1000f;
        maxPlayerHealth = 1000f;
        playerHealthSlider.value = pHealth / maxPlayerHealth;
        //Instantiate(enemyPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        isChanging = false;
        isCon = false;
        //Movments Part -=-=-=-=-=-=-=-=-=-=-
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        zMove = false;
        targetDown = false;
        zForwardMove = true;
        targetForward = false;
        xLeftOrRightMovment = false;
        isStandJump = false;
        isWalkJump = false;
        playerBackFront = GameObject.Find("Camera Look");
        cameraMain = GameObject.Find("Cam Follo look");

        //Sword Part -=-=-=-=-=-=-=-=-=-=-
        isSwordChoosed = true;
        isSwordArmed = false;
        mainSwordFixed = GameObject.Find("Sword_Main");
        mainSwordFixed.SetActive(false);
        swordBackpag = GameObject.Find("MagicSword_Iron");
        isSwordChangin = false;
        //Pistol Part -=-=-=-=-=-=-=-=-=-=-
        aimDrone = GameObject.Find("Drone");        
        mainPistolFixed = GameObject.Find("pistolinHand");
        mainPistolFixed.SetActive(false);
        crossHair = GameObject.Find("CrossHair");
        crossHair.SetActive(false);
        pistolOnLeg = GameObject.Find("pistol2");        
        isPistoldArmed = false;
        isPistolChoosed = false;
        isPistoldChangin = false;
        // Pistol Fight -=-=-=-=-=-=-=-=-=-=-
        isPistolFight = false;
        isPistolReadyToShoot = false;
        playerRightHand = GameObject.Find("RightForeArm");
        playerLeftHand = GameObject.Find("LeftForeArm");
        
        // Sword Fight -=-=-=-=-=-=-=-=-=-=-
        isSwordFight = false;
        isStandFight1 = true;
        isStandFight2 = false;
        isWalkFight1 = true;
        isWalkFight2 = false;
        isXposition = false;
        // Sword Combat -=-=-=-=-=-=-=-=-=-=-
        gameOver.text = "";
        isEnemyDetacted = false;
        isDefenceActive = false;
        pHealth = 1000f;
        deathTime = 8f;      
    }

    void Update()
    {        
        if (Input.GetKeyDown(KeyCode.C))
        {
            //isCon = true;
            if (isSwordChoosed && !isSwordArmed)
            {
                isSwordChoosed = false;
                isPistolChoosed = true;
                print("Pistol Choosen");
            }
            else if(isPistolChoosed && !isPistoldArmed)
            {
                isSwordChoosed = true;
                isPistolChoosed = false;
                print("Sword Choosen");
            }
            else if (isSwordChoosed && isSwordArmed && isGrounded)
            {                
                isSwordChoosed = false;
                isPistolChoosed = true;                
                animator.SetBool("SSword", true);
                if (!isPistoldArmed)
                {                    
                    animator.SetBool("DPistol", true);
                }
                else if (isPistoldArmed)
                {
                    animator.SetBool("SPistol", true);
                }
                else
                {
                    isPistoldChangin = false;
                }
            }
            else if (isPistolChoosed && isPistoldArmed && isGrounded)
            { 
                isSwordChoosed = true;
                isPistolChoosed = false;                
                animator.SetBool("SPistol", true);
                if (!isSwordArmed)
                {
                    animator.SetBool("DSword", true);
                }
                else if (isSwordArmed)
                {
                    animator.SetBool("SSword", true);
                }
                else
                {
                    isSwordChangin = false;
                }               

            }
        }

        cameraEnd = cameraMain.transform.position;
        if (pHealth > 1f)
        {
            if (transform.position.y > 5f && !isGrounded && velocity.y < -3.5f)
            {
                animator.SetBool("floting", true);                
            }
            else if (transform.position.y < 5f && transform.position.y > 4f && !isGrounded && velocity.y < -3.5f)
            {
                animator.SetBool("falling", true);                
            }
            else
            {
                animator.SetBool("floting", false);
                animator.SetBool("falling", false);
            }            

            Move();

            if (Input.GetKey(KeyCode.Z) && isGrounded && direction == Vector3.zero && isSwordArmed)
            {
                animator.SetBool("Defa", true);
                isDefenceActive = true;
            }
            else
            {
                animator.SetBool("Defa", false);
                isDefenceActive = false;
            }

            if (Input.GetKeyDown(KeyCode.Mouse1) && isGrounded )
            {
                isChanging = true;
                if (isSwordChoosed)
                {
                    isSwordChangin = true;
                    if (!isSwordArmed)
                    {                        
                        animator.SetBool("DSword", true);
                    }
                    else if (isSwordArmed)
                    {                        
                        animator.SetBool("SSword", true);
                    }
                    else
                    {
                        isSwordChangin = false;
                    }
                }
                 else if(isPistolChoosed)
                {
                    isPistoldChangin = true;                     
                    if (!isPistoldArmed)
                    {                        
                        animator.SetBool("DPistol", true);
                    }
                    else if (isPistoldArmed)
                    {                      
                        animator.SetBool("SPistol", true);
                    }
                    else
                    {
                        isPistoldChangin = false;
                    }
                }

            }

            if (isPistoldArmed && isPistolReadyToShoot)
            {                
                RaycastHit phit;
                if (Physics.Raycast(aimDrone.transform.position, aimDrone.transform.forward, out phit, Mathf.Infinity, LayerMask.GetMask("BadEnemy")))
                {
                    bulletDistance = phit.distance;
                    isEnemyDetacted = true;
                    if (phit.distance > 17f)
                    {
                        if (bulletDistance > 50f)
                        {
                            bulletDistanceDisplay.text = (int)bulletDistance + " Meter" + " | It's Far Away";
                            EnemyHealthPercentageCalculation();    
                        }
                        else
                        {
                            bulletDistanceDisplay.text = (int)bulletDistance + " Meter" + " | I Got It ...";
                            EnemyHealthPercentageCalculation();
                        }
                    }                     
                    else
                    {
                        bulletDistanceDisplay.text = "";
                        enemyHealth.text = "";
                    }
                }
                else
                {
                    bulletDistanceDisplay.text = "";
                    enemyHealth.text = "";
                    isEnemyDetacted = false;
                }
            }
            else
            {
                bulletDistanceDisplay.text = "";
                enemyHealth.text = "";
            }

            ///Enemy_Hited_Sword_Edit();

            if (Input.GetKeyDown(KeyCode.Mouse0) && isGrounded && !isDefenceActive)
            {
                if (isSwordArmed)
                {                    
                    isSwordFight = true;
                    if (direction == Vector3.zero)
                    {
                        if (isStandFight1 && !isStandFight2)
                        {
                            isStandFight1 = false; isStandFight2 = true;
                            if (isXposition)
                            {
                                animator.SetBool("X1P", true);                                
                            }
                            else if (!isXposition)
                            {                                
                                animator.SetBool("SSF1", true);                                
                            }
                        }

                        else if (!isStandFight1 && isStandFight2)
                        {
                            isStandFight1 = true; isStandFight2 = false;
                            if (isXposition)
                            {
                                animator.SetBool("X3P", true);                                
                            }
                            else if (!isXposition)
                            {
                                animator.SetBool("SSF2", true);                                
                            }
                        }
                    }

                    if (direction != Vector3.zero && !isDefenceActive)
                    {
                        moveSpeed = 0f;
                        if (isWalkFight1 && !isWalkFight2 && !isDefenceActive)
                        {
                            isWalkFight1 = false; isWalkFight2 = true;
                            animator.SetBool("SWF1", true);                            
                        }

                        else if (!isWalkFight1 && isWalkFight2 && !isDefenceActive)
                        {
                            isWalkFight1 = true; isWalkFight2 = false;
                            animator.SetBool("SWF2", true);                            
                        }
                    }
                }

                if (isPistoldArmed)
                {
                    audiosource.PlayOneShot(gunShotAudioClip);
                    if (isEnemyDetacted && bulletDistance < 50f && isPistolReadyToShoot)
                    {                        
                        isPistolFight = true;
                        Hit_Enemy_Sword_Pistol();
                    }                         
                }
                else
                {                    
                    isPistolFight = false;                    
                }

            }
            else
            {
                animator.SetBool("SSF1", false); animator.SetBool("SSF2", false); animator.SetBool("X3P", false); animator.SetBool("X1P", false);
                animator.SetBool("SWF1", false); animator.SetBool("SWF2", false); animator.SetBool("GunShootVibration", false); isPistolFight = false;
            }
        }

        else
        {           
            animator.SetBool("pDethII", true);
            deathTimer += Time.deltaTime;
            if (deathTimer > 5f)
            {
                gameOver.text = "GAME OVER";
            }
            if (deathTimer > deathTime)
            {
                cameraMain.transform.position += new Vector3(0, transform.position.y * 1.23f , 0);
            }
            if (deathTimer > 5f)
            {
                //Application.Quit();
            }

        }
    }

    private void Move()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }        
            
        moveZ = Input.GetAxisRaw("Vertical");
        moveX = Input.GetAxisRaw("Horizontal");

        if (moveZ < 0)
        {
            zMove = true;
            zForwardMove = false;
        }

        if (moveZ > 0)
        {
            zForwardMove = true;
            zMove = false;
        }

        if (moveX > 0f || moveX < 0f)
        {
            xLeftOrRightMovment = true;
        }        
        
        direction = new Vector3(moveX, 0f, moveZ).normalized;
        targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothValocity, turnSmoothTime);

        //================================================
        //================================================
        // Amaing Pistol Coding =+=+=+=+=+=+=+=+=+=+=+=
        // The "angle" variable is the player exact routation angle, so I found two deferent degrees
        // of "angle" variable and named the "AngleX" && "AngleY" | "AngleX" is "angle" + 30 degree more
        // && "AngleY" is "angle" - 19 degree less, then I coded > if main Camera angle be in the range of
        // "AngleX" && "AngleY" variabales, then it is time to define a crosshair, so as degrees in our "Main Camera"
        // by variabale name "cam", and the "angle" variabales are between (0 - 360) 
        // I used a slise pitza trick (Right // Left) to mention the missing degrees in the "else" section. 

        AngleX = (+angle) + 30f;
        AngleY = (+angle) - 19f;        
        //-----------------------------------------------
        if (AngleX > 360f)
        {
            AngleX = (AngleX - 360f);
        }
        else
        {
            AngleX = angle + 30f;
        }
                
        if (AngleY < 0)
        {
            AngleY = (360f - (-AngleY));
        }
        else
        {
            AngleY = AngleY - 19f;
        }


        if (cam.eulerAngles.y > AngleY && cam.eulerAngles.y < AngleX && isPistoldArmed)
        {
            isPistolReadyToShoot = true;
            crossHair.SetActive(true);
        }

        else
        {
            if (cam.eulerAngles.y > 328f && cam.eulerAngles.y < 360f && AngleY > 280f && isPistoldArmed)
            {
                isPistolReadyToShoot = true;
                crossHair.SetActive(true);
            }
            else if (cam.eulerAngles.y > 0f && cam.eulerAngles.y < 30f && AngleX < 50f && isPistoldArmed)
            {
                isPistolReadyToShoot = true;
                crossHair.SetActive(true);
            }
            else
            {
                isPistolReadyToShoot = false;
                crossHair.SetActive(false);
            }

        }
        //================================================
        //================================================

        playerCamMovment();

        if (isGrounded && !isStandJump && !isSwordFight)
        {
            
            if (direction != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                if (isChanging || isCon)
                {
                    moveSpeed = 0.0f;
                }
                else
                {
                    Walk();
                }                
                moveDir = Quaternion.Euler(0f, targetAngle, 0f) * (Vector3.forward);
            }

            else if (direction != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                if (isChanging || isCon)
                {
                    moveSpeed = 0.0f;
                }
                else
                {
                    Run();
                }                
                moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            }

            else if (direction == Vector3.zero)
            {
                moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.zero;
                Idol();
            }

            else
            {
                moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.zero;
            }
            direction *= moveSpeed;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (direction == Vector3.zero)
                {
                    isStandJump = true;
                    animator.SetBool("IdelJump", true);                    
                }
                if (direction != Vector3.zero)
                {                    
                    animator.SetBool("WalkJump", true);
                    isWalkJump = true;
                }                                
            }
            else
            {
                animator.SetBool("IdelJump", false);
                animator.SetBool("WalkJump", false);
                isSwordChangin = false;
                isStandJump = false;
                isWalkJump = false;
                isSwordFight = false;
            }
        }
        
        controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    
    private void Idol()
    {
        if (!isSwordArmed && !isPistoldArmed)
        {                       
            animator.SetFloat("Blend", 0f, 0.1f, Time.deltaTime);
        }
        
        else if (isSwordArmed)
        {
            isXposition = false;
            animator.SetFloat("MovAct", 0f, 0.1f, Time.deltaTime);
        }

        else if (isPistoldArmed)
        {
            isXposition = false;
            animator.SetFloat("PistolMovment", 0f, 0.1f, Time.deltaTime);
        }
    }

    private void Walk()
    {
        
        if (!isSwordArmed && !isPistoldArmed && isGrounded && !isStandJump && !isDefenceActive)
        {
            moveSpeed = walkSpeed;
            animator.SetFloat("Blend", 0.5f, 0.1f, Time.deltaTime);
        }

        else if (isSwordArmed && isGrounded && !isStandJump && !isSwordFight && !isDefenceActive)
        {            
            moveSpeed = walkSpeed - 1f;
            animator.SetFloat("MovAct", 0.5f, 0.1f, Time.deltaTime);
        }

        else if (isPistoldArmed && !isSwordArmed && isGrounded && !isStandJump)
        {
            moveSpeed = walkSpeed;
            animator.SetFloat("PistolMovment", 0.5f, 0.1f, Time.deltaTime);
        }
    }

    private void Run()
    {
        moveSpeed = runSpeed;
        if (!isSwordArmed && !isPistoldArmed && isGrounded && !isStandJump && !isDefenceActive)
        {
            moveSpeed = runSpeed;
            animator.SetFloat("Blend", 1f, 0.1f, Time.deltaTime);
        }

        else if (isSwordArmed && isGrounded && !isStandJump && !isSwordFight && !isDefenceActive)
        {            
            moveSpeed = runSpeed - 1f;
            animator.SetFloat("MovAct", 1f, 0.1f, Time.deltaTime);
        }

        else if (isPistoldArmed && !isSwordArmed && isGrounded && !isStandJump)
        {
            moveSpeed = runSpeed;
            animator.SetFloat("PistolMovment", 1f, 0.1f, Time.deltaTime);
        }
    }
    
    public void idelPhysicJump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);        
    }    

    public void StandJumpingOff()
    {        
        isStandJump = false;        
        isSwordChangin = false;
        isSwordFight = false;
    }

    public void walkPhysicJump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        isSwordFight = false;
    }

    public void DrawSword()
    {      
        mainSwordFixed.SetActive(true);
        swordBackpag.SetActive(false);
        isSwordArmed = true;
        animator.SetBool("DSword", false);
        isSwordChangin = false;
        isChanging = false;
    }
    public void SheathSword()
    {        
        mainSwordFixed.SetActive(false);
        swordBackpag.SetActive(true);        
        isSwordArmed = false;       
        animator.SetBool("SSword", false);
        isSwordChangin = false;
        isChanging = false;
    }

    // PISTOL VOIDS ----------------------------------------

    public void PistolDraw()
    {       
        mainPistolFixed.SetActive(true);
        pistolOnLeg.SetActive(false);
        isPistoldArmed = true;
        animator.SetBool("DPistol", false);
        isPistoldChangin = false;
        isChanging = false;
    }

    public void PistolSheath()
    {        
        mainPistolFixed.SetActive(false);
        pistolOnLeg.SetActive(true);
        isPistoldArmed = false;
        animator.SetBool("SPistol", false);
        isPistoldChangin = false;
        isChanging = false;
    }

    //======================================================
    private void playerCamMovment()
    {
        if (moveZ == 0f && zMove == true && moveX == 0f)
        {
            if (!targetDown)
            {
                saveTargetDown = targetAngle + 180f;
                transform.rotation = Quaternion.Euler(0f, saveTargetDown, 0f);
                targetDown = true;
            }
        }
        else if (moveZ < 0f && zMove == true)
        {
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
        else if (moveZ == 0f && zForwardMove == true && moveX == 0f)
        {
            if (!targetForward)
            {
                saveTargetForward = targetAngle;
                transform.rotation = Quaternion.Euler(0f, saveTargetForward, 0f);
                targetForward = true;
            }
        }
        else if (moveZ == 0f && xLeftOrRightMovment == true && moveX == 0f)
        {
            if (!targetForward)
            {
                saveTargetForward = angle;
                transform.rotation = Quaternion.Euler(0f, saveTargetForward, 0f);
                targetForward = true;
            }            
        }
        else if (moveZ > 0f && zForwardMove == true)
        {
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }       

        else
        {      
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            zMove = false;
            targetDown = false;
            zForwardMove = false;
            targetForward = false;            
        }
    }    
    public void SF_X_T()
    {
        isXposition = true;        
    }

    public void SF_X_F()
    {
        isXposition = false;        
        isSwordFight = false;        
    }
    /*private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 300f);
        Gizmos.DrawRay(swordlazer.transform.position, swordlazer.transform.forward * 30f);
    }*/
    public void EnemyHealthPercentageCalculation()
    {
        /*if ((int)((Enemy_One.eHealth / Enemy_One.eMaxHealth) * 100) <= 0)
        {
            enemyHealth.text = "% 0 | Enemy Eliminated";
        }
        else
        {
            enemyHealth.text = "% " + (int)((Enemy_One.eHealth / Enemy_One.eMaxHealth) * 100) + " | Still Alive";
        }*/
    }

    public void playerDirectHit(int x)
    {
        pHealth -= x;
        playerHealthSlider.value = pHealth / maxPlayerHealth;        
    }

    public void Enemy_Hited_Sword_Edit()
    {       
        RaycastHit phit;
        if (Physics.Raycast(swordlazer.transform.position, swordlazer.transform.forward, out phit, Mathf.Infinity, LayerMask.GetMask("BadEnemy")))
        {
            phit.collider.gameObject.GetComponent<Enemy_One>().HitEnemy(25);
        }        
    }

    public void Hit_Enemy_Sword_Pistol()
    {
        RaycastHit phit;
        if (Physics.Raycast(aimDrone.transform.position, aimDrone.transform.forward, out phit, Mathf.Infinity, LayerMask.GetMask("BadEnemy")))
        {                     
            phit.collider.gameObject.GetComponent<Enemy_One>().HitEnemy(25);            
        }
    }

}
