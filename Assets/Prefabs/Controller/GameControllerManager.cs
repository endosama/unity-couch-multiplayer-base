using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;
using System.Linq;


public struct PlayerControllerMapping
{
    public int ControlledId;
    public int PlayerId;
}

public static class GameControllerManager
{
    private const int KeyboardHash = 0;
    private static List<PlayerControllerMapping> _ControllerMapping;
    private static List<Controller> _PlayerControllers;
    private static List<InputDevice> _inputDevices;
    private static bool keyboardDetected = false;

    public static bool IsKeyboardHash(int deviceHash)
    {
        return deviceHash == KeyboardHash;
    }
    
    private static void Initialize()
    {

        if (_ControllerMapping == null)
        {
            _ControllerMapping = new List<PlayerControllerMapping>();
            _PlayerControllers = new List<Controller>();
            _inputDevices = InputManager.Devices.ToList();
        }
    }

    public static IEnumerable<int> GetNotConnectedControllerIds()
    {
        Initialize();
        return _inputDevices
            .Select(input => input.GetHashCode())
            .Where(input => !IsControllerAlreadyConnectedToPlayer(input));

    }

    public static IEnumerable<int> GetConnectedControllerIds()
    {
        return _ControllerMapping.Select(pcm => pcm.ControlledId);
    }

    public static bool IsControllerAlreadyConnectedToPlayer(int deviceHash)
    {
        return _ControllerMapping.Any(pc => pc.ControlledId == deviceHash);
    }

    public static void ControllerUnpluggedEvent(int deviceHash)
    {
        _inputDevices = InputManager.Devices.ToList();
        var playerConnectedToControlled = _ControllerMapping.Where(pc => pc.ControlledId == deviceHash).ToList();
        if (playerConnectedToControlled.Count > 0)
        {
            //Disconnect controller
            var playerId = playerConnectedToControlled[0].PlayerId;
            var playerController = _PlayerControllers.Find(pc => pc.GetInstanceID() == playerId);
            playerController.DisconnectController();
            _ControllerMapping.RemoveAll(pc => pc.PlayerId == playerId);

        }
    }

    public static void KeyboardPluggedEvent()
    {
        keyboardDetected = true;
        Initialize();
        var notConnectedPlayers = GetNotConnectedPlayers();
        if (notConnectedPlayers.Any())
        {
            var controller = notConnectedPlayers[0];
            var keyboardController = new KeyboardDevice();
            controller.ConnectController(keyboardController);
            var pcm = new PlayerControllerMapping
            {
                ControlledId = KeyboardHash,
                PlayerId = controller.GetInstanceID()
            };
            _ControllerMapping.Add(pcm);
        }
        else
        {
            Engine.instance.CreatePlayer();
        }
    }

    private static List<Controller> GetNotConnectedPlayers()
    {
        var connectedPlayerId = _ControllerMapping.Select(cm => cm.PlayerId);
        return _PlayerControllers.Where(pc => !connectedPlayerId.Contains(pc.GetInstanceID())).ToList();
    }

    public static void ControllerPluggedEvent(int deviceHash)
    {
        _inputDevices = InputManager.Devices.ToList();

        Initialize();
        if (!IsControllerAlreadyConnectedToPlayer(deviceHash))
        {
            var notConnectedPlayers = GetNotConnectedPlayers();
            if (notConnectedPlayers.Any())
            {
                var controller = notConnectedPlayers[0];
                var inputDevice = _inputDevices.First(i => i.GetHashCode() == deviceHash);
                var controllerDevice = new ControllerDevice(inputDevice);
                controller.ConnectController(controllerDevice);
                var pcm = new PlayerControllerMapping
                {
                    ControlledId = deviceHash, PlayerId = controller.GetInstanceID()
                };
                _ControllerMapping.Add(pcm);

            }
            else
            {
                Engine.instance.CreatePlayer();
            }
        }
    }

    public static IInputDevice GetInputDevice(Controller playerController)
    {
        var playerControllerHash = playerController.GetInstanceID();
        Initialize();
        

        foreach (var inputDevice in _inputDevices)
        {
            if (_ControllerMapping.Any(cm => cm.ControlledId == inputDevice.GetHashCode()))
            {
                continue;
            }
            _PlayerControllers.Add(playerController);
            var pcm =  new PlayerControllerMapping()
            {
                ControlledId = inputDevice.GetHashCode(),
                PlayerId = playerControllerHash

            };
            _ControllerMapping.Add(pcm);
            break;
        }

        _PlayerControllers.Add(playerController);
        if (_ControllerMapping.Any(pcm => pcm.PlayerId == playerControllerHash))
        {
            Debug.Log("Controller assigned to player: "+ playerControllerHash);
            var playerControllerMapping= _ControllerMapping.First(pcm => pcm.PlayerId== playerControllerHash);
            var inputDevice = _inputDevices.Find(input => input.GetHashCode() == playerControllerMapping.ControlledId);
            var controllerDevice = new ControllerDevice(inputDevice);
            return controllerDevice;
        }
        else
        {
            if (keyboardDetected && _ControllerMapping.All(pcm => pcm.ControlledId != 0))
            {
                var keyboardDevice = new KeyboardDevice();
                _PlayerControllers.Add(playerController);
                var pcm = new PlayerControllerMapping()
                {
                    ControlledId = KeyboardHash,
                    PlayerId = playerControllerHash

                };
                _ControllerMapping.Add(pcm);
                return keyboardDevice;
            }
            else
            {
                Debug.Log("No free controller for player: " + playerControllerHash);
                return null;
            }
        }
        
    }
}