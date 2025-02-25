using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public delegate void LeveledUp();
    
    public Slider xpBar;
    public Slider hpBar;
    
    public static LeveledUp OnLevelUp;
    public static int Level;
    public static int xpTotal;
    public static float surviveSeconds;

    private static int _xp;
    private static int _maxXp;
    private static float _maxXpMultiplier = 1.5f;

    private bool _playerInstanceInitialised = false;
    
    [SerializeField] private GameObject _deathScreen;

    void Start() {
        Level = 1;
        _xp = 0;
        xpTotal = _xp;
        _maxXp = 3;
        surviveSeconds = 0f;

        _deathScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (!(_playerInstanceInitialised || PlayerController.PlayerInstance.IsUnityNull())) {
            PlayerController.PlayerInstance.OnDeath += ShowDeathScreen;
            _playerInstanceInitialised = true;
        }

        hpBar.maxValue = PlayerController.PlayerInstance.maxHealth;
        hpBar.value = PlayerController.PlayerInstance.health;

        xpBar.maxValue = _maxXp;
        xpBar.value = _xp;

        if (!PlayerController.PlayerInstance.IsDestroyed())
            surviveSeconds += Time.deltaTime;
    }

    public static void AddXP(int amount) {
        xpTotal += amount;
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

    private void ShowDeathScreen() {
        _deathScreen.SetActive(true);
        Time.timeScale = 0f;
    }
}
