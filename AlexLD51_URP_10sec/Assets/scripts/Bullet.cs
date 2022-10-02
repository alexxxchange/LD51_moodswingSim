using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.TryGetComponent(out enemy _enemy);
        if (_enemy != null)
        {
            _enemy.Die();
            Destroy(gameObject);
        }

    

        else
        {
            Destroy(gameObject);
        }
    }
}
