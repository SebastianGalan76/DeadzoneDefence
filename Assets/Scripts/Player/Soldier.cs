using UnityEngine;

public class Soldier : MonoBehaviour, IDamageable, IPlaceable {
    [SerializeField] private float health;

    [SerializeField] private Transform character;
    [SerializeField] private Weapon weapon;

    private FieldOfView fov;

    private void Awake() {
        fov = GetComponent<FieldOfView>();
        weapon.Initialize(character, fov);
    }

    public void TakeHealth(float value) {
        health -= value;

        if(health < 0) {
            Die();
        }
    }

    public bool AfterPlace() {
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<CollisionDetector>());

        gameObject.layer = LayerMask.NameToLayer("Player");
        weapon.enabled = true;

        fov.Hide();
        return true;
    }

    public void Initialize() {
        
    }

    private void Die() {
        Destroy(gameObject);
    }

    public Vector3 GetCenterPosition() {
        return character.position;
    }
}
