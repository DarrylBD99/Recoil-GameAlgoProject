using UnityEngine;
using System.Collections;

public class DamageFlash : MonoBehaviour
{
    private Material _material;
    public static float FlashTime = 0.2f;
    public static float FlashAmount = 1f;

    private Coroutine _damageFlashCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        Entity entity = GetComponentInParent<Entity>();
        entity.OnDamaged += OnDamage;
        _material = GetComponent<SpriteRenderer>().material;

    }

    void OnDamage() {
        _damageFlashCoroutine = StartCoroutine(CallDamageFlash());
    }

    private IEnumerator CallDamageFlash() {
        _material.SetFloat("_FlashAmount", FlashAmount);

        yield return new WaitForSecondsRealtime(FlashTime);

        _material.SetFloat("_FlashAmount", 0f);
    }
}
