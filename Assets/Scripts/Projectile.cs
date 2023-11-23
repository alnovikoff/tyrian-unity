using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10.0f;
    private Vector3 velocity = Vector3.zero;
    private GameObject pref;
    public int damage = 5;
    public static Projectile Instantiate(Vector3 pos, Vector3 gunVelocity, Vector3 gunUnitAimingDir)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Rigidbody rb = sphere.AddComponent<Rigidbody>();
        rb.mass = 0f;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        sphere.gameObject.layer = 13;
        sphere.gameObject.tag = "BossProjectile2";
        sphere.GetComponent<Renderer>().material.color = Color.red;
        Projectile self = sphere.AddComponent<Projectile>();
        Physics.IgnoreCollision(sphere.GetComponent<Collider>(), sphere.GetComponent<Collider>());

        self.transform.position = pos;
        self.velocity = gunVelocity + self.speed * gunUnitAimingDir;
        return self;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "PlayerProjectile")
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.position = GameUtils.Instance.ComputeEulerStep(
        transform.position, velocity, Time.deltaTime);
        if (EnvironmentProps.Instance.IsOutsideArea(transform.position))
            Destroy(gameObject);
    }
}
