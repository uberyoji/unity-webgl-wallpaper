using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public Rigidbody Body;

    public float Damping = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        Damping = Random.Range(Damping - Damping / 2f, Damping + Damping / 2f);
        Body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
