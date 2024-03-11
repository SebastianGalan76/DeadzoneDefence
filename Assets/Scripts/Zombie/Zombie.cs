using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombie {
    public class Zombie : MonoBehaviour, IEnemy {
        [SerializeField] private float health;
        [SerializeField] private Transform centerPosition;

        [SerializeField] private GameObject bloodSplashPrefab;

        public void TakeHealth(float value) {
            health -= value;

            if(health <= 0) {
                Die();
            }

            GameObject bloodSplash = Instantiate(bloodSplashPrefab);
            bloodSplash.transform.position = GetCenterPosition();
            Destroy(bloodSplash, 10f);
        }

        public Vector3 GetCenterPosition() {
            return centerPosition.position;
        }

        private void Die() {
            Destroy(gameObject);
        }
    }
}
