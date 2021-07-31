using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{

    public Transform pointer;

    public Renderer renderer;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = pointer.position - transform.position;
        renderer.materials[0].SetVector("_LightDirection", -pointer.forward);
    }
}
