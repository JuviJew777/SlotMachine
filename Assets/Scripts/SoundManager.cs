using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase encargada del manejo de audios
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    AudioSource winAudio;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        winAudio = GetComponent<AudioSource>();
    }
    public void PlayWinSound()
    {
        winAudio.Play();
    }
}
