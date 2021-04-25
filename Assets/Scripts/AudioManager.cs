using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer Mixer;

    [Range(0, 1)] public float Bass;
    [Range(0, 1)] public float Drums;
    [Range(0, 1)] public float Piano;
    [Range(0, 1)] public float Sax;
    [Range(0, 1)] public float Rain;
    [Range(0, 1)] public float Thunder;

    // public void Start()
    // {
    //     Update();
    // }

    // public void Update()
    // {
    //     Mixer.SetFloat("Bass", Mathf.Lerp(-80, 0, Bass));
    //     Mixer.SetFloat("Drums", Mathf.Lerp(-80, 0, Drums));
    //     Mixer.SetFloat("Piano", Mathf.Lerp(-80, 0, Piano));
    //     Mixer.SetFloat("Sax", Mathf.Lerp(-80, 0, Sax));
    //     Mixer.SetFloat("Rain", Mathf.Lerp(-80, 0, Rain));
    //     Mixer.SetFloat("Thunder", Mathf.Lerp(-80, 0, Thunder));
    // }
}
