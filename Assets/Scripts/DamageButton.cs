using UnityEngine;

public class DamageButton : MonoBehaviour
{
    public float amount;
    public void DamageTarget(Entity target) {
        if (target == null) {
            Debug.Log("Entity is dead");
            return;
        }

        target.damage(amount);
    }
}
