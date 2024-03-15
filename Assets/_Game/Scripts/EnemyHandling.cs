using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandling : MonoBehaviour
{
    public int Health = 10;
    public Color HitColor;

    [SerializeField] ParticleSystem Destroytparticle, collisionParticle;
    [SerializeField] Color DefaultColor;
    [SerializeField] GameObject FirePrefab;

    public static EnemyHandling Instance;
    public bool isObjSpecial, isObjFire;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        DefaultColor = this.transform.GetComponent<SpriteRenderer>().color;
        StartCoroutine(CheckingForFire());
    }
    IEnumerator CheckingForFire()
    {
        yield return new WaitForSeconds(4f);
        if (isObjFire) 
        {
            StartCoroutine(RandomFire());
        }
    }
    IEnumerator RandomFire()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3, 10));
            GameManager.Instance.EnemyFireBullet(this.gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            Health--;

            if (Health == 0)
            {
                GameObject _particle = Instantiate(Destroytparticle.gameObject, collision.contacts[0].point, Quaternion.identity);
                
                if(isObjSpecial == true)
                {
                    Debug.Log("Special obj Destroyed" + collision.gameObject.transform.position);
                    if (this.gameObject != null)
                    {
                        GenerateSpecialPowerEnemy(collision.transform.position);
                    }
                    else
                    {
                        Debug.Log("Null Object");
                    }
                }
                Debug.Log("Destroyed enemy");
                Destroy(collision.gameObject);
                Destroy(this.gameObject);
                GameManager.Instance.CheckAvailableEnemy();
            }
            else
            {
                this.transform.GetComponent<SpriteRenderer>().color = HitColor;

                GameObject _particle = Instantiate(collisionParticle.gameObject, collision.transform.position, Quaternion.identity);
                Destroy(_particle, 1.5f);
                StartCoroutine(ColorReset());
                Destroy(collision.gameObject);
                GameManager.Instance.CheckAvailableEnemy();
            }
        }
    }

    IEnumerator ColorReset()
    {
        yield return new WaitForSeconds(0.2f);
        this.transform.GetComponent<SpriteRenderer>().color = DefaultColor;
    }
    public void GenerateSpecialPowerEnemy(Vector3 pos)
    {
        // Check if GameManager.Instance is not null
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GenerateSpecialPower(pos);
        }
        else
        {
            // GameManager.Instance is null
            Debug.LogError("GameManager.Instance is null!");
        }

    }
}