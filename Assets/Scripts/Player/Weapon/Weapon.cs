using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float fireDelay;

    [SerializeField] private Transform gunBarrel;
    [SerializeField] private GameObject bulletPrefab;

    private float time;
    private FieldOfView fov;
    private Transform character;

    private void Awake() {
        enabled = false;
    }

    public void Initialize(Transform character, FieldOfView fov) {
        this.character = character;
        this.fov = fov;
    }

    private void Update() {
        time -= Time.deltaTime;
        if(time <= 0) {
            Shoot();
            time = fireDelay;
        }
    }

    private void Shoot() {
        Transform closestEnemy = fov.GetClosestEnemy();
        if(closestEnemy == null ) {
            return;
        }

        IDamageable damageableEnemy = closestEnemy.GetComponent<IDamageable>();
        if( damageableEnemy == null ) {
            return;
        }

        //Rotate towards the closest target
        Vector3 directionToTarget = closestEnemy.position - transform.position;
        directionToTarget.y = 0;
        character.rotation = Quaternion.LookRotation(directionToTarget);

        GameObject bulletObj = Instantiate(bulletPrefab);
        bulletObj.transform.position = gunBarrel.position;

        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.Send(closestEnemy, damageableEnemy, damage, 120f);
    }
}
