using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperHealth : MonoBehaviour
{
    public AudioClip collectedClip;
    public GameObject PickUpEffect;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            
            if (controller.health < controller.maxHealth)
            {
                controller.ChangeHealth(2);
                GameObject HealthCollectible = Instantiate(PickUpEffect,transform.position, Quaternion.identity);
                Destroy(gameObject);
            
                controller.PlaySound(collectedClip);
            }
        }

    }
}