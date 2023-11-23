using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public int damage = 1;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody>().velocity = transform.position * -1;
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), GetComponent<Collider>());
    }

    private void Update()
    {
        if(transform.position.x < -20.0f || transform.position.x > 20.0f || transform.position.z < -20.0f || transform.position.z > 20.0f)
            Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
