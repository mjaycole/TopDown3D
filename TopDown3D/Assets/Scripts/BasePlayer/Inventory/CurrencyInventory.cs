using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyInventory : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] TMP_Text currencyText = null;

    [Header("Values")]
    [SerializeField] int currentCurrency;

    void Start()
    {
        currencyText.text = currentCurrency.ToString();
    }

    public void AddCurrency(int amount)
    {
        currentCurrency += amount;
        currencyText.text = currentCurrency.ToString();
    }
}
