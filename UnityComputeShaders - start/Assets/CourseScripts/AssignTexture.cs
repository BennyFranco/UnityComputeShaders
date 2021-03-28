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
        outputTexture = new RenderTexture(texResoulution, texResoulution, 0);
        outputTexture.enableRandomWrite = true;
        outputTexture.Create();

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            DispatchShader(texResoulution / 8, texResoulution / 8);
        }
    }
}
