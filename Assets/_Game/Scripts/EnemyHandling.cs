using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandling : MonoBehaviour
{
    public int Health = 10;
    public Color HitColor;

    public static EnemyHandling Instance;

    private void Awake()
    {
        Instance = this;
    }
}
