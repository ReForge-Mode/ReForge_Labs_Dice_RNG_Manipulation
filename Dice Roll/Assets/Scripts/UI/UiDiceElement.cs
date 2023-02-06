using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiDiceElement : MonoBehaviour
{
    public int elementIndex = 0;
    public int elementCount = 0;
    public UIDiceGuarantee diceGuarantee;
    public TextMeshProUGUI diceCount;
    public UnityEvent onValueZero;
    public UnityEvent onValueMax;
    public UnityEvent onValueValid;

    public void Start()
    {
        UpdateUI();
    }

    public void PlusValue()
    {
        if (diceGuarantee.totalCount < diceGuarantee.maxCount)
        {
            elementCount++;
            UpdateUI();
            UpdateUIMain();
        }
    }

    public void MinusValue()
    {
        if (elementCount > 0)
        {
            elementCount--;
            UpdateUI();
            UpdateUIMain();
        }
    }

    public void UpdateUIMain()
    {
        diceGuarantee.UpdateElementCount(elementIndex, elementCount);
    }

    public void UpdateUI()
    {
        diceCount.text = elementCount.ToString();

        //Assume that everything is okay
        onValueValid.Invoke();
        if (elementCount <= 0)
        {
            onValueZero.Invoke();
        }

        //Check global state takes priority, so it is applied last
        if (diceGuarantee.totalCount >= diceGuarantee.maxCount)
        {
            onValueMax.Invoke();
        }
        else if (diceGuarantee.totalCount <= 0)
        {
            onValueZero.Invoke();
        }
    }
}
