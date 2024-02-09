using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject EnemyObject, EnemyGameArea, EnemySpawningPoint1, EnemySpawningPoint2, EnemyParent;
    [SerializeField] List<GameObject> AllEnemies, AllSelectedSpecialEnemies;
    private void Start()
    {
        // Adjust the time scale to speed up the animation
        Time.timeScale = 2f;
        StartCoroutine(SpawnAndMoveEnemies());
    }
    IEnumerator SpawnAndMoveEnemies()
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
    }
    int val;

    public void SelectObjFromList()
    {
        for (int i = 0; i < 5; i++)
        {
            do
            {
                val = Random.Range(0, AllEnemies.Count);
                Debug.Log("Val = " + val);
            } while (AllSelectedSpecialEnemies.Contains(AllEnemies[val]));
            AllSelectedSpecialEnemies.Add(AllEnemies[val]);
            AllSelectedSpecialEnemies[val].GetComponent<EnemyHandling>().isSpecialObj = true;
        }
    }
}
