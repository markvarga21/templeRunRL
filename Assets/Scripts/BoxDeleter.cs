using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDeleter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject); 
    }
}
