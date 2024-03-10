using UnityEngine;

public class PlaceObjectSystem : MonoBehaviour
{
    private static PlaceObjectSystem _instance;

    [SerializeField] private FieldManager fieldManager;

    private GameObject currentObject;
    private Unit currentUnit;
    private CollisionDetector collisionDetector;
    private IPlaceable placeable;

    private Ray ray;
    private RaycastHit hit;
    private new Camera camera;

    private void Awake() {
        _instance = this;

        camera = Camera.main;

        enabled = false;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            StopOperation();
        }

        ray = camera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, fieldManager.fieldMask)) {
            if(hit.collider == null) {
                currentObject.SetActive(false);
                return;
            }

            if(hit.collider.gameObject.layer == 6 || hit.collider.gameObject.layer == 9) {
                currentObject.SetActive(false);
                return;
            }

            currentObject.SetActive(true);
            currentObject.transform.position = hit.point;

            if(Input.GetMouseButtonDown(0)) {
                if(collisionDetector == null || !collisionDetector.IsCollide()) {
                    PlaceObject();
                } else {
                    Failed();
                }
            }
        } else {
            currentObject.SetActive(false);
        }
    }

    public void StartOperation(Unit unit) {
        Destroy(currentObject);

        currentUnit = unit;
        currentObject = Instantiate(unit.unitPrefab);
        currentObject.SetActive(false);

        fieldManager.ShowAllowedFields(unit.unitType);

        placeable = currentObject.GetComponent<IPlaceable>();
        placeable?.Initialize();

        collisionDetector = currentObject.GetComponent<CollisionDetector>();

        enabled = true;
    }

    public void StopOperation() {
        Destroy(currentObject);
        fieldManager.HideAllFields();
        enabled = false;
    }

    private void PlaceObject() {
        placeable?.AfterPlace();

        currentObject = null;
        StartOperation(currentUnit);
    }

    private void Failed() {
        fieldManager.MarkFieldsRed();
    }


    public static PlaceObjectSystem GetInstance() {
        return _instance;
    }
}