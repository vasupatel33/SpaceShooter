using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] ParticleSystem particleSystem;
    [SerializeField] Color DefaultColor;

    private void Start()
    {
        Debug.Log("start");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");
        if(collision.gameObject.tag == "enemy")
        {
            collision.transform.GetComponent<EnemyHandling>().Health--;

            if (collision.transform.GetComponent<EnemyHandling>().Health == 0)
            {
                Debug.Log("Enemy Destroyed");
                GameObject particle = Instantiate(particleSystem.gameObject, collision.transform.position, Quaternion.identity);
                Destroy(particle.gameObject, 2f);
                Destroy(collision.gameObject);
                Destroy(this.gameObject);
            }
            else
            {
                Debug.Log("else called = " + collision.transform.GetComponent<EnemyHandling>().Health);
                collision.transform.GetComponent<SpriteRenderer>().color = collision.transform.GetComponent<EnemyHandling>().HitColor;
                StartCoroutine(ColorReset(collision.gameObject));
                Destroy(this.gameObject);
            }

        }
    }
    IEnumerator ColorReset(GameObject collosionObject)
    {
        yield return new WaitForSeconds(0.1f);
        collosionObject.transform.GetComponent<SpriteRenderer>().color = DefaultColor;
        Debug.Log("Color Reset");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger");
    }
}
