using DG.Tweening;
using System.Collections;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    [SerializeField] GameObject EnemyObject, EnemyGameArea, EnemySpawningPoint, EnemyParent;

    private void Start()
    {
        StartCoroutine(SpawnAndMoveEnemies());
    }

    IEnumerator SpawnAndMoveEnemies()
    {
        for (int i = 0; i < EnemyGameArea.transform.childCount; i++)
        {
            // Spawn a new enemy object
            GameObject g = Instantiate(EnemyObject, EnemySpawningPoint.transform.position, Quaternion.identity, EnemyParent.transform);

            // Move the enemy to the specific child position
            yield return StartCoroutine(EnemyMove(g, EnemyGameArea.transform.GetChild(i).position));
        }
    }

    IEnumerator EnemyMove(GameObject obj, Vector3 targetPosition)
    {
        obj.transform.DOMove(targetPosition, 0.5f).SetEase(Ease.OutQuint);
        yield return new WaitForSeconds(0.5f);

    }
}
