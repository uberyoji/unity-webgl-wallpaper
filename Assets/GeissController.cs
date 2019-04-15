using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeissController : MonoBehaviour
{
    public Camera TexCam;
    
    public Material MatA;
    public Material MatB;
    public Material MatC;

    public RenderTexture[] RT;

    int Index = 0;

    Camera Cam; // current camera

    public RenderTexture RTA { get { return RT[0]; } }
    public RenderTexture RTB { get { return RT[Index == 0 ? 1 : 2]; } }
    public RenderTexture RTC { get { return RT[Index == 0 ? 2 : 1]; } }

    private void Start()
    {
        Cam = GetComponent<Camera>();
    }

    void OnPreRender()
    {
        MatA.mainTexture = RTA;
        MatB.mainTexture = RTB;
        Cam.SetTargetBuffers(RTC.colorBuffer, RTC.depthBuffer);
    }

    public void OnPostRender()
    {
        // Output final
        MatC.mainTexture = RTC;

        // Cycles buffers
        Index = ++Index % 2; // RT.Length;

        // Update texture camera
        TexCam.SetTargetBuffers(RTA.colorBuffer, RTA.depthBuffer);

//        Debug.Log("Texture Swap (" + Index + ")");
    }
}
