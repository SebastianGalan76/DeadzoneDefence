using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour, IDamageable, IPlaceable {
    [SerializeField] private float health;

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
    }

    public void Initialize() {
        
    }

    private void Die() {
        Destroy(gameObject);
    }
}
