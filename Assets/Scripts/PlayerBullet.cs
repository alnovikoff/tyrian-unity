using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    UIManager uiManager;    
    private void Start()
    {
        uiManager = FindAnyObjectByType<UIManager>();
        Destroy(gameObject, 3.0f);
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
    }
    void Update()
    {
        if (gameObject.transform.position.z > 15)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Meteor")
        {
            Debug.Log("Meteor hits player bullet");
            Currencies.Instance.scoreHitsInt++;
            Currencies.Instance.credHitsInt += UIManager.Instance.playerBase.currentHealth * UIManager.Instance.playerBase.playerBulletDamage;
            uiManager.RefreshCurrncies();
            Destroy(gameObject);
        }
        if(collision.gameObject.tag == "Boss")
        {
            Destroy(gameObject);
        }
    }
}
