using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour, IDamageable, IPlaceable {
    [SerializeField] private float health;

    private FieldOfView fov;

    private void Awake() {
        fov = GetComponent<FieldOfView>();
    }

    public void TakeHealth(float value) {
        health -= value;

        if(health < 0) {
            Die();
        }
    }

    public void AfterPlace() {
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<CollisionDetector>());

        gameObject.layer = LayerMask.NameToLayer("Player");

        fov.Hide();
    }

    public void Initialize() {
        
    }

    private void Die() {
        Destroy(gameObject);
    }
}
