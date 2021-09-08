using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public class PlayerExperience : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] TMP_Text experienceText = null;
    [SerializeField] TMP_Text levelText = null;

    [Header("Experience and Level Variables")]
    [SerializeField] int levelCap;
    [SerializeField] int currentXP;
    [SerializeField] int currentLevel;
    [SerializeField] int xpNeeded;

    public UnityEvent<int> LevelIncrease;

    private void Start()
    {
        currentLevel = 1;
        CalculateXPNeeded();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            AddXP(20);
        }
    }

    private void CalculateXPNeeded()
    {
        xpNeeded = Mathf.RoundToInt((5 * (Mathf.Pow((float)currentLevel + 1, 3))));
    }

    public void AddXP(int amount)
    {
        currentXP += amount;

        if (currentXP >= xpNeeded && currentLevel < levelCap)
        {
            currentLevel++;
            CalculateXPNeeded();

            LevelIncrease.Invoke(currentLevel);
        }
    }
}
