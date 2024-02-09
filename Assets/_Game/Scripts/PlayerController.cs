using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject bulletSpawnPoint, BulletPref;
    Vector2 screenPos;
    Vector2 maxScreenVal;
    float objectHalfWidth;

    public static PlayerController instance;


    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        bulletSpawnPoint = GameObject.Find("AllBullets");
        //InvokeRepeating("SpawningBullet", 0.4f,0.4f);
        screenPos = new Vector2(Screen.width, Screen.height);
        maxScreenVal = Camera.main.ScreenToWorldPoint(screenPos);
        objectHalfWidth = transform.localScale.x / 2f; // Assuming the object's width is its scale on the x-axis
        Debug.Log("Max screen position = " + maxScreenVal.x + ", " + maxScreenVal.y);
        InvokeRepeating("SpawningBullet", 0.4f, 0.4f);
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
    public void SpawningBullet()
    {
        GameObject bullet = Instantiate(BulletPref, this.transform.GetChild(0).transform.position, Quaternion.Euler(0, 0, 90), bulletSpawnPoint.transform);
        Destroy(bullet,4f);
    }
}
