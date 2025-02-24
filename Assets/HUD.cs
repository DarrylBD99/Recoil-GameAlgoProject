using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static Entity player;
    public Slider xpBar;
    public Slider hpBar;

    // Update is called once per frame
    void Update()
    {
        hpBar.maxValue = player.maxHealth;
        hpBar.value = player.health;
    }
}
