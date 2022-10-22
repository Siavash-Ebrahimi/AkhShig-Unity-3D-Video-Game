using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class eAnimEvents : MonoBehaviour
{
    [SerializeField] AudioSource eAudioSource;
    [SerializeField] AudioClip eFootAudioClip;
    public float enemyFootDistanceSound;
    void Start()
    {
        eAudioSource = GetComponent<AudioSource>();
        eAudioSource.volume = 0.0f;
        enemyFootDistanceSound = 18f; 
    }
    void Update()
    {
        //eAudioSource.volume += 1f;
    }
    public void Player_Hited_Sword()
    {        
              
    }
    public void EnemyDefenced()
    {
        Enemy_One.isEnemyDefenced = true;
    }

    public void EnemyNotDefenced()
    {
        
    }

    public void EnemyFootSoundAudioClip()
    {
        eAudioSource.volume = 0.065f;
        eAudioSource.PlayOneShot(eFootAudioClip);
        /*if (Enemy_One.distance < 18f)
        {
            eAudioSource.volume = (enemyFootDistanceSound / Enemy_One.distance) - 1f;
            eAudioSource.PlayOneShot(eFootAudioClip);
            print(eAudioSource.volume);
        }        
        else if (Enemy_One.distance > 30f)
        {
            eAudioSource.volume = 0.014f;
            eAudioSource.PlayOneShot(eFootAudioClip);
            print("Biger Than 30");
        }*/
    }
}
