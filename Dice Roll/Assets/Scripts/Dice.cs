using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [Header("References")]
    public DiceData diceData;
    public GameObject[] faceDetectors;

    [Header("Debug")]
    public int defaultFaceResult = -1;
    public int alteredFaceResult = -1;

    /// <summary>
    /// For a possible object pooling system,
    /// we could reset the dice back and reuse it again
    /// </summary>
    public void Reset()
    {
        defaultFaceResult = -1;
        alteredFaceResult = -1;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F12))
        {
            defaultFaceResult = FindFaceResult();
            RotateDice(alteredFaceResult);
        }
    }

    /// <summary>
    /// Rotate the dice from the defaultFaceResult to alteredFaceResult
    /// </summary>
    /// <param name="alteredFaceResult"></param>
    public void RotateDice(int alteredFaceResult)
    {
        if (alteredFaceResult != 8)
        {
            this.alteredFaceResult = alteredFaceResult;
            Vector3 rotate = diceData.faceRelativeRotation[defaultFaceResult].rotation[alteredFaceResult];
            transform.Rotate(rotate);
        }
        else
        {
            this.alteredFaceResult = defaultFaceResult;
        }
    }

    /// <summary>
    /// Find the result of the roll, the topmost face of the dice
    /// </summary>
    /// <returns></returns>
    public int FindFaceResult()
    {
        //Since we have all child objects for each face,
        //We just need to find the highest Y value
        int maxIndex = 0;
        for (int i = 1; i < faceDetectors.Length; i++)
        {
            if (faceDetectors[maxIndex].transform.position.y < faceDetectors[i].transform.position.y)
            {
                maxIndex = i;
            }
        }
        defaultFaceResult = maxIndex;
        return maxIndex;
    }
}
