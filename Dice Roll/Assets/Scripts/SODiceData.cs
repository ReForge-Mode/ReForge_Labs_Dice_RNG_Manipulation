using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "D8 Data", menuName = "Scriptable Object/Dice Data")]
public class DiceData : ScriptableObject
{
    public List<FaceRelativeRotation> faceRelativeRotation;
    public List<Texture> textureUnlitList;
    public List<Texture> textureLitList;

    [System.Serializable]
    public struct FaceRelativeRotation
    {
        public string element;
        public List<Vector2> rotation;
    }
}