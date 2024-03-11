using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    private IDamageable damageableTarget;
    private float damage;
    private float speed;

    public void Send(Transform target, IDamageable damageableTarget, float damage, float speed) {
        this.target = target;
        this.damageableTarget = damageableTarget;
        this.damage = damage;
        this.speed = speed;
    }

    private void Update() {
        if(target == null) {
            Destroy(gameObject);
            return;
        }

        Vector3 centerPosition = damageableTarget.GetCenterPosition();
        transform.position = Vector3.MoveTowards(transform.position, centerPosition, speed * Time.deltaTime);

        if((centerPosition - transform.position).sqrMagnitude < 1) {
            damageableTarget.TakeHealth(damage);

            Destroy(gameObject);
        }
    }
}
