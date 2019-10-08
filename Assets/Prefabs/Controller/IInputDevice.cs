using UnityEngine;

public interface IInputDevice
{
    bool IsShooting();
    Vector2 GetMovement();
    Vector2 GetRotation();
}