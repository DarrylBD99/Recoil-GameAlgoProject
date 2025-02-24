using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Slider xpBar;
    public Slider hpBar;

    // Update is called once per frame
    void Update()
    {
        hpBar.maxValue = PlayerController.PlayerInstance.maxHealth;
        hpBar.value = PlayerController.PlayerInstance.health;
    }
}
