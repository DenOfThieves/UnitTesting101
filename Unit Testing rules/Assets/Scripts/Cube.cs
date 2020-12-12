using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public float health;
    void Update()
    {
        DisableOnDeath();
    }

    public void DisableOnDeath()
    {
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }    
}
