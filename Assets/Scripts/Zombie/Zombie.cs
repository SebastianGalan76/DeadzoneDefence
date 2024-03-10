using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombie {
    public class Zombie : MonoBehaviour, IEnemy {
        [SerializeField] private float health;

        private Movement movement;

        private void Awake() {
            movement = GetComponent<Movement>();

        }

        public void TakeHealth(float value) {
            health -= value;

            if(health <= 0) {
                Die();
            }
        }

        private void Die() {
            movement.Die();
        }
    }
}
