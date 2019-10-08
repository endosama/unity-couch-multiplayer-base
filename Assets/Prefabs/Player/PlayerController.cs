using Assets.Prefabs.Controller;
using UnityEngine;

namespace Assets.Prefabs.Player
{
    public class PlayerController : MonoBehaviour
    {
        public IInputDevice inputDevice;
        public float movementSpeed = 1;
        public Color color;
        private Rigidbody m_Rigidbody;
        private GameObject m_Marker;

        public void Start()
        {
            color = new Color(Random.value, Random.value, Random.value);
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Marker = transform.Find("Marker").gameObject;
            m_Marker.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", color);
            inputDevice = GameControllerManager.GetInputDevice(this);
            if (inputDevice != null)
            {
                m_Marker.SetActive(true);
            }
        }

        private void Shoot()
        {
            var gunBehaviour = GetComponentInChildren<GunBehaviour>();
            if (gunBehaviour != null)
            {
                gunBehaviour.Shoot();
            }
        }

        private void Move(Vector2 movement)
        {
            Debug.Log("Move" + movement);
            var m = movement * Time.deltaTime * movementSpeed * 100f;
            m_Rigidbody.velocity = new Vector3(m.x, 0 , m.y);
        }

        private void Rotate(Vector2 movement)
        {
            if (movement.magnitude == 0) {return;}
            Debug.Log("Rotate" + movement);
            var normalized = movement.normalized;
            var direction = new Vector3(normalized.x, 0, normalized.y);
            var rot = transform.rotation;
            rot.SetLookRotation(direction, Vector3.up);
            m_Rigidbody.MoveRotation(rot);
        }

        private void GetInputs()
        {
            if (inputDevice.IsShooting())
            {
                Shoot();
            }

            var movement = inputDevice.GetMovement();
            if (movement.magnitude > 0)
            {
                Move(movement);
            }

            var rotation = inputDevice.GetRotation();
            if (rotation.magnitude > 0)
            {
                Rotate(rotation);
            }
        }

        public void Update()
        {
            if (inputDevice != null)
            {
                GetInputs();
            }
        }


        public void DisconnectController()
        {
            inputDevice = null;
            m_Marker.SetActive(false);
        }
        public void ConnectController(IInputDevice input)
        {
            inputDevice = input;
            m_Marker.SetActive(true);
        }
    }
}
