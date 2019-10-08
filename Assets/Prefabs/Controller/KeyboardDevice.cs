using UnityEngine;

namespace Assets.Prefabs.Controller
{
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
}
