using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignTexture : MonoBehaviour
{
    public ComputeShader shader;
    public int texResoulution = 256;

    private Renderer rend;
    private RenderTexture outputTexture;
    private int kernelHandle;

    // Start is called before the first frame update
    void Start()
    {
        // 1. Create a render texture with size 'texResolution' (256 x 256) and depth 0
        outputTexture = new RenderTexture(texResoulution, texResoulution, 0);
        // 2. Allows to our compute shader to write and read this texture 
        outputTexture.enableRandomWrite = true;
        // 3.  Creates the hardware texture, check: https://docs.unity3d.com/ScriptReference/RenderTexture.Create.html
        outputTexture.Create();

        // 4. Get the renderer component reference
        rend = GetComponent<Renderer>();
        rend.enabled = true;

        InitShader();
    }

    private void InitShader()
    {
        kernelHandle = shader.FindKernel("CSMain");
        shader.SetTexture(kernelHandle, "Result", outputTexture);
        rend.material.SetTexture("_MainTex", outputTexture);

        DispatchShader(texResoulution / 16, texResoulution / 16);
    }

    private void DispatchShader(int x, int y)
    {
        shader.Dispatch(kernelHandle, x, y, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.U))
        {
            DispatchShader(texResoulution / 8, texResoulution / 8);
        }
    }
}
