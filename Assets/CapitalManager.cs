using System;
using System.Collections;
using UnityEngine;

public class CapitalManager : MonoBehaviour, ICapitalManager
{
    public float MaxValue { get; private set; }
    public float CurrentValue { get; private set; }
    public float MainValue { get; private set; }
    public float UpgradeCost { get; private set; }

    private float accumulationRate = 1f;
    private float accumulationInterval = 5f;
    private Coroutine accumulationCoroutine;

    private void Start()
    {
        MaxValue = 1000f;
        UpgradeCost = 50f;
        LoadResource();
        CalculateAccumulatedResources();
        StartAccumulation();
    }

    private void OnApplicationQuit()
    {
        SaveResource();
    }

    private void StartAccumulation()
    {
        if (accumulationCoroutine != null)
        {
            StopCoroutine(accumulationCoroutine);
        }
        accumulationCoroutine = StartCoroutine(AccumulateResourcePeriodically());
    }

    private IEnumerator AccumulateResourcePeriodically()
    {
        while (true)
        {
            AccumulateResource();
            yield return new WaitForSeconds(accumulationInterval);
        }
    }

    public void AccumulateResource()
    {
        if (CurrentValue < MaxValue)
        {
            CurrentValue = Mathf.Min(CurrentValue + accumulationRate, MaxValue);
            SaveResource();
        }
    }

    public void LoadResource()
    {
        CurrentValue = PlayerPrefs.GetFloat("Resource", 0);
        MainValue = PlayerPrefs.GetFloat("MainValue", 0);
        MaxValue = PlayerPrefs.GetFloat("MaxValue", 1000f);
        accumulationRate = PlayerPrefs.GetFloat("AccumulationRate", 1f);
        accumulationInterval = PlayerPrefs.GetFloat("AccumulationInterval", 5f);
        UpgradeCost = PlayerPrefs.GetFloat("UpgradeCost", 50f);
    }

    public void SaveResource()
    {
        PlayerPrefs.SetFloat("Resource", CurrentValue);
        PlayerPrefs.SetFloat("MainValue", MainValue);
        PlayerPrefs.SetFloat("MaxValue", MaxValue);
        PlayerPrefs.SetFloat("AccumulationRate", accumulationRate);
        PlayerPrefs.SetFloat("AccumulationInterval", accumulationInterval);
        PlayerPrefs.SetFloat("UpgradeCost", UpgradeCost);
        PlayerPrefs.SetString("LastSavedTime", DateTime.Now.ToBinary().ToString());
        PlayerPrefs.Save();
    }

    private void CalculateAccumulatedResources()
    {
        if (PlayerPrefs.HasKey("LastSavedTime"))
        {
            long binaryTime = Convert.ToInt64(PlayerPrefs.GetString("LastSavedTime"));
            DateTime lastSavedTime = DateTime.FromBinary(binaryTime);
            TimeSpan timePassed = DateTime.Now - lastSavedTime;

            int intervalsPassed = (int)(timePassed.TotalSeconds / accumulationInterval);
            float accumulatedAmount = intervalsPassed * accumulationRate;

            CurrentValue = Mathf.Min(CurrentValue + accumulatedAmount, MaxValue);
        }
    }

    public void TransferToMain()
    {
        MainValue += CurrentValue;
        CurrentValue = 0;
        SaveResource();
    }

    public void ResetAllProgress()
    {
        CurrentValue = 0;
        MainValue = 0;
        MaxValue = 1000f;
        accumulationRate = 1f;
        accumulationInterval = 5f;
        UpgradeCost = 50f;
        PlayerPrefs.DeleteAll();
        SaveResource();
        StartAccumulation();
    }

    public void Upgrade()
    {
        if (MainValue >= UpgradeCost)
        {
            MainValue -= UpgradeCost;
            MaxValue *= 1.05f;
            accumulationInterval *= 0.95f;
            UpgradeCost *= 1.1f;
            SaveResource();
            StartAccumulation();
        }
    }
    public void AddCoins(int amount)
    {
        CurrentValue += amount;
        SaveResource();
    }
}