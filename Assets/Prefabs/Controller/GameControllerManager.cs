using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;
using System.Linq;
public static class GameControllerManager
{
    private static Dictionary<int, InputDevice> _ControllerMapping;
    private static List<InputDevice> _inputDevice;
    private static void Initialize()
    {
        _ControllerMapping = new Dictionary<int, InputDevice>();
        _inputDevice = InputManager.Devices.ToList();
    }

    public static InputDevice GetInputDevice(int instanceID)
    {
        if (_ControllerMapping == null)
        {
            Initialize();
        }

        foreach (var inputDevice in _inputDevice)
        {
            if (_ControllerMapping.ContainsValue(inputDevice))
            {
                continue;
            }
            _ControllerMapping.Add(instanceID, inputDevice);
            break;
        }

        if (_ControllerMapping.ContainsKey(instanceID))
        {
            Debug.Log("Controller assigned to player: "+instanceID);
            return _ControllerMapping[instanceID];
        }
        else
        {
            Debug.Log("No free controller for player: " + instanceID);
            return null;
        }
        
    }
}