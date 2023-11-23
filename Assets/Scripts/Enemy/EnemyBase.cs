using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected int currentHealth;
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int enemyDamage;

    public float speed = 5.0f;
    int rand = 0;
    float canFire = 0.3f;
    float delay = 0f;
    float bulletSpeed = 4f;

    public Transform spawnPoint;
    public GameObject bulletPref;

    public Image helthBar;
    public TMP_Text healthTxt;


    public void Start()
    {
        rand = Random.Range(-5, 5);
        //helthBar = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>();
        //healthTxt = transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>();

        currentHealth = maxHealth;
        helthBar.fillAmount =(float)currentHealth / (float)maxHealth;
        healthTxt.text = currentHealth.ToString();
    }

    public void Update()
    {
        MoveEnemy();
        if (Time.time > canFire)
        {
            EnemyShoot();
            delay = Random.Range(3, 5);
            canFire = Time.time + delay;
        }
    }

    private void EnemyShoot()
    {
        FindObjectOfType<AudioManager>().PlaySound("shot2");
        var bullet = Instantiate(bulletPref, spawnPoint.position, spawnPoint.rotation);
        bullet.GetComponent<Rigidbody>().velocity = spawnPoint.forward * 25;
        Vector3.Normalize(bullet.GetComponent<Rigidbody>().velocity);
    }

    private void MoveEnemy()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        float pingPongValue = Mathf.PingPong(Time.time, 1) - rand;
        gameObject.transform.position = new Vector3(pingPongValue, transform.position.y, transform.position.z);


        if (gameObject.transform.position.z < -11)
        {
            Destroy(gameObject);
            EnemySpawner.enemyCounter--;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerBullet")
        {
            if(currentHealth > 0) 
            {
                FindObjectOfType<AudioManager>().PlaySound("explosion");
                currentHealth -= UIManager.Instance.playerBase.playerBulletDamage;
                Currencies.Instance.scoreHitsInt++;
                Currencies.Instance.credHitsInt += UIManager.Instance.playerBase.currentHealth * UIManager.Instance.playerBase.playerBulletDamage;
                UIManager.Instance.RefreshCurrncies();

                helthBar.fillAmount = (float)currentHealth / (float)maxHealth;
                healthTxt.text = currentHealth.ToString();  
                if (currentHealth <= 0)
                {
                    FindObjectOfType<AudioManager>().PlaySound("explosion1");
                    Destroy(gameObject);
                    EnemySpawner.enemyCounter--;
                    Currencies.Instance.scoreKillsInt++;
                    Currencies.Instance.credKillsInt += UIManager.Instance.playerBase.currentHealth * UIManager.Instance.playerBase.playerBulletDamage;
                    UIManager.Instance.RefreshCurrncies();
                    GameManager.Instance.gameGloasCounter++;
                }
            }
            else
            {
                FindObjectOfType<AudioManager>().PlaySound("explosion1");
                Destroy(gameObject);
                EnemySpawner.enemyCounter--;
                GameManager.Instance.gameGloasCounter++;
            }
        }
        if (collision.gameObject.tag == "Player")
        {
            if (currentHealth > 0)
            {
                FindObjectOfType<AudioManager>().PlaySound("explosion2");
                currentHealth -= UIManager.Instance.playerBase.playerDamage;
                if (!Cheats.Instance.isCheatsAcivated)
                {
                    UIManager.Instance.playerBase.currentHealth -= enemyDamage;
                    UIManager.Instance.playerBase.RefreshUI();
                }
                helthBar.fillAmount = (float)currentHealth / (float)maxHealth;
                healthTxt.text = currentHealth.ToString();
                if (currentHealth <= 0)
                {
                    GameManager.Instance.gameGloasCounter++;
                    FindObjectOfType<AudioManager>().PlaySound("explosion2");
                    Destroy(gameObject);
                    EnemySpawner.enemyCounter--;
                    Currencies.Instance.scoreKillsInt++;
                    Currencies.Instance.credKillsInt += UIManager.Instance.playerBase.currentHealth * UIManager.Instance.playerBase.playerDamage;
                    UIManager.Instance.RefreshCurrncies();
                }
            }
            else
            {
                FindObjectOfType<AudioManager>().PlaySound("explosion2");
                Destroy(gameObject);
                EnemySpawner.enemyCounter--;
                GameManager.Instance.gameGloasCounter++;
            }
        }
    }

}
