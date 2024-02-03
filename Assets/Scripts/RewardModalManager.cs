using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


/// <summary>
/// Clase que maneja el modal de victoria donde se desplegara el score en caso de ganar
/// </summary>
public class RewardModalManager : MonoBehaviour
{
    public static RewardModalManager Instance;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    [SerializeField]
    ParticleSystem[] myParticles;

    [SerializeField]
    Animator ModuleAnimation;

    [SerializeField]
    TextMeshProUGUI scoreText;

    /// <summary>
    /// Funcion para activar animaciones y sonidos de victoria en caso de que esta ocurra
    /// </summary>
    /// <param name="winingReels"></param>
    /// <param name="score"></param>
    public void StartSuccessAnimation(int winingReels,int score)
    {
        SoundManager.instance.PlayWinSound();
        ModuleAnimation.SetTrigger("SetAnim");

        if (winingReels < 3)
        {
            myParticles[0].Play();
            myParticles[1].Play();
        }
        else
        {
            myParticles[0].Play();
            myParticles[1].Play();
            myParticles[2].Play();
        }
        scoreText.text = ""+score;
        ModuleAnimation.Play("WinModuleShow");
    }

}
