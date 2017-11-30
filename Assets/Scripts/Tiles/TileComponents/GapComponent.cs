using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class GapComponent : MonoBehaviour
{
    [Range(1.0f, 20.0f)]
    private float length = 5.0f;

    public Transform startHelper, endHelper;

    // Use this for initialization
    void Start()
    {

    }

    public void Resize(float newLength)
    {
        length = newLength;
        endHelper.localPosition = Vector2.right * length;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
