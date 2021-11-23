using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;

public class BGMManager : MonoBehaviour
{
    public AudioClip safeBGM;
    public AudioClip battleBGM;

    [SerializeField] float musicTransitionDuration;

    private void Start()
    {
        TengenToppaAudioManager.Instance.PlayMusic(safeBGM, safeBGM, 4);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TransitionToBattleMusic();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            TransitionToSafeMusic();
        }
    }

    public void TransitionToBattleMusic()
    {
        StartCoroutine(BattleMusicTransition());
    }

    IEnumerator BattleMusicTransition()
    {
        TengenToppaAudioManager.Instance.StopMusic();
        yield return new WaitForSeconds(1);
        TengenToppaAudioManager.Instance.PlayMusic(battleBGM, battleBGM, 2);

    }

    public void TransitionToSafeMusic()
    {
        StartCoroutine(SafeMusicTransition());
    }

    IEnumerator SafeMusicTransition()
    {
        TengenToppaAudioManager.Instance.StopMusic();
        yield return new WaitForSeconds(1);
        TengenToppaAudioManager.Instance.PlayMusic(safeBGM, safeBGM, 4);

    }
}
