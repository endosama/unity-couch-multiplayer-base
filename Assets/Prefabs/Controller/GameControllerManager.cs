using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Prefabs.Player;
using InControl;
using UnityEngine;

namespace Assets.Prefabs.Controller
{
    public struct PlayerControllerMapping
    {
        public int controlledId;
        public int playerId;
    }

    public static class GameControllerManager
    {
        private const int KeyboardHash = 0;
        private static List<PlayerControllerMapping> _controllerMapping;
        private static List<PlayerController> _playerControllers;
        private static List<InputDevice> _inputDevices;
        private static bool _keyboardDetected = false;

        public static bool IsKeyboardHash(int deviceHash)
        {
            return deviceHash == KeyboardHash;
        }
    
        private static void Initialize()
        {

            if (_controllerMapping == null)
            {
                _controllerMapping = new List<PlayerControllerMapping>();
                _playerControllers = new List<global::Assets.Prefabs.Player.PlayerController>();
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
            return _controllerMapping.Select(pcm => pcm.controlledId);
        }

        public static bool IsControllerAlreadyConnectedToPlayer(int deviceHash)
        {
            return _controllerMapping.Any(pc => pc.controlledId == deviceHash);
        }

        public static void ControllerUnpluggedEvent(int deviceHash)
        {
            _inputDevices = InputManager.Devices.ToList();
            var playerConnectedToControlled = _controllerMapping.Where(pc => pc.controlledId == deviceHash).ToList();
            if (playerConnectedToControlled.Count > 0)
            {
                //Disconnect controller
                var playerId = playerConnectedToControlled[0].playerId;
                var playerController = _playerControllers.Find(pc => pc.GetInstanceID() == playerId);
                playerController.DisconnectController();
                _controllerMapping.RemoveAll(pc => pc.playerId == playerId);

            }
        }

        private static void ConnectDeviceToPlayer(PlayerController controller, IInputDevice device, int deviceHash)
        {
            controller.ConnectController(device);
            var pcm = new PlayerControllerMapping
            {
                controlledId = deviceHash,
                playerId = controller.GetInstanceID()
            };
            _controllerMapping.Add(pcm);
            Debug.Log("Controller assigned to player: " + controller.GetInstanceID());
        }

        private static bool TryConnectionToPlayer(int deviceHash, IInputDevice device)
        {
            Initialize();
            var notConnectedPlayers = GetNotConnectedPlayers();
            if (notConnectedPlayers.Any())
            {
                var controller = notConnectedPlayers[0];
                ConnectDeviceToPlayer(controller, device, deviceHash);
                return true;
            }
            else
            {
                Engine.instance.CreatePlayer();
            }
            return false;
        }
        public static void KeyboardPluggedEvent()
        {
            _keyboardDetected = true;
            var keyboardController = new KeyboardDevice();
            TryConnectionToPlayer(KeyboardHash, keyboardController);
        }

        private static List<global::Assets.Prefabs.Player.PlayerController> GetNotConnectedPlayers()
        {
            var connectedPlayerId = _controllerMapping.Select(cm => cm.playerId);
            return _playerControllers.Where(pc => !connectedPlayerId.Contains(pc.GetInstanceID())).ToList();
        }

        public static void ControllerPluggedEvent(int deviceHash)
        {
            _inputDevices = InputManager.Devices.ToList();

            Initialize();
            if (IsControllerAlreadyConnectedToPlayer(deviceHash)) return;
            var inputDevice = _inputDevices.First(i => i.GetHashCode() == deviceHash);
            var controllerDevice = new ControllerDevice(inputDevice);
            TryConnectionToPlayer(deviceHash, controllerDevice);
        }

        public static IInputDevice GetInputDevice(PlayerController playerController)
        {
            Initialize();
            var playerControllerHash = playerController.GetInstanceID();
            if (!_playerControllers.Contains(playerController))
            {
                _playerControllers.Add(playerController);
            }

            foreach (var inputDevice in _inputDevices)
            {
                if (_controllerMapping.Any(cm => cm.controlledId == inputDevice.GetHashCode()))
                {
                    continue;
                }
                var controllerDevice = new ControllerDevice(inputDevice);
                ConnectDeviceToPlayer(playerController, controllerDevice, inputDevice.GetHashCode());
                return controllerDevice;
            }

            if (_keyboardDetected && _controllerMapping.All(pcm => pcm.controlledId != 0))
            {
                var keyboardDevice = new KeyboardDevice();
                ConnectDeviceToPlayer(playerController, keyboardDevice, KeyboardHash);
                return keyboardDevice;
            }
            
            Debug.Log("No free controller for player: " + playerControllerHash);
            return null;
        }
    }
}