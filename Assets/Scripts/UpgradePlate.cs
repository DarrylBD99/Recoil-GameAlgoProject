using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UpgradeUI;

public class UpgradePlate : MonoBehaviour
{
    public delegate void UpgradePlateSeleceted(StatType statType, float amount);
    public UpgradePlateSeleceted OnUpgradePlateSeleceted;

    [SerializeField]
    public Image icon;
    public TextMeshProUGUI header, description;

    public Sprite attackIcon, defenseIcon, speedIcon, healthIcon, attackCooldownIcon, bulletSpeedIcon, maxHealthIcon;

    [NonSerialized]
    public float value;

    [NonSerialized]
    public StatType upgradeStat;

    // Set up Upgrade Plate details
    public void SetStatType(StatType statType, float value) {
        this.value = value;
        upgradeStat = statType;
        string statTypeString = "";
        string formatBase = "Increase {0} by {1}.";

        switch (upgradeStat) {
            case StatType.Attack:
                icon.sprite = attackIcon;
                header.text = "Bulk Up";
                statTypeString = "Attack";
                break;
            //case StatType.Defense:
            //    icon.sprite = defenseIcon;
            //    header.text = "Toughen";
            //    statTypeString = "Defense";
            //    break;
            case StatType.Speed:
                icon.sprite = speedIcon;
                header.text = "Speed";
                statTypeString = "Movement Speed";
                break;
            case StatType.Health:
                icon.sprite = healthIcon;
                header.text = "Recover";
                statTypeString = "HP";
                formatBase = "Recover {0} by {1}.";
                break;
            case StatType.AttackCooldown:
                icon.sprite = attackCooldownIcon;
                header.text = "Faster Reload";
                statTypeString = "Attack Cooldown";
                formatBase = "Decrease {0} by {1}";
                break;
            case StatType.BulletSpeed:
                icon.sprite = bulletSpeedIcon;
                header.text = "Speed of Sound";
                statTypeString = "Bullet Speed";
                break;
            case StatType.MaxHealth:
                icon.sprite = maxHealthIcon;
                header.text = "Enhance";
                statTypeString = "Max Health";
                break;
        }
        description.text = string.Format(formatBase, statTypeString, value.ToString());
    }

    // Upgrade plate is clicked
    public void SelectUpgrade() {
        OnUpgradePlateSeleceted?.Invoke(upgradeStat, value);
    }
}
