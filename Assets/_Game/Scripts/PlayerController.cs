using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject bulletSpawnPoint1, BulletPref;
    Vector2 screenPos;
    Vector2 maxScreenVal;
    float objectHalfWidth;
    public bool isBulletFireOn;

    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        bulletSpawnPoint1 = GameObject.Find("AllBullets");
        screenPos = new Vector2(Screen.width, Screen.height);
        maxScreenVal = Camera.main.ScreenToWorldPoint(screenPos);
        objectHalfWidth = transform.localScale.x / 2f; // Assuming the object's width is its scale on the x-axis
        //Debug.Log("Max screen position = " + maxScreenVal.x + ", " + maxScreenVal.y);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float newXPos = Mathf.Clamp(currentPos.x, -maxScreenVal.x + objectHalfWidth, maxScreenVal.x - objectHalfWidth);
            float newYPos = Mathf.Clamp(currentPos.y, -maxScreenVal.y + (objectHalfWidth - 0.5f), maxScreenVal.y - (objectHalfWidth - 0.5f));
            Vector3 newPosition = new Vector3(newXPos, newYPos, transform.position.z);
            transform.DOMove(newPosition, 0.5f);
        }
    }
    bool isSecondSpawnPointOn, isThirdSpawnPointOn;
    int specialSpawnCount = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "specialBullet")
        {
            Debug.Log("Special bullet spawnned");

            if(isSecondSpawnPointOn == true)
            {
                isThirdSpawnPointOn = true;
            }
            if(!isSecondSpawnPointOn)
            {
                isSecondSpawnPointOn = true;
            }
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.CompareTag("enemyBullet"))
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
            GameManager.Instance.GameOverPanelOpen();
        }
    }
    [SerializeField] AudioClip bulletFireClip;
    public void SpawningBullet()
    {
        Common.Instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().PlayOneShot(bulletFireClip);
        GameObject bullet = Instantiate(BulletPref, this.transform.GetChild(0).transform.position, Quaternion.Euler(0, 0, 90), bulletSpawnPoint1.transform);
        Destroy(bullet,4f);
        if(isSecondSpawnPointOn)
        {
            GameObject bullet1 = Instantiate(BulletPref, this.transform.GetChild(1).transform.position, Quaternion.Euler(0, 0, 90), bulletSpawnPoint1.transform);
            Destroy(bullet, 4f);
        }
        if (isThirdSpawnPointOn)
        {
            GameObject bullet2 = Instantiate(BulletPref, this.transform.GetChild(2).transform.position, Quaternion.Euler(0, 0, 90), bulletSpawnPoint1.transform);
            Destroy(bullet, 4f);
        }

    }
    public void SpawnBulletMethodForOtherScript()
    {
        InvokeRepeating("SpawningBullet", 0.4f, 0.4f);
    }
}
