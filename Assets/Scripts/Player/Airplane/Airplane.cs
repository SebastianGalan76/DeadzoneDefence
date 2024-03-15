using System;
using UnityEngine;

public class Airplane : MonoBehaviour
{
    [Header("Visualization")]
    public DestinationVisualization visualization;

    [Header("Airplane")]
    public GameObject airplaneObj;

    //Time to reach the destination after placing the aircraft
    [SerializeField] private float timeToDestination = 1f;

    //Calculated speed of the airplane required to reach the destination in #timeToDestination
    protected float speed;

    private Vector3 startPosition;
    private Vector3 destinationPosition;

    private float airplaneYPos;
    private float elapsedTime;
    private bool reachedDestination;

    private void Start() {
        enabled = false;
        reachedDestination = false;

        visualization.CreateVisualization();
    }

    private void Update() {
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / timeToDestination);

        if(t < 1) {
            transform.position = Vector3.Lerp(startPosition, destinationPosition, t);
        } else {
            if(!reachedDestination) {
                OnDestination();
            }

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    public bool AfterPlace() {
        AirplaneSystem airplaneSystem = AirplaneSystem.GetInstance();
        startPosition = GetStartPosition(airplaneSystem);

        destinationPosition = visualization.GetDestinationPosition(startPosition, airplaneYPos);
        visualization.AfterPlace();

        speed = Vector3.Distance(startPosition, destinationPosition) / timeToDestination;

        airplaneObj.SetActive(true);
        transform.position = startPosition;
        transform.LookAt(destinationPosition);

        enabled = true;
        return false;
    }

    Vector3 GetStartPosition(AirplaneSystem airplaneSystem) {
        Vector3 camera = airplaneSystem.cameraCenter.position + new Vector3(0, 0, -50f);

        //Vector3 camera = new Vector3(Camera.main.transform.position.x, airplaneSystem.airplaneYPosition, Camera.main.transform.position.z);
        Vector3[] startPositions = airplaneSystem.CalculatePositions(visualization.borders);
        airplaneYPos = startPositions[0].y;

        if(Vector3.Distance(camera, startPositions[0]) > Vector3.Distance(camera, startPositions[1])) {
            return startPositions[1];
        }

        return startPositions[0];
    }

    protected virtual void OnDestination() {
        reachedDestination = true;
        Destroy(gameObject, 15f);
    }

    [System.Serializable]
    public struct DestinationVisualization {
        public Transform handle;

        public Transform[] borders;
        public GameObject targetPrefab;
        public float length;

        public readonly void AfterPlace() {
            Destroy(handle.gameObject);
        }
        public readonly void CreateVisualization() {
            borders[0].transform.position += new Vector3(0, 0, length / 2);
            borders[1].transform.position -= new Vector3(0, 0, length / 2);

            float size = 3f;
            int amount = Convert.ToInt32(length / size) + 1;
            GameObject targetGO;

            if(amount % 2 != 0) {
                targetGO = Instantiate(targetPrefab, handle);
                targetGO.transform.position = handle.position;

                amount--;
                amount /= 2;

                for(int i = 0;i < amount;i += 1) {
                    targetGO = Instantiate(targetPrefab, handle);
                    targetGO.transform.position = handle.position + new Vector3(0, 0, (i + 1) * size);

                    targetGO = Instantiate(targetPrefab, handle);
                    targetGO.transform.position = handle.position - new Vector3(0, 0, (i + 1) * size);
                }
            } else {
                amount /= 2;

                for(int i = 0;i < amount;i += 1) {
                    targetGO = Instantiate(targetPrefab, handle);
                    targetGO.transform.position = handle.position + new Vector3(0, 0, (i * size) + (size / 2));

                    targetGO = Instantiate(targetPrefab, handle);
                    targetGO.transform.position = handle.position - new Vector3(0, 0, (i * size) + (size / 2));
                }
            }
        }
        public readonly Vector3 GetDestinationPosition(Vector3 startPosition, float yPos) {
            if(Vector3.Distance(startPosition, borders[0].position) > Vector3.Distance(startPosition, borders[1].position)) {
                return new Vector3(borders[1].position.x, yPos, borders[1].position.z);
            }
            return new Vector3(borders[0].position.x, yPos, borders[0].position.z);
        }
    }
}
