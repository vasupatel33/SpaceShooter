using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandling : MonoBehaviour
{
    [SerializeField] int Health = 10;

    public static EnemyHandling Instance;

    private void Awake()
    {
        Instance = this;
    }
}
