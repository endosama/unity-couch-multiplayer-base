using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehaviour : MonoBehaviour
{
    private float m_LastShotTime;
    private float DeltaBetweenShots = 0.2f;
    private Transform m_BulletHole;
    public GameObject bullet;
    

    void Start()
    {
        m_BulletHole = transform.Find("BulletHole");
        m_LastShotTime = Time.time;
    }

    // Update is called once per frame
    public void Shoot()
    {
        if (Time.time - m_LastShotTime > DeltaBetweenShots)
        {
            Debug.Log("Shooting");
            m_LastShotTime = Time.time;
            var player = GameObject.FindGameObjectWithTag("Player");
            var bulletInstance = Instantiate(bullet, m_BulletHole.position, player.transform.rotation) as GameObject;
            var bulletRigidbody = bulletInstance.GetComponent<Rigidbody>();
            var bulletVelocity = transform.TransformDirection(-Vector3.left * 10);
            bulletVelocity.y = 0;
            bulletRigidbody.velocity = bulletVelocity;
        }
    }
}
