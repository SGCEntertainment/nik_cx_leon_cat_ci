using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if(!instance)
            {
                instance = FindObjectOfType<SoundManager>();
            }

            return instance;
        }
    }

    [SerializeField] AudioSource effect;
    [SerializeField] AudioSource loop;

    [Space(10)]
    [SerializeField] AudioClip select;
    [SerializeField] AudioClip normal;
    [SerializeField] AudioClip negative;

    public void PlayEffect(int id)
    {
        if(effect.isPlaying)
        {
            effect.Stop();
        }

        AudioClip target = id switch
        {
            0 => select,
            1 => normal,
            2 => negative,
            _ => null
        };

        if (target != null)
        {
            effect.PlayOneShot(target);
        }
    }

    public void SetMute(out bool IsMute)
    {
        effect.mute = !effect.mute;
        loop.mute = !loop.mute;

        IsMute = effect.mute;
    }
}
