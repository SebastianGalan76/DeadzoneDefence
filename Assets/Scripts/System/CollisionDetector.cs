using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour {
    private int collision = 0;

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Decoration" || other.gameObject.tag == "Zombie") {
            collision++;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Decoration" || other.gameObject.tag == "Zombie") {
            collision--;
        }
    }

    private void OnEnable() {
        collision = 0;
    }

    public bool IsCollide() {
        return collision > 0;
    }
}
