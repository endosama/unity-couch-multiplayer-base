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
    private static List<PlayerControllerMapping> _ControllerMapping;
    private static List<Controller> _PlayerControllers;
    private static List<InputDevice> _inputDevices;

    private static void Initialize()
    {
        _ControllerMapping = new List<PlayerControllerMapping>();
        _PlayerControllers = new List<Controller>();
        _inputDevices = InputManager.Devices.ToList();
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
    
    public static void ControllerPluggedEvent(int deviceHash)
    {
        _inputDevices = InputManager.Devices.ToList();

        if (_ControllerMapping == null)
        {
            Initialize();
        }

        if (!IsControllerAlreadyConnectedToPlayer(deviceHash))
        {
            var connectedPlayerId = _ControllerMapping.Select(cm => cm.PlayerId);
            var notConnectedPlayers = _PlayerControllers.Where(pc => !connectedPlayerId.Contains(pc.GetInstanceID())).ToArray();
            if (notConnectedPlayers.Any())
            {
                var controller = notConnectedPlayers[0];
                var inputDevice = _inputDevices.First(i => i.GetHashCode() == deviceHash);
                controller.ConnectController(inputDevice);
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

    public static InputDevice GetInputDevice(Controller playerController)
    {
        var playerControllerHash = playerController.GetInstanceID();
        if (_ControllerMapping == null)
        {
            Initialize();
        }

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
            return _inputDevices.Find(input => input.GetHashCode() == playerControllerMapping.ControlledId);
        }
        else
        {
            Debug.Log("No free controller for player: " + playerControllerHash);
            return null;
        }
        
    }
}