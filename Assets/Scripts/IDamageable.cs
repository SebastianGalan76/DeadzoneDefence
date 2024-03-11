using UnityEngine;

public interface IDamageable
{
    void TakeHealth(float value);
    Vector3 GetCenterPosition();
}
