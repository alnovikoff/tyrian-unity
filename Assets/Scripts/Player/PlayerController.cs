using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;


    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveSpeedAndroid;

    [Header("Shoot")]
    public Transform spawnPoint;
    public GameObject bulletPref;
    public float bulletSpeed = 20.0f;
    public float delay = 0.25f;
    public float canFire = 0.25f;

    // Android 
    public float delayMobile = 0.45f;
    [SerializeField] private InputActionReference inputAction;


    void Update()
    {
#if UNITY_STANDALONE
        Move();
        if (Input.GetButton("Jump") && Time.time > canFire)
        {
            Shoot();
            canFire = Time.time + delay;
        }
#endif
#if UNITY_ANDROID
        if (Time.time > canFire && Time.timeSinceLevelLoad > 3.5f)
        {
            Shoot();
            canFire = Time.time + delayMobile;
        }

        Vector2 moveDir = inputAction.action.ReadValue<Vector2>();
        transform.Translate(moveDir * moveSpeedAndroid * Time.deltaTime);
        transform.position = EnvironmentProps.Instance.IntoArea(transform.position, gameObject.GetComponent<Collider>().transform.localScale.x, gameObject.GetComponent<Collider>().transform.localScale.z);
#endif
    }

    private void Move()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        gameObject.transform.position += move * Time.deltaTime * moveSpeed;
        transform.position = EnvironmentProps.Instance.IntoArea(transform.position, gameObject.GetComponent<Collider>().transform.localScale.x, gameObject.GetComponent<Collider>().transform.localScale.z);
    }

    private void Shoot()
    {
        FindObjectOfType<AudioManager>().PlaySound("shot");
        var bullet = Instantiate(bulletPref, spawnPoint.position, spawnPoint.rotation);
        bullet.GetComponent<Rigidbody>().velocity = spawnPoint.up * bulletSpeed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Hit Enemy");
            Currencies.Instance.scoreCrashInt++;
            UIManager.Instance.RefreshCurrncies();
            Currencies.Instance.credCrashInt += UIManager.Instance.playerBase.currentHealth * UIManager.Instance.playerBase.playerBulletDamage;
            Currencies.Instance.scoreCrashInt++;
        }

        if (collision.gameObject.tag == "Meteor")
        {
            FindObjectOfType<AudioManager>().PlaySound("explosion1");
            if (!Cheats.Instance.isCheatsAcivated)
                UIManager.Instance.playerBase.currentHealth -= collision.gameObject.GetComponent<Meteor>().meteorDamage;
            UIManager.Instance.playerBase.RefreshUI();
            Currencies.Instance.credCrashInt += UIManager.Instance.playerBase.currentHealth * UIManager.Instance.playerBase.playerBulletDamage;
            Currencies.Instance.scoreCrashInt++;
            UIManager.Instance.RefreshCurrncies();
            Debug.Log("Hit Meteor with damage: " + collision.gameObject.GetComponent<Meteor>().meteorDamage);
        }

        if (collision.gameObject.tag == "Bullet")
        {
            FindObjectOfType<AudioManager>().PlaySound("explosion");
            if (!Cheats.Instance.isCheatsAcivated)
                UIManager.Instance.playerBase.currentHealth -= collision.gameObject.GetComponent<ProjectileBase>().projectileDamage;
            UIManager.Instance.playerBase.RefreshUI();
            Debug.Log("Hit Enemy Bullet with damage: " + collision.gameObject.GetComponent<ProjectileBase>().projectileDamage);
        }

        if (collision.gameObject.tag == "Boss")
        {
            FindObjectOfType<AudioManager>().PlaySound("explosion1");
            Currencies.Instance.credCrashInt += UIManager.Instance.playerBase.currentHealth * UIManager.Instance.playerBase.playerBulletDamage;
            Currencies.Instance.scoreCrashInt++;
        }

        if (collision.gameObject.tag == "BossProjectile")
        {
            FindObjectOfType<AudioManager>().PlaySound("explosion1");
            if (!Cheats.Instance.isCheatsAcivated)
                UIManager.Instance.playerBase.currentHealth -= collision.gameObject.GetComponent<BossProjectile>().damage;
            UIManager.Instance.playerBase.RefreshUI();
        }

        if (collision.gameObject.tag == "BossProjectile2")
        {
            FindObjectOfType<AudioManager>().PlaySound("explosion");
            if (!Cheats.Instance.isCheatsAcivated)
                UIManager.Instance.playerBase.currentHealth -= collision.gameObject.GetComponent<Projectile>().damage;
            UIManager.Instance.playerBase.RefreshUI();
        }
    }
}
