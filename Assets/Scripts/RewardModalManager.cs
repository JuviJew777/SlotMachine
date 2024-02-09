using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


/// <summary>
/// Class that handles the victory modal where the score will be displayed in case of winning.
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
    /// Function to activate victory animations and sounds if it occurs.
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
