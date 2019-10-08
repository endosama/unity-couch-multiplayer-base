using InControl;
using UnityEngine;

namespace Assets.Prefabs.Controller
{
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
}

