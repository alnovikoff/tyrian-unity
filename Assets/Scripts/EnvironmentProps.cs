using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentProps : MonoBehaviour
{
    public static EnvironmentProps Instance { get; private set; }

    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;

    public void Awake()
    {
        // Check, if we do not have any instance yet.
        if (Instance == null)
        {
            // 'this' is the first instance created => save it.
            Instance = this;
        }
        else if (Instance != this)
        {
            // Destroy 'this' object as there exist another instance
            Destroy(this.gameObject);
        }
    }

    public Vector3 IntoArea(Vector3 pos, float dx, float dz)
    {
        Vector3 result = pos;
        result.x = result.x - dx < minX ? minX + dx : result.x;
        result.x = result.x + dx > maxX ? maxX - dx : result.x;
        result.z = result.z - dz < minZ ? minZ + dz : result.z;
        result.z = result.z + dz > maxZ ? maxZ - dz : result.z;
        return result;
    }

    public bool IsOutsideArea(Vector3 pos)
    {
        return pos.x < minX || pos.x > maxX || pos.z < minZ || pos.z > maxX;
    }
}
