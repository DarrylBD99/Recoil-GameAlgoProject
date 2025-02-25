using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private UpgradePlate[] _upgradePlates = new UpgradePlate[3];
    [SerializeField] private GameObject _upgradesUI;

    [SerializeField] private AudioClip _upgradeSound;
    [SerializeField] private AudioClip _levelUpSound;

    private float _amountStack = 1;
    private float _levelModulus = 4;
    private float _timer = 0.6f;

    public enum StatType
    {
        Attack,
//        Defense,
        Speed,
        Health,
        AttackCooldown,
        BulletSpeed,
        MaxHealth
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        _upgradesUI.SetActive(false);
        HUD.OnLevelUp += Upgrade;

        foreach (UpgradePlate upgradePlate in _upgradePlates)
            upgradePlate.OnUpgradePlateSeleceted += UpgradeSelected;
    }

    // Display Upgrade Plates
    void Upgrade() {
        _upgradesUI.SetActive(true);
        Time.timeScale = 0f; // Pause game
        AudioManager.PlaySoundEffect(_levelUpSound);

        if (HUD.Level % _levelModulus == 0) _amountStack += 0.5f;


        foreach (UpgradePlate upgradePlate in _upgradePlates) {
            StatType statType = (StatType)(UnityEngine.Random.Range(1, Enum.GetNames(typeof(StatType)).Length) - 1);

            float amount = UnityEngine.Random.Range(0.5f, _amountStack);

            if (statType == StatType.AttackCooldown && amount <= 1)
                amount = 1.5f;

            upgradePlate.SetStatType(statType, 1);
        }
    }

    void UpgradeSelected(StatType statType, float amount) {
        Time.timeScale = 1f; // Resume Game
        _upgradesUI.SetActive(false);
        AudioManager.PlaySoundEffect(_upgradeSound);

        switch (statType)
        {
            case StatType.Attack:
                PlayerController.PlayerInstance.attack += amount;
                break;
            //case StatType.Defense:
            //    PlayerController.PlayerInstance.defense += amount;
            //    break;
            case StatType.Speed:
                PlayerController.PlayerInstance.speed += amount;
                break;
            case StatType.Health:
                PlayerController.PlayerInstance.health += amount;
                break;
            case StatType.AttackCooldown:
                PlayerController.PlayerInstance.attackCooldown /= amount;
                break;
            case StatType.BulletSpeed:
                PlayerController.PlayerInstance.bulletSpeed += amount;
                break;
            case StatType.MaxHealth:
                PlayerController.PlayerInstance.maxHealth += amount;
                break;
        }
    }
}
