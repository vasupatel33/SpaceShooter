using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] ParticleSystem particleSystem;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");
        if(collision.gameObject.tag == "enemy")
        {
            GameObject particle = Instantiate(particleSystem.gameObject, collision.transform.position, Quaternion.identity);
            Destroy(particle.gameObject,2f);
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger");
    }
}
