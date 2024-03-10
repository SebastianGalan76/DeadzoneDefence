using UnityEditor;
using UnityEngine;

namespace EditorHelper {
    [CustomEditor(typeof(Zombie.Movement))]
    public class ZombieMovement : Editor {
        private void OnSceneGUI() {
            Zombie.Movement movement = (Zombie.Movement)target;

            Handles.color = Color.gray;
            Handles.DrawWireArc(movement.transform.position, Vector3.up, Vector3.forward, 360, movement.GetViewRadius(), 3f);
        }
    }
}