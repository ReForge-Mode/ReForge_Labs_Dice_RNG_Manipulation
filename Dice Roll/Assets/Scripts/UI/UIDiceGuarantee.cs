using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UIDiceGuarantee : MonoBehaviour
{
    public int totalCount = 0;
    public int maxCount;
    public List<int> diceGuaranteeCount;
    public DiceManager2 diceManager;
    public TextMeshProUGUI countText;

    public List<UiDiceElement> diceElementList;

    public void Awake()
    {
        maxCount = diceManager.generateAmount;
        totalCount = 0;
    }

    public void UpdateElementCount(int elementIndex, int value)
    {
        diceGuaranteeCount[elementIndex] = value;

        //Count all elements
        totalCount = 0;
        for (int i = 0; i < 8; i++)
        {
            totalCount += diceGuaranteeCount[i];
        }
        countText.text = totalCount.ToString();

        UpdateUIElement();
        UpdateGuarantee();
    }

    public void UpdateUIElement()
    {
        foreach (var ui in diceElementList)
        {
            ui.UpdateUI();
        } 
    }

    /// <summary>
    /// Put the needed dice into the DiceManager
    /// </summary>
    public void UpdateGuarantee()
    {
        int diceCount = diceManager.generateAmount - 1;

        //For every diceGuarantee on the list
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < diceGuaranteeCount[i]; j++)
            {
                diceManager.targetedResult[diceCount] = (Elements)i;
                diceCount--;
            }
        }

        //Fill the rest with Any dice
        for (int i = diceCount; i > 0; i--)
        {
            diceManager.targetedResult[i] = Elements.Any;
        }
    }
}