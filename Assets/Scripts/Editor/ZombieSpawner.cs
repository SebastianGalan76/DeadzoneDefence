using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace EditorHelper {
    [CustomEditor(typeof(Zombie.Spawner))]
    public class ZombieSpawner : Editor {
        private void OnSceneGUI() {
            Zombie.Spawner spawner = (Zombie.Spawner)target;

            Handles.color = Color.red;
            Handles.DrawWireArc(spawner.transform.position, Vector3.up, Vector3.forward, 360, spawner.getSpawnRadius(), 3f);
        }
    }
}