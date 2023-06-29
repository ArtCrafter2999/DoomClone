using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyAnimations : MonoBehaviour
{
    [SerializeField] private EnemyBase enemy;
    public void Attack()
    {
        enemy.Attack();
    }

    public void HurtSound()
    {
        enemy.HurtSound();
    }
    
    public void DeathSound()
    {
        enemy.DeathSound();
    }
}
