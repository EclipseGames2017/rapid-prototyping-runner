﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBase : MonoBehaviour
{

    [SerializeField, Range(1.0f, 20.0f)]
    protected float length;
    protected ETileType mTileType;

    public float Length { get { return length; } }
    public ETileType TileType { get { return mTileType; } }

    public virtual void Init(ETileType type)
    {
        mTileType = type;
    }

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
