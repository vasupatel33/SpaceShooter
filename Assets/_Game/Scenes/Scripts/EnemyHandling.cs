using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandling : MonoBehaviour
{
    public int Health = 10;
    public Color HitColor;

    [SerializeField] ParticleSystem Destroytparticle, collisionParticle;
    [SerializeField] Color DefaultColor;

    public static EnemyHandling Instance;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        DefaultColor = this.transform.GetComponent<SpriteRenderer>().color;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");
        if (collision.gameObject.tag == "bullet")
        {
            Health--;

            if (Health == 0)
            {
                Debug.Log("Enemy Destroyed");
                GameObject _particle = Instantiate(Destroytparticle.gameObject, collision.transform.position, Quaternion.identity);
                Debug.Log("Particle instantiated at: " + this.transform.position);
                Destroy(collision.gameObject);
                Destroy(this.gameObject);
            }
            else
            {
                this.transform.GetComponent<SpriteRenderer>().color = HitColor;

                GameObject _particle = Instantiate(collisionParticle.gameObject, collision.transform.position, Quaternion.identity);
                Debug.Log("Particle instantiated at contact point: " + collision.contacts[0].point);
                Destroy(_particle, 1.5f);
                Debug.Log("particle played");
                StartCoroutine(ColorReset());
                Destroy(collision.gameObject);
            }
        }
    }

    IEnumerator ColorReset()
    {
        yield return new WaitForSeconds(0.2f);
        this.transform.GetComponent<SpriteRenderer>().color = DefaultColor;
        Debug.Log("Color Reset");
    }
}