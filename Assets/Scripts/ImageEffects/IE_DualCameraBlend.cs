using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class IE_DualCameraBlend : MonoBehaviour
{

    public Shader shader;
    public Texture2D Transition;
    [Range(0.0f, 1.01f)]
    public float cutoff = 0.5f;

    private Camera mainCamera;
    public Camera secondaryCamera;

    RenderTexture rTarget;

    private Material effectMaterial;

    // Use this for initialization
    void Awake()
    {
        mainCamera = GetComponent<Camera>();
        //gameObject.layer = LayerMask.GetMask("CaptureA");

        rTarget = new RenderTexture(Screen.width, Screen.height, 24);
        secondaryCamera.targetTexture = rTarget;

        effectMaterial = new Material(shader);

        //Shader.SetGlobalTexture("_SecondTex", targetA);
        //Shader.SetGlobalTexture("_Transition", Transition);

        effectMaterial.SetTexture("_SecondTex", rTarget);
        effectMaterial.SetTexture("_Transition", Transition);

    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        effectMaterial.SetFloat("_Cutoff", cutoff);
        Graphics.Blit(source, destination, effectMaterial);
    }

    // Update is called once per frame
    void Update()
    {

    }
}