using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBase : MonoBehaviour
{
    [SerializeField, Range(1.0f, 20.0f)]
    protected float length;

    public float Length { get { return length; } }

    public virtual void Resize(float newLength)
    {
        length = newLength;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
