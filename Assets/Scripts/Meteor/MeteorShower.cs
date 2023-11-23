using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeteorShower : MonoBehaviour
{
    public GameObject meteorPref;

    private void Start()
    {
        Invoke("StartWithDelay", 3);
    }

    private void StartWithDelay()
    {
        StartCoroutine(SendMeteor());
    }

    public IEnumerator SendMeteor()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 2.5f));
            var meteor = Instantiate(meteorPref, new Vector3(Random.Range(-5, 5), 0, 19), Quaternion.identity);
        }
    }
}
