using System.Linq;
using InControl;
using UnityEngine;

public class ControllerPlugDetector : MonoBehaviour
{
    private bool keyboardDetected = false;
    void Update()
    {

        if (!keyboardDetected)
        {
            if (Input.anyKey)
            {
                keyboardDetected = true;
                GameControllerManager.KeyboardPluggedEvent();
            }
        }
        var devices = InputManager.Devices.ToList();
        var connectedControllers = GameControllerManager.GetConnectedControllerIds().ToList();
        var deviceHashes = devices.Select(device => device.GetHashCode()).ToArray();

        var newDevices = deviceHashes.Except(connectedControllers);
        var removedDevices = connectedControllers.Except(deviceHashes);

        foreach (var deviceHash in newDevices)
        {
            GameControllerManager.ControllerPluggedEvent(deviceHash);
        }

        foreach (var deviceHash in removedDevices)
        {
            if (!GameControllerManager.IsKeyboardHash(deviceHash))
            {
                if (GameControllerManager.IsControllerAlreadyConnectedToPlayer(deviceHash))
                {
                    GameControllerManager.ControllerUnpluggedEvent(deviceHash);
                }
            }
        }
    }
}
