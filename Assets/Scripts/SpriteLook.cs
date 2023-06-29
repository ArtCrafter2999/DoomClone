using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SpriteLook : MonoBehaviour
{
    private Transform _player;
    [Inject]
    private void Construct(Player player)
    {
        _player = player.transform;
    }
    
    void Update()
    {
        var direction = (_player.position - transform.position).normalized;
        direction = new Vector3(direction.x, 0, direction.z);
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
