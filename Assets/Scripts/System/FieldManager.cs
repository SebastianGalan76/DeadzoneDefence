using System.Collections;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    public LayerMask fieldMask;

    public PlaceableField[] fields;
    public Material redColor;
    public Material greenColor;

    public void ShowAllowedFields(UnitType unitType) {
        foreach(PlaceableField field in fields) {
            if(field.IsAllowed(unitType)) {
                field.ChangeColor(greenColor);
                field.Show();
            } else {
                field.Hide();
            }
        }
    }
    public void MarkFieldsRed() {
        StopAllCoroutines();

        foreach(PlaceableField field in fields) {
            field.ChangeColor(redColor);
        }

        StartCoroutine(wait());

        IEnumerator wait() {
            yield return new WaitForSeconds(0.25f);
            
            foreach(PlaceableField field in fields) {
                field.ChangeColor(greenColor);
            }
        }
    }
    public void HideAllFields() {
        foreach(PlaceableField field in fields) {
            field.Hide();
        }
    }
}
