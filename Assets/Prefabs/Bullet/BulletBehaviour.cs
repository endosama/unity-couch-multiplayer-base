using Assets.Prefabs.Player;
using UnityEngine;

namespace Assets.Prefabs.Bullet
{
    public class BulletBehaviour : MonoBehaviour
    {
        private Rigidbody m_Rigidbody;
        public float Speed = 1;
        // Start is called before the first frame update
        void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            //m_Rigidbody.MovePosition(transform.forward * Speed * Time.deltaTime);
        }

        void OnCollisionEnter(Collision collision)
        {
            var health = collision.collider.GetComponent<Health>();
            if (health != null)
            {
                health.AddDamage(0.1f);
                collision.collider.attachedRigidbody.AddForce(m_Rigidbody.velocity.normalized * 5000);
            }
            GameObject.Destroy(this.gameObject);
        }
    }
}
