using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    public void Shoot()
    {
        enemy.Shoot();
    }
}
