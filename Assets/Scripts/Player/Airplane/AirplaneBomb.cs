using UnityEngine;

public class AirplaneBomb : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float damage;
    [SerializeField] private float firingRange;

    [SerializeField] private GameObject explodeVFXPrefab;

    private float targetYPos;
    private float speed = 15f;

    public void Update() {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        if(transform.position.y <= targetYPos) {
            Explode(transform.position + Vector3.up);
        }
    }

    public void Drop(float targetYPos) {
        this.targetYPos = targetYPos;

        enabled = true;
    }

    private void Explode(Vector3 position) {
        enabled = false;

        GameObject vfx = Instantiate(explodeVFXPrefab);
        vfx.transform.position = position;
        Destroy(vfx, 2);

        Collider[] hits = Physics.OverlapSphere(position, firingRange);

        foreach(Collider hit in hits) {
            IDamageable damegeableObject = hit.GetComponent<IDamageable>();

            if(damegeableObject != null) {
                damegeableObject.TakeHealth(damage);
            }
        }

        Destroy(gameObject);
    }
}
