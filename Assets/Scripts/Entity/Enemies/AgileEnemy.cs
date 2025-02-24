using System.Collections;
using System.Linq;
using UnityEngine;

public class AgileEnemy : EnemyBase
{
    public GameObject dagger;
    public Transform daggerDamageTip;
    public Transform daggerDamageBottom;

    private bool _damagedTarget;
    private bool _cooldownBool;
    private float _cooldown;

    // States for Agile Enemy
    protected enum State {
        Follow,
        Attack
    }

    private State _currentState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected new void Start() {
        _currentState = State.Follow;
        _damagedTarget = false;
        _cooldownBool = false;
        _cooldown = 0f;

        dagger.SetActive(false);

        base.Start();
    }

    // Update is called once per frame
    protected new void Update() {
        if (_currentState == State.Follow) {
            if (Vector3.Distance(transform.position, Target.transform.position) <= entity.range && !_cooldownBool) {
                _currentState = State.Attack;
                StartCoroutine("Attack");
            }

            base.Update();
        }
        if (_cooldownBool) {
            _cooldown += Time.deltaTime;
            if (_cooldown >= entity.attackCooldown) {
                _cooldown = 0f;
                _cooldownBool = false;
            }
        }
    }

    // Update is called once per fixed frame
    protected new void FixedUpdate() {
        if (_currentState == State.Follow)
            base.FixedUpdate();
    }

    // Agile swings dagger to attack
    IEnumerator Attack() {
        dagger.transform.rotation = sprite.transform.rotation;
        dagger.transform.Rotate(new(0, 0, -50));
        dagger.SetActive(true);

        Vector3 daggerPosPreviousTip = daggerDamageTip.position;
        Vector3 daggerPosPreviousBottom= daggerDamageBottom.position;

        float attackTime = entity.attackSpeed;
        for (float x = 0.0f; x <= attackTime; x += Time.deltaTime) {
            dagger.transform.Rotate(new(0, 0, (attackTime - x) * 3));

            // Check if player is within dagger
            if (!_damagedTarget) {
                RaycastHit2D[] tipDamage = Physics2D.LinecastAll(daggerPosPreviousTip, daggerDamageTip.position);
                RaycastHit2D[] bottomDamage = Physics2D.LinecastAll(daggerPosPreviousBottom, daggerDamageBottom.position);

                RaycastHit2D[] hitObjects = tipDamage.Concat(bottomDamage).ToArray();

                foreach (RaycastHit2D hitObject in hitObjects)
                    if (hitObject.collider.gameObject.GetComponent<Entity>() == Target) {
                        Target.damage(entity.attack);
                        _damagedTarget = true;
                        break;
                    }
            }
            yield return null;
        }
        _currentState = State.Follow;
        _cooldownBool = true;
        dagger.SetActive(false);
        _damagedTarget = false;
    }
}
