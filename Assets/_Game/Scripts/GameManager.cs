using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject EnemyObject, EnemySpawningPoint1, EnemySpawningPoint2, EnemyParent, GameOverPanel, FirePrefab;
    [SerializeField] List<GameObject> AllEnemies, AllSelectedEnemyForPower, AllSelectedEnemyForFire, AlllEnemyGameArea;

    private bool isBulletFired;

    public static GameManager Instance;

    public void CheckAvailableEnemy()
    {
        for(int i = 0; i < AllEnemies.Count; i++)
        {
            if (AllEnemies[i] == null)
            {
                AllEnemies.Remove(AllEnemies[i]);
            }
        }
        Debug.Log("Ememy = "+AllEnemies.Count);
        if (AllEnemies.Count == 1)
        {
            SceneManager.LoadScene(1);
        }
    }
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        // Adjust the time scale to speed up the animation
        isBulletFired = false;
        Time.timeScale = 2f;
        int val = Random.Range(0, AlllEnemyGameArea.Count);
        StartCoroutine(SpawnAndMoveEnemies(AlllEnemyGameArea[val]));
        //SelectEnemyForFire();
    }
    IEnumerator SpawnAndMoveEnemies(GameObject EnemyGameArea)
    {
        float spawnDelay = 0.8f; // Adjust the delay between each object spawn

        for (int i = 0; i < EnemyGameArea.transform.childCount; i++)
        {
            Vector3 spawnPoint = i % 2 == 0 ? EnemySpawningPoint1.transform.position : EnemySpawningPoint2.transform.position;
            GameObject g = Instantiate(EnemyObject, spawnPoint, Quaternion.identity, EnemyParent.transform);
            AllEnemies.Add(g);

            yield return new WaitForSeconds(spawnDelay); // Wait for the specified spawn delay before moving the enemy

            // Move the enemy towards the target position
            StartCoroutine(EnemyMove(g, EnemyGameArea.transform.GetChild(i).position));
        }
    }
    IEnumerator EnemyMove(GameObject obj, Vector3 targetPosition)
    {
        // Define circular motion parameters
        float radiusY = 1f; // Set the radius of the circular path in the y-axis
        float duration = 3f; // Set the duration of the circular motion

        // Calculate the center of the circular path
        Vector3 center = (targetPosition + obj.transform.position) / 2f;

        // Store the initial position of the object
        Vector3 initialPosition = obj.transform.position;

        // Start the circular motion animation
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            // Calculate the current angle around the circle
            float angle = Mathf.Lerp(0f, Mathf.PI * 2f, elapsedTime / duration);

            // Calculate the position on the circular path in the y-axis
            float circularY = Mathf.Sin(angle) * radiusY;

            // Calculate the position along the line from initial position to target position
            Vector3 currentPosition = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);

            // Calculate the final position by combining circular motion and linear motion
            Vector3 finalPosition = new Vector3(currentPosition.x, initialPosition.y + circularY, currentPosition.z);

            // Move the object smoothly towards the final position
            obj.transform.position = finalPosition;

            // Increment the elapsed time
            elapsedTime += Time.deltaTime * 2;

            yield return new WaitForSeconds(0.001f); // Wait for the next frame
        }

        // Move the object to the target position
        obj.transform.DOMove(targetPosition, 2f);
        
        StartCoroutine(WaitUntillEnemySet());
    }
    IEnumerator WaitUntillEnemySet()
    {
        if (!isBulletFired)
        {
            isBulletFired = true;
            yield return new WaitForSeconds(3f);
            SelectEmenyForPower();
            SelectEnemyForFire();
            Debug.Log("Bullet fired");
            yield return new WaitForSeconds(4f);
            PlayerController.instance.SpawnBulletMethodForOtherScript();
            
        }
    }
    int val;

    public void SelectEmenyForPower()
    {
        //AllSelectedEnemyForPower.Clear();
        //for (int i = 0; i < 5; i++)
        //{
        //    do
        //    {
        //        val = Random.Range(0, AllEnemies.Count);
        //        Debug.Log("Val = " + val);
        //    } while (AllSelectedEnemyForPower.Contains(AllEnemies[val]));

        //    //AllSelectedEnemyForPower.Add(AllEnemies[val]);
        //    //AllSelectedEnemyForPower[AllSelectedEnemyForPower.Count - 1].GetComponent<EnemyHandling>().isObjSpecial = true;
        //}

        for (int i = 0; i < 3; i++)
        {
            do
            {
                val = Random.Range(0, AllEnemies.Count);
                
            } while (AllSelectedEnemyForPower.Contains(AllEnemies[val]));
            AllSelectedEnemyForPower.Add(AllEnemies[val]);
            AllSelectedEnemyForPower[i].GetComponent<EnemyHandling>().isObjSpecial = true;
        }
    }

    public void SelectEnemyForFire()
    {
        AllSelectedEnemyForFire.Clear();
        for (int i = 0; i < 5; i++)
        {
            do
            {
                val = Random.Range(0, AllEnemies.Count);

            } while (AllSelectedEnemyForFire.Contains(AllEnemies[val]));
            AllSelectedEnemyForFire.Add(AllEnemies[val]);
            AllSelectedEnemyForFire[i].GetComponent<EnemyHandling>().isObjFire = true;
        }
    }
    public void EnemyFireBullet(GameObject obj)
    {
        Debug.Log("Fire spawnned");
        GameObject bullet = Instantiate(FirePrefab, obj.transform.GetChild(0).position, Quaternion.Euler(0, 0, 90));
        bullet.GetComponent<Rigidbody2D>().AddForce( new Vector2(Random.Range(20,-50), Random.Range(20, -50)));
        bullet.transform.localScale = new Vector3(1, 1, 0);
        Destroy(bullet, 10f);
    }
    [SerializeField] List<GameObject> AllSpecialPowers;

    public void GenerateSpecialPower(Vector3 spawnPos)
    {
        int val = Random.Range(0, AllSpecialPowers.Count);
        Instantiate(AllSpecialPowers[val], spawnPos, Quaternion.identity);
    }
    public void GameOverPanelOpen()
    {
        GameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void GameOverPanelClose()
    {
        GameOverPanel.SetActive(false);
        Time.timeScale = 1;
    }
    public void OnClick_ResetBtnGameOverPanel()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }
    public void OnClick_HomeBtnGameOverPanel()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
