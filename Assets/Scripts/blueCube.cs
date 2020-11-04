using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blueCube : MonoBehaviour
{
    public MeshRenderer M_render;
    // Start is called before the first frame update
    void Start()
    {
        M_render = GetComponent<MeshRenderer>();
        M_render.material.color = Color.blue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
