using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision coll)
    {
        GameObject collisionGO = coll.gameObject;
        if (collisionGO.CompareTag("Projectile"))
        {
            Destroy(this.gameObject);
        }
    }
}
