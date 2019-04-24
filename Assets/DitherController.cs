using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DitherController : MonoBehaviour
{
    public MeshRenderer Plane;

    public Material[] Mats;

    private int Index = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Index = ++Index % Mats.Length;
            Plane.material = Mats[Index];
        }
    }
}
