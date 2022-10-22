using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationEvents : MonoBehaviour
{    
    GameObject player;
    public static bool isEnemyHitedBySword;
    //----------------------------------
    public static bool isCons1;
    public static bool isCoffs1;
    GameObject enemyone;

    [SerializeField] AudioSource audioSoPlayerFoot;
    [SerializeField] AudioClip PlayerfootStepAudioClip;

    [SerializeField] AudioSource audioSoPlayer;
    [SerializeField] AudioClip PlayerDrwaSwordAudioClip;
    [SerializeField] AudioClip PlayerDrawPistolAudioClip;
    [SerializeField] AudioClip pSwordFight1AudioClip;
    [SerializeField] AudioClip pSwordFight2AudioClip;
    [SerializeField] AudioClip pSwordFight3AudioClip;

    [SerializeField] AudioClip pSwordAhh1AudioClip;
    [SerializeField] AudioClip pSwordAhh2AudioClip;
    // [SerializeField] AudioClip hitAudioClip;
    //[SerializeField] AudioClip bodyhitAudioClip;

    private float moveZ;
    private float moveX;
    public void Start()
    {        
        enemyone = GameObject.Find("Enemy");
        player = GameObject.Find("Player");
        isEnemyHitedBySword = false;        
    }

    public void Update()
    {
        moveZ = Input.GetAxisRaw("Vertical");
        moveX = Input.GetAxisRaw("Horizontal");
    }

    //---------------------------------
    public void C_ons1()
    {
        PlayerControler.isCon = true;
    }

    public void C_offs1()
    {
        PlayerControler.isCon = false;
    }    
    //---------------------------------

    public void Draw()
    {        
        player.gameObject.GetComponent<PlayerControler>().DrawSword();
    }

    public void Sheath()
    {
        player.gameObject.GetComponent<PlayerControler>().SheathSword();
    }

    // PISTOL EVENTS PART =======================================
    public void PistolDraw()
    {
        player.gameObject.GetComponent<PlayerControler>().PistolDraw();       
    }

    public void PistolSheath()
    {
        player.gameObject.GetComponent<PlayerControler>().PistolSheath();    
    }
    //===========================================================
    public void jumpEventIdol()
    {       
        player.gameObject.GetComponent<PlayerControler>().idelPhysicJump();        
    }

    public void jumpEventWalk()
    {
        player.gameObject.GetComponent<PlayerControler>().walkPhysicJump();        
    }

    public void StandJumpNo()
    {
        player.gameObject.GetComponent<PlayerControler>().StandJumpingOff();
    }

    /// Reading Sword Fight X Position
    public void StandFTrue()
    {
        player.gameObject.GetComponent<PlayerControler>().SF_X_T();
    }

    public void StandFFalse()
    {
        player.gameObject.GetComponent<PlayerControler>().SF_X_F();        
    }   

    public void Enemy_Hited_Sword()
    {
        player.gameObject.GetComponent<PlayerControler>().Enemy_Hited_Sword_Edit();        
        
    }
    
    public void Not_Enemy_Hited_Sword()
    {
        //Enemy_One.isEnemyDefenced = true;
    }

    public void EnemyNotDefenced()
    {
        //Enemy_One.isEnemyDefenced = true;
    }

    public void PlayerFootStepsSoundEfect()
    {
        if (moveZ != 0f || moveX != 0f)
        {
            audioSoPlayerFoot.PlayOneShot(PlayerfootStepAudioClip);
        }       
    }

    public void DrwaSwordSoundEfect()
    {
        audioSoPlayer.PlayOneShot(PlayerDrwaSwordAudioClip);        
    }

    public void DrwaSwordAhhSoundEfect()
    {        
        audioSoPlayerFoot.PlayOneShot(pSwordAhh2AudioClip);
    }

    public void DrwaSwordSoundEfectSheat()
    {
        audioSoPlayerFoot.PlayOneShot(PlayerDrwaSwordAudioClip);
    }

    public void DrwaPistolSoundEfect()
    {
        audioSoPlayer.PlayOneShot(PlayerDrawPistolAudioClip);
    }

    // Sound Efect Sword Fight ================================

    public void SwordFightAudioSound01()
    {
        audioSoPlayer.PlayOneShot(pSwordFight1AudioClip);        
        //audioSoPlayer.PlayOneShot(pSwordAhh2AudioClip);
    }
    public void SwordFightAudioSound02()
    {
        audioSoPlayer.PlayOneShot(pSwordFight2AudioClip);
        audioSoPlayer.PlayOneShot(pSwordAhh2AudioClip);
    }
    public void SwordFightAudioSound03()
    {
        audioSoPlayer.PlayOneShot(pSwordFight3AudioClip);
        audioSoPlayer.PlayOneShot(pSwordAhh2AudioClip);
    }


}
