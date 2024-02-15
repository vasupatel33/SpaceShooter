    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] ParticleSystem _particle;
    private void Start()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if(collision.gameObject.tag == "enemy")
        //{
        //    GameObject particleObj = Instantiate(_particle.gameObject, collision.contacts[0].point, Quaternion.identity);
        //    Destroy(particleObj, 1.5f);
        //}
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger");
    }
}