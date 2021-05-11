using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDestroyer : MonoBehaviour
{
    // Start is called before the first frame updat

    [SerializeField] private GameObject destroyObj;

    internal void Activate()
    {
        Destroy(destroyObj);
    }
}
