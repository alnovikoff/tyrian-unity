using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Meteor : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] public int meteorDamage;

    private void Start()
    {
        float size = Random.Range(0.5f, 1.5f);
        this.gameObject.transform.localScale = new Vector3(size, size, size);
        gameObject.GetComponent<SphereCollider>().radius = size;
        if (size < 1f)
        {
            speed = 8f;
        }
        else
        {
            speed = 4f;
        }
    }
    void Update()
    {
        gameObject.transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
        if(gameObject.transform.position.z < -11)
        {
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerBullet")
        {
            GameManager.Instance.gameGloasCounter++;
            FindAnyObjectByType<AudioManager>().PlaySound("explosion1");
            Destroy(gameObject);
            Debug.Log("Meteor hits player bullet");
            Currencies.Instance.scoreKillsInt++;
            Currencies.Instance.credKillsInt += UIManager.Instance.playerBase.currentHealth * UIManager.Instance.playerBase.playerBulletDamage;
            UIManager.Instance.RefreshCurrncies();
        }
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            GameManager.Instance.gameGloasCounter++;
        }
    }
}
