using UnityEngine;

public class PlaceableField : MonoBehaviour
{
    [SerializeField] private UnitType[] allowedUnitTypes;

    private MeshRenderer meshRenderer;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Show() {
        gameObject.SetActive(true);
    }
    public void Hide() {
        gameObject.SetActive(false);
    }
    public void ChangeColor(Material material) {
        meshRenderer.material = material;
    }
    public bool IsAllowed(UnitType objectType) {
        foreach(UnitType type in allowedUnitTypes) {
            if(objectType == type) {
                return true;
            }
        }

        return false;
    }
}
