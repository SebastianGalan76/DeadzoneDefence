using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUnit : MonoBehaviour {
    [SerializeField] private Unit unit;

    private void Awake() {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void OnClick() {
        PlaceObjectSystem.GetInstance().StartOperation(unit);
    }
}
