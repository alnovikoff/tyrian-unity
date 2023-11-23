using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
    [SerializeField] public int projectileHealth;
    [SerializeField] public int projectileDamage;

    public void Start()
    {
        Destroy(gameObject, 3.0f);
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), GetComponent<Collider>());
    }

    public void Update()
    {
        if (gameObject.transform.position.z < -15)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "PlayerBullet")
        {
            Destroy(gameObject);
        }
    }
}
