using UnityEngine;

namespace Zombie {
    public class Spawner : MonoBehaviour {
        [SerializeField] private GameObject zombiePrefab;
        [SerializeField] private float spawnInterval;
        [SerializeField] private float spawnRadius;

        private float timeToNextSpawn;

        private void Update() {
            timeToNextSpawn -= Time.deltaTime;

            if(timeToNextSpawn <= 0) {
                SpawnZombie();
                timeToNextSpawn = spawnInterval;
            }
        }

        public float GetSpawnRadius() {
            return spawnRadius;
        }

        private void SpawnZombie() {
            if(zombiePrefab == null) {
                return;
            }

            Instantiate(zombiePrefab, getRandomPosition(), Quaternion.identity);
        }
        private Vector3 getRandomPosition() {
            return transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, Random.Range(-spawnRadius, spawnRadius));
        }
    }
}
