using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InControl;
using UnityEngine;

public interface IInputDevice
{
    bool IsShooting();
    Vector2 GetMovement();
    Vector2 GetRotation();
}

public class ControllerDevice : IInputDevice
{
    public InputDevice inputDevice;

    public ControllerDevice(InputDevice inputDevice)
    {
        this.inputDevice = inputDevice;
    }
    public Vector2 GetMovement()
    {
        var lpadX = inputDevice.GetControl(InputControlType.LeftStickX);
        var lpadY = inputDevice.GetControl(InputControlType.LeftStickY);
        if (lpadX.HasChanged || lpadY.HasChanged)
        {
            return new Vector2(lpadX.Value, lpadY.Value);
        }
        else
        {
            return Vector2.zero;
        }
    }

    public Vector2 GetRotation()
    {
        var rpadX = inputDevice.GetControl(InputControlType.RightStickX);
        var rpadY = inputDevice.GetControl(InputControlType.RightStickY);
        if (rpadX.HasChanged || rpadY.HasChanged)
        {
            return new Vector2(rpadX.Value, rpadY.Value);
        }
        else
        {
            return Vector2.zero;
        }
    }

    public bool IsShooting()
    {
        var shoot = inputDevice.GetControl(InputControlType.RightTrigger);
        return shoot.IsPressed;
    }
}

public class KeyboardDevice : IInputDevice
{
    public KeyboardDevice()
    {}
    public Vector2 GetMovement()
    {
        var lpadX = Input.GetAxis("Horizontal");
        var lpadY = Input.GetAxis("Vertical");
        if (lpadX != 0 || lpadY != 0)
        {
            return new Vector2(lpadX, lpadY);
        }
        else
        {
            return Vector2.zero;
        }
    }

    public Vector2 GetRotation()
    {
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");
        if (h > 0|| v > 0)
        {
            return new Vector2(v,h);
        }
        else
        {
            return Vector2.zero;
        }
    }

    public bool IsShooting()
    {
        return Input.GetButton("Fire1");
    }
}
