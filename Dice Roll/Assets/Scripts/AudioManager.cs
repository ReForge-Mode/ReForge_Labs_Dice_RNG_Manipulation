using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] sounds;

    private void Awake()
    {
        foreach (AudioClip s in sounds)
        {
            //s.sou = gameObject.AddComponent<AudioSource>();

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
