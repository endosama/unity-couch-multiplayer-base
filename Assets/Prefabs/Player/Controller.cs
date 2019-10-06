using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public InputDevice inputDevice;
    public float movementSpeed = 1;
    public Color color;
    private Rigidbody m_Rigidbody;
    private GameObject marker;
    public void Start()
    {
        color = new Color(Random.value, Random.value, Random.value) ;
        m_Rigidbody = GetComponent<Rigidbody>();
        inputDevice  = GameControllerManager.GetInputDevice(this);
        marker = transform.Find("Marker").gameObject;
        marker.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", color);
        if (inputDevice != null)
        {
            marker.SetActive(true);
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
        var shoot = inputDevice.GetControl(InputControlType.RightTrigger);
        if (shoot.IsPressed)
        {
            Shoot();
        }
        var LpadX = inputDevice.GetControl(InputControlType.LeftStickX);
        var LpadY = inputDevice.GetControl(InputControlType.LeftStickY);
        if (LpadX.HasChanged || LpadY.HasChanged)
        {
            Move(new Vector2(LpadX.Value,LpadY.Value));
        }

        var RpadX = inputDevice.GetControl(InputControlType.RightStickX);
        var RpadY = inputDevice.GetControl(InputControlType.RightStickY);
        if (RpadX.HasChanged || RpadY.HasChanged)
        {
            Rotate(new Vector2(RpadX.Value, RpadY.Value));
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
        marker.SetActive(false);
    }
    public void ConnectController(InputDevice input)
    {
        inputDevice = input;
        marker.SetActive(true);
    }
}
