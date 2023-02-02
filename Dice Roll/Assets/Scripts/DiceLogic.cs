using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceLogic : MonoBehaviour
{
    public Rigidbody rb;
    public MeshRenderer meshRenderer;
    public List<GameObject> faceList;
    public List<Texture> textureList;

    [Header("Audio")]
    public AudioSource soundLit;
    public AudioSource soundRollLow;
    public AudioSource soundRollHigh;

    private void Awake()
    {
        StartCoroutine(DoWhenStopped());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Arena"))
        {
            if(!soundRollLow.isPlaying) soundRollLow.Play();
        }

        if (collision.transform.CompareTag("Dice"))
        {
            if (!soundRollHigh.isPlaying) soundRollHigh.Play();
        }
    }

    private IEnumerator DoWhenStopped()
    {
        yield return new WaitForSeconds(1);

        while (true)
        {
            if (rb.velocity == Vector3.zero && rb.angularVelocity == Vector3.zero)
            {
                int topDiceFace = FindTopFace();
                LitUpTexture(topDiceFace);

                soundLit.Play();

                break;
            }
            yield return null;
        }
    }

    private int FindTopFace()
    {
        //Since we have all child objects for each face,
        //We just need to find the highest Y value
        int maxIndex = 0;
        for (int i = 1; i < faceList.Count; i++)
        {
            if (faceList[maxIndex].transform.position.y < faceList[i].transform.position.y)
            {
                maxIndex = i;
            }
        }
        return maxIndex;
    }

    private void LitUpTexture(int topFaceIndex)
    {
        //Find material. Unity reshuffle the material order, so I had to do this

        switch (topFaceIndex)
        {
            case 0: //Cryo
                meshRenderer.materials[4].mainTexture = textureList[topFaceIndex];
                break;

            case 1: //Dendro
                meshRenderer.materials[5].mainTexture = textureList[topFaceIndex];
                break;

            case 2: //Electro
                meshRenderer.materials[1].mainTexture = textureList[topFaceIndex];
                break;

            case 3: //Anemo
                meshRenderer.materials[0].mainTexture = textureList[topFaceIndex];
                break;

            case 4: //Omni
                meshRenderer.materials[6].mainTexture = textureList[topFaceIndex];
                break;

            case 5: //Geo
                meshRenderer.materials[7].mainTexture = textureList[topFaceIndex];
                break;

            case 6: //Pyro
                meshRenderer.materials[3].mainTexture = textureList[topFaceIndex];
                break;

            case 7: //Hydro
                meshRenderer.materials[2].mainTexture = textureList[topFaceIndex];
                break;

            default:
                break;
        }
    }
}
