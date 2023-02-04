using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictDice : MonoBehaviour
{
    public List<ElementData> elementData;

    public void AlterTheResult(Elements result, Elements changeTo)
    {
        Vector3 rotateAlteration = Vector3.zero;

        //Find element of the same name
        for (int i = 0; i < elementData.Count; i++)
        {
            if (elementData[i].element == result)
            {
                //Find the value for that element
                if (changeTo == Elements.Cryo) rotateAlteration = elementData[i].elementRelativeOrientation.Cryo;
                if (changeTo == Elements.Dendro) rotateAlteration = elementData[i].elementRelativeOrientation.Dendro;
                if (changeTo == Elements.Electro) rotateAlteration = elementData[i].elementRelativeOrientation.Electro;
                if (changeTo == Elements.Anemo) rotateAlteration = elementData[i].elementRelativeOrientation.Anemo;
                if (changeTo == Elements.Omni) rotateAlteration = elementData[i].elementRelativeOrientation.Omni;
                if (changeTo == Elements.Geo) rotateAlteration = elementData[i].elementRelativeOrientation.Geo;
                if (changeTo == Elements.Pyro) rotateAlteration = elementData[i].elementRelativeOrientation.Pyro;
                if (changeTo == Elements.Hydro) rotateAlteration = elementData[i].elementRelativeOrientation.Hydro;
            }
        }

        //Rotate by that amount, with x and y flipped. But why? I don't know
        transform.Rotate(rotateAlteration);
    }

    [System.Serializable]
    public struct ElementData
    {
        public Elements element;
        public GameObject heightDetector;
        public ElementRelativeOrientation elementRelativeOrientation;
    }

    /// <summary>
    /// This struct details all rotation needed to spin the dice to a certain face
    /// </summary>
    [System.Serializable]
    public struct ElementRelativeOrientation
    {
        public Vector2 Cryo;
        public Vector2 Dendro;
        public Vector2 Electro;
        public Vector2 Anemo;
        public Vector2 Omni;
        public Vector2 Geo;
        public Vector2 Pyro;
        public Vector2 Hydro;
    }
}

/// <summary>
/// This lists all elements and the index order from 0-7
/// </summary>
public enum Elements
{
    Cryo, Dendro, Electro, Anemo, Omni, Geo, Pyro, Hydro, None
}
