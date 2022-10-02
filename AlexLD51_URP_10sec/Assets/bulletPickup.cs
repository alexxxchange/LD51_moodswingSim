using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletPickup : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject pickupPrefab;
    bool triggered;
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.TryGetComponent(out Player player);
        if (player != null && !triggered)
        {
            triggered = true;
           Instantiate(pickupPrefab, transform.position, Quaternion.identity);
           player.bullets++;
            player.UpdateBulletUI();
            Destroy(gameObject);
        }
    }
}

