using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skybox : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float skyboxSpeed = 1.5f;
    void Start()
    {
        //RenderSettings.skybox.SetFloat("_Rotation", -90);
    }

    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * skyboxSpeed);
    }
}
