using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class XPSpawn : MonoBehaviour
{
    [SerializeField] int xpToAdd;
    [SerializeField] TMP_Text xpText = null;

    public void SetXP(int amount)
    {
        xpToAdd = amount;
        GameObject.FindObjectOfType<PlayerExperience>().AddXP(xpToAdd);
        xpText.text = "+ " + xpToAdd.ToString() + " XP";
    }
}
