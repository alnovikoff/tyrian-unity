using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefs;
    public static int enemyCounter = 0;

    private void Start()
    {
        enemyCounter = 0;
        StartCoroutine(SendMeteor());
    }

    public IEnumerator SendMeteor()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 2.5f));
            if (enemyCounter < 3)
            {
                int rand = Random.Range(0, enemyPrefs.Length);
                var enemy = Instantiate(enemyPrefs[rand], new Vector3(Random.Range(-5.0f, 5.0f), 0, 19), new Quaternion(0, 180, 0, 0));
                enemyCounter++;
                Debug.Log(enemyCounter);
            }
        }
    }
}
