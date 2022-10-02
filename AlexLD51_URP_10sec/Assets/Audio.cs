using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Audio : MonoBehaviour
{
    [SerializeField] AudioSource sourceA;
    [SerializeField] AudioSource sourceB;
    [SerializeField] AudioSource stingers;
    [SerializeField] AudioClip stingerup;
    [SerializeField] AudioClip stingerdown;
    [SerializeField] AudioSource oneshots;
    [SerializeField] AudioClip shootFail;

    [SerializeField] AudioMixerSnapshot chaseSnap;
    [SerializeField] AudioMixerSnapshot scatterSnap;
    [SerializeField] AudioMixer mixer;

    private void Start()
    {
       
    }
    public void FadeToScatter()
    {
        scatterSnap.TransitionTo(1);
        stingers.PlayOneShot(stingerup);
    }

    public void FadeToChase()
    {
        chaseSnap.TransitionTo(1);
        stingers.PlayOneShot(stingerdown);
    }

    public void PlayShootFail()
    {
        if (!oneshots.isPlaying) oneshots.PlayOneShot(shootFail);
    }
   


}
