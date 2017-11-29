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

        // make a new material from the shader
        effectMaterial = new Material(shader);

        //Shader.SetGlobalTexture("_SecondTex", targetA);
        //Shader.SetGlobalTexture("_Transition", Transition);

        // put the other camera's render texture into the material
        effectMaterial.SetTexture("_SecondTex", rTarget);
        // put the transition texture into the material
        effectMaterial.SetTexture("_Transition", Transition);
        // tell the shader the screen aspect ratio
        effectMaterial.SetFloat("_Ratio", (float)Screen.height / (float)Screen.width);

    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // set the cutoff value in the shader
        effectMaterial.SetFloat("_Cutoff", cutoff);
        // run the material on the camera's output, this then goes to either the next imageEffect or to the screen
        Graphics.Blit(source, destination, effectMaterial);
    }

    // Update is called once per frame
    void Update()
    {

    }
}