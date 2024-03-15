using Unity.VisualScripting;
using UnityEngine;

public class Bomber : Airplane, IPlaceable
{
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private int bombAmount;

    private float targetYPos;

    public void Initialize() {

    }

    protected override void OnDestination() {
        base.OnDestination();
        float dropRepeatSpeed = (visualization.length / (bombAmount - 1)) / speed;
        InvokeRepeating("DropBomb", 0f, dropRepeatSpeed);
    }

    private void DropBomb() {
        bombAmount--;

        GameObject bomb = Instantiate(bombPrefab);

        bomb.transform.position = transform.position;
        bomb.GetComponent<AirplaneBomb>().Drop(targetYPos);

        if(bombAmount <= 0) {
            float randomDirection = Random.Range(0, 100);

            if(randomDirection < 50) {
                transform.Rotate(0f, Random.Range(15f, 25f) * -1, 0f);
            } else {
                transform.Rotate(0f, Random.Range(15f, 25f), 0f);
            }

            CancelInvoke("DropBomb");
        }
    }
}
