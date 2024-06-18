using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    List <AudioSource> audios=new List<AudioSource>();
    private int audiosNum=3;
    public AudioClip SE_player_shoot;
    public AudioClip SE_player_hurt;
    public AudioClip SE_player_death;
    public AudioClip SE_enemy_hurt;
    public AudioClip SE_enemy_death;
    void Start()
    {
        
    }

    public AudioClip GetAudio(string name)
    {
        switch(name)
        {
            case "SE_player_shoot":
                return SE_player_shoot;
            case "SE_player_hurt":
                return SE_player_hurt;
            case "SE_player_death":
                return SE_player_death;
            case "SE_enemy_hurt":
                return SE_enemy_hurt;
            case "SE_enemy_death":
                return SE_enemy_death;

        }
        return null;
    }
}
