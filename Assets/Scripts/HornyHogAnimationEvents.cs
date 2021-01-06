using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornyHogAnimationEvents : MonoBehaviour
{
    [SerializeField] HornyHogController hornyHog;
    private EnemySoundManager enemySoundManager;
    [SerializeField] GameObject TwinkleAnim;
    private void Awake()
    {
        enemySoundManager = FindObjectOfType<EnemySoundManager>();

    }

    public void PlayOneSoundEvaporate()
    {
        enemySoundManager.PlayOneSound("Evaporate");
    }
    public void PlayOneSoundHitGround()
    {
        enemySoundManager.PlayOneSound("HitGround");
    }

    public void PlayOneSoundPreAttack()
    {
        //enemySoundManager.PlayOneSound("PreAttack");
    }
    public void PlayTwinkle()
    {
        TwinkleAnim.SetActive(true);
        Invoke("StopTwinkle", 0.3f);
    }

    private void StopTwinkle()
    {
        TwinkleAnim.SetActive(false);
    }

}
