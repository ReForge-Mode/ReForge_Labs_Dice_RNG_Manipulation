using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiceLogic : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    public MeshRenderer meshRenderer;
    public AudioSource soundLit;
    public List<GameObject> faceList;
    public List<Texture> textureUnlitList;
    public List<Texture> textureLitList;

    [Header("Debug")]
    public bool isNotMoving = false;
    public bool isSimulated = false;

    private void Update()
    {
        CheckWhenStopMoving();
    }

    private void CheckWhenStopMoving()
    {
        if (rb.velocity == Vector3.zero && rb.angularVelocity == Vector3.zero)
        {
            if (isNotMoving == false)
            {
                //This part only triggers once when the bool change value
                int topDiceFace = FindTopFace();
                ChangeTextureToLit(topDiceFace);
                soundLit.Play();

                isNotMoving = true;
            }
        }
        else
        {
            if(isNotMoving == true)
            {
                ResetTexturetoUnlit();
            }

            isNotMoving = false;
        }
    }

    public int FindTopFace()
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

    private void ResetTexturetoUnlit()
    {
        meshRenderer.materials[4].mainTexture = textureUnlitList[0];
        meshRenderer.materials[5].mainTexture = textureUnlitList[1];
        meshRenderer.materials[1].mainTexture = textureUnlitList[2];
        meshRenderer.materials[0].mainTexture = textureUnlitList[3];
        meshRenderer.materials[6].mainTexture = textureUnlitList[4];
        meshRenderer.materials[7].mainTexture = textureUnlitList[5];
        meshRenderer.materials[3].mainTexture = textureUnlitList[6];
        meshRenderer.materials[2].mainTexture = textureUnlitList[7];
    }

    private void ChangeTextureToLit(int topFaceIndex)
    {
        //Unity reshuffle the material order, so I had to do this manually

        switch (topFaceIndex)
        {
            case 0: //Cryo
                meshRenderer.materials[4].mainTexture = textureLitList[topFaceIndex];
                break;

            case 1: //Dendro
                meshRenderer.materials[5].mainTexture = textureLitList[topFaceIndex];
                break;

            case 2: //Electro
                meshRenderer.materials[1].mainTexture = textureLitList[topFaceIndex];
                break;

            case 3: //Anemo
                meshRenderer.materials[0].mainTexture = textureLitList[topFaceIndex];
                break;

            case 4: //Omni
                meshRenderer.materials[6].mainTexture = textureLitList[topFaceIndex];
                break;

            case 5: //Geo
                meshRenderer.materials[7].mainTexture = textureLitList[topFaceIndex];
                break;

            case 6: //Pyro
                meshRenderer.materials[3].mainTexture = textureLitList[topFaceIndex];
                break;

            case 7: //Hydro
                meshRenderer.materials[2].mainTexture = textureLitList[topFaceIndex];
                break;

            default:
                break;
        }
    }
}
