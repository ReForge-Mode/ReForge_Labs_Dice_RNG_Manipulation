using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIDiceGuarantee : MonoBehaviour
{
    public int totalCount = 0;
    public DiceGuaranteeCount diceGuaranteeCount;
    public TextMeshProUGUI countText;
    public List<TextMeshProUGUI> elementCountText;


    public void AddCount(int elementIndex)
    {
        if (totalCount < 8)
        {
            int i = (elementIndex);
            switch (i)
            {
                case 0: diceGuaranteeCount.Cryo++; break;
                case 1: diceGuaranteeCount.Dendro++; break;
                case 2: diceGuaranteeCount.Electro++; break;
                case 3: diceGuaranteeCount.Anemo++; break;
                case 4: diceGuaranteeCount.Omni++; break;
                case 5: diceGuaranteeCount.Geo++; break;
                case 6: diceGuaranteeCount.Pyro++; break;
                case 7: diceGuaranteeCount.Hydro++; break;
            }
            totalCount++;
            UpdateElementCount();
            countText.text = totalCount.ToString();
        }
    }

    public void ReduceCount(int elementIndex)
    {
        if (totalCount > 0)
        {
            switch (elementIndex)
            {
                case 0: diceGuaranteeCount.Cryo--; break;
                case 1: diceGuaranteeCount.Dendro--; break;
                case 2: diceGuaranteeCount.Electro--; break;
                case 3: diceGuaranteeCount.Anemo--; break;
                case 4: diceGuaranteeCount.Omni--; break;
                case 5: diceGuaranteeCount.Geo--; break;
                case 6: diceGuaranteeCount.Pyro--; break;
                case 7: diceGuaranteeCount.Hydro--; break;
            }
            totalCount--;
            UpdateElementCount();
            countText.text = totalCount.ToString();
        }
    }

    public void UpdateElementCount()
    {
        elementCountText[0].text = diceGuaranteeCount.Cryo.ToString();
        elementCountText[1].text = diceGuaranteeCount.Dendro.ToString();
        elementCountText[2].text = diceGuaranteeCount.Electro.ToString();
        elementCountText[3].text = diceGuaranteeCount.Anemo.ToString();
        elementCountText[4].text = diceGuaranteeCount.Omni.ToString();
        elementCountText[5].text = diceGuaranteeCount.Geo.ToString();
        elementCountText[6].text = diceGuaranteeCount.Pyro.ToString();
        elementCountText[7].text = diceGuaranteeCount.Hydro.ToString();
    }
}

public struct DiceGuaranteeCount
{
    public int Cryo;
    public int Dendro;
    public int Electro;
    public int Anemo;
    public int Omni;
    public int Geo;
    public int Pyro;
    public int Hydro;
}
