using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceAudio : MonoBehaviour
{
    public AudioSource soundRollLow;
    public AudioSource soundRollHigh;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Arena"))
        {
            if (!soundRollLow.isPlaying) soundRollLow.Play();
        }

        if (collision.transform.CompareTag("Dice"))
        {
            if (!soundRollHigh.isPlaying) soundRollHigh.Play();
        }
    }
}
