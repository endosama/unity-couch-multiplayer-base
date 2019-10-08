using UnityEngine;

namespace Assets.Prefabs.Player
{
    public class Health : MonoBehaviour
    {

        private float maxHealth = 1;

        public float currentHealth = 1;

        public void AddDamage(float amount)
        {
            currentHealth -= amount;
        }
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }


    }
}
