using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLook : MonoBehaviour
{
    void Update()
    {
        var direction = (Player.Instance.transform.position - transform.position).normalized;
        direction = new Vector3(direction.x, 0, direction.z);
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
