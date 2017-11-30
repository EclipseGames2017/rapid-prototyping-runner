using System;
using System.Collections;
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

    // call this when you spawn it (or move it when object pooling)
    public virtual void Resize(TileResizeArgs args)
    {
        length = args.length;
    }
}


/*
 * Stuff you want to tell the tle when you initialize it
 * here i just want length (eveything needs length)
 * */
public class TileResizeArgs
{
    public TileResizeArgs(float length)
    {
        this.length = length;
    }
    public float length;
}
