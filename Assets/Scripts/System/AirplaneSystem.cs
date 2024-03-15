using System.Collections.Generic;
using UnityEngine;

public class AirplaneSystem : MonoBehaviour
{
    public float airplaneYPosMin = 15f;
    public float airplaneYPosMax = 25f;

    private static AirplaneSystem _instance;

    public Transform cameraCenter;
    [SerializeField] private float radius;

    private void Awake() {
        _instance = this;
    }

    //Obliczanie pozycji startu samolotu na podstawie równania prostej przechodzącej przez dwa punkty A(a,b) B(c,d)
    //równania okręgu o środku w punkcie (A,B) i promieniu R.
    public Vector3[] CalculatePositions(Transform[] visualizationBorders) {
        Vector3 start = visualizationBorders[0].position;
        Vector3 end = visualizationBorders[1].position;

        Vector3[] positions = new Vector3[2];
        float g, f, h, i;

        h = calculateH();

        List<float> xPositions;

        if(start.x == end.x) {
            start.x += 0.01f;
        }

        g = calculateG();
        f = calculateF();
        i = calculateI();

        xPositions = SolveQuadraticEquation(Mathf.Pow(g, 2) + 1, 2 * g * f - 2 * cameraCenter.position.z * g - 2 * cameraCenter.position.x, i);

        if(xPositions.Count == 2) {
            float yPos = Random.Range(airplaneYPosMin, airplaneYPosMax);
            positions[0] = new Vector3(xPositions[0], yPos, g * xPositions[0] + f);
            positions[1] = new Vector3(xPositions[1], yPos, g * xPositions[1] + f);
        }

        return positions;

        float calculateG() {
            return (start.z - end.z) / (start.x - end.x);
        }
        float calculateF() {
            return start.z - (((start.z - end.z) / (start.x - end.x)) * start.x);
        }
        float calculateH() {
            return Mathf.Pow(cameraCenter.position.x, 2) + Mathf.Pow(cameraCenter.position.z, 2) - Mathf.Pow(radius, 2);
        }
        float calculateI() {
            return Mathf.Pow(f, 2) - (2 * cameraCenter.position.z * f) + h;
        }
    }
    private List<float> SolveQuadraticEquation(float a, float b, float c) {
        List<float> solution = new List<float>();
        float delta = b * b - 4 * a * c;

        float x;

        if(delta < 0) {
            solution.Add(0);
        } else if(delta == 0) {
            x = -b / (2 * a);
            solution.Add(x);
        } else {
            x = (-b + Mathf.Sqrt(delta)) / (2f * a);
            solution.Add(x);

            x = (-b - Mathf.Sqrt(delta)) / (2f * a);
            solution.Add(x);
        }

        return solution;
    }

    public static AirplaneSystem GetInstance() {
        return _instance;
    }
}
