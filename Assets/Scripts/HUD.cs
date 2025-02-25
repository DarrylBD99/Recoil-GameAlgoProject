using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public delegate void LeveledUp();
    
    public Slider xpBar;
    public Slider hpBar;
    
    public static LeveledUp OnLevelUp;
    public static int Level = 1;
    private static int _xp = 0;
    private static int _maxXp = 5;
    private static float _maxXpMultiplier = 1.5f;

    // Update is called once per frame
    void Update() {
        hpBar.maxValue = PlayerController.PlayerInstance.maxHealth;
        hpBar.value = PlayerController.PlayerInstance.health;

        xpBar.maxValue = _maxXp;
        xpBar.value = _xp;
    }

    public static void AddXP(int amount) {
        _xp += amount;
        if (_xp >= _maxXp) {
            _xp -= _maxXp;
            _maxXp = (int)(_maxXp * _maxXpMultiplier);
            Level++;
            OnLevelUp?.Invoke();
            Debug.Log("Max XP: " + _maxXp);
            Debug.Log("Level: " + Level);
        }
    }
}
