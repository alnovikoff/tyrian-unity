using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] public int bulletDamage;
    private void Start()
    {
        Destroy(gameObject, 3.0f);
    }

    void Update()
    {
        if (gameObject.transform.position.z < -11)
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
        if(collision.gameObject.tag == "PlayerBullet")
        {
            Destroy(gameObject);
        }
    }
}
