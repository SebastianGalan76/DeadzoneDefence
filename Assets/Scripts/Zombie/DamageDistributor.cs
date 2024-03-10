using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UI;

public class DamageDistributor : MonoBehaviour {
    [SerializeField] private float damage;
    [SerializeField] private float interval;

    [SerializeField] private LayerMask enemyMask;

    private float timeToNextDamage;

    private void Update() {
        timeToNextDamage -= Time.deltaTime;
    }

    private void OnTriggerStay(Collider other) {
        if(timeToNextDamage > 0)
            return;

        if((enemyMask & (1 << other.gameObject.layer)) != 0) {
            IDamageable damageableEnemy = other.gameObject.GetComponent<IDamageable>();

            if(damageableEnemy != null) {
                damageableEnemy.TakeHealth(damage);
                timeToNextDamage = interval;
            }
        }
    }
}
