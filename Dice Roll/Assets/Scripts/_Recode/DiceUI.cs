using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceUI : MonoBehaviour
{
    [Header("States")]
    public bool isContactWithArena;
    public bool isContactWithDice;
    public bool isInSimulation = true;
    public bool isNotMoving = false;
    public bool isTextureLit = false;

    [Header("References")]
    public DiceData diceData;
    public Dice diceLogic;
    public MeshRenderer meshRenderer;
    public AudioSource soundLit;
    public AudioSource soundRollLow;
    public AudioSource soundRollHigh;

    /// <summary>
    /// For a possible object pooling system,
    /// we could reset the dice back and reuse it again
    /// </summary>
    public void Reset()
    {
        ResetTexturetoUnlit();
        isContactWithArena = false;
        isContactWithDice = false;
        isInSimulation = true;
        isNotMoving = false;
        isTextureLit = false;
    }

    public void ShowDiceResult()
    {
        if (isTextureLit == false)
        {
            soundLit.Play();
            ChangeTextureToLit(diceLogic.alteredFaceResult);
            isTextureLit = true;
        }
    }


    #region Texture-Related Functions
    /// <summary>
    /// Lit up the texture on a specific dice face
    /// </summary>
    /// <param name="faceResult"></param>
    private void ChangeTextureToLit(int faceResult)
    {
        //Unity shuffle the material order during import,
        //so I had to do this manually one by one

        switch (faceResult)
        {
            case 0: //Cryo
                meshRenderer.materials[4].mainTexture = diceData.textureLitList[faceResult];
                break;

            case 1: //Dendro
                meshRenderer.materials[5].mainTexture = diceData.textureLitList[faceResult];
                break;

            case 2: //Electro
                meshRenderer.materials[1].mainTexture = diceData.textureLitList[faceResult];
                break;

            case 3: //Anemo
                meshRenderer.materials[0].mainTexture = diceData.textureLitList[faceResult];
                break;

            case 4: //Omni
                meshRenderer.materials[6].mainTexture = diceData.textureLitList[faceResult];
                break;

            case 5: //Geo
                meshRenderer.materials[7].mainTexture = diceData.textureLitList[faceResult];
                break;

            case 6: //Pyro
                meshRenderer.materials[3].mainTexture = diceData.textureLitList[faceResult];
                break;

            case 7: //Hydro
                meshRenderer.materials[2].mainTexture = diceData.textureLitList[faceResult];
                break;
        }
    }

    /// <summary>
    /// Reset all lit texture back to normal
    /// </summary>
    private void ResetTexturetoUnlit()
    {
        meshRenderer.materials[4].mainTexture = diceData.textureUnlitList[0];
        meshRenderer.materials[5].mainTexture = diceData.textureUnlitList[1];
        meshRenderer.materials[1].mainTexture = diceData.textureUnlitList[2];
        meshRenderer.materials[0].mainTexture = diceData.textureUnlitList[3];
        meshRenderer.materials[6].mainTexture = diceData.textureUnlitList[4];
        meshRenderer.materials[7].mainTexture = diceData.textureUnlitList[5];
        meshRenderer.materials[3].mainTexture = diceData.textureUnlitList[6];
        meshRenderer.materials[2].mainTexture = diceData.textureUnlitList[7];
    }
    #endregion

    #region Audio-Related Functions
    //This is to help the Animation Recorder capture the event
    //when the sound should be played
    public void PlaySoundRollLow()
    {
        if (!soundRollLow.isPlaying) soundRollLow.Play();
    }

    public void PlaySoundRollHigh()
    {
        if (!soundRollHigh.isPlaying) soundRollHigh.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Arena"))
        {
            isContactWithArena = true;
        }

        if (collision.transform.CompareTag("Dice"))
        {
            isContactWithDice = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Arena"))
        {
            isContactWithArena = false;
        }

        if (collision.transform.CompareTag("Dice"))
        {
            isContactWithDice = false;
        }
    }
    #endregion
}
