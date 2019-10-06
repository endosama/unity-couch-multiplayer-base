using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InControl;
using UnityEngine;

public class ControllerPlugDetector : MonoBehaviour
{
    private List<int> connectedControllers;

    void Start()
    {
        connectedControllers = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        var devices = InputManager.Devices.ToList();
        var deviceHashes = devices.Select(device => device.GetHashCode());

        var newDevices = deviceHashes.Except(connectedControllers);
        var removedDevices = connectedControllers.Except(deviceHashes);

        foreach (var deviceHash in newDevices)
        {
            GameControllerManager.ControllerPluggedEvent(deviceHash);
        }

        foreach (var deviceHash in removedDevices)
        {
            if (GameControllerManager.IsControllerAlreadyConnectedToPlayer(deviceHash))
            {
                GameControllerManager.ControllerUnpluggedEvent(deviceHash);
            }
        }
    }
}
