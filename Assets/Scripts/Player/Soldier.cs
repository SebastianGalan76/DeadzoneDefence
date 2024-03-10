using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour, IDamageable {
    [SerializeField] private float health;

    public void TakeHealth(float value) {
        health -= value;

        if(health < 0) {
            Die();
        }
    }

    private void Die() {
        Destroy(gameObject);
    }
}
