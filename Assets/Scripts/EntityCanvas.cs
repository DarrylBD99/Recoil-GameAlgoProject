using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EntityCanvas : MonoBehaviour
{
    private GameObject _healthBar;
    private Slider _healthBarSlider;

    private Entity _entity;
    private float _elapsedSeconds = 0f;
    private static float HealthBarVisibleTime = 3f;

    private bool _showHealthBar = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        // Get Entity
        _entity = GetComponentInParent<Entity>();
        _entity.OnDamaged += UpdateHealthBar;

        _healthBar = transform.Find("HealthBar").gameObject;
        _healthBarSlider = _healthBar.GetComponent<Slider>();

        // Setup Health Bar
        _healthBarSlider.maxValue = _entity.maxHealth;
        _healthBarSlider.value = _entity.health;
        _healthBar.SetActive(_showHealthBar);

    }

    void UpdateHealthBar() {
        _healthBarSlider.value = _entity.health;
        _showHealthBar = true;
        _elapsedSeconds = 0f;
    }

    void Update()
    {
        if (_showHealthBar) {
            _elapsedSeconds += Time.deltaTime;

            if (_elapsedSeconds > HealthBarVisibleTime)
                _showHealthBar = false;
        }

        _healthBar.SetActive(_showHealthBar);
        transform.rotation = Quaternion.identity;
    }
}
