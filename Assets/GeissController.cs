using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Need to create a chain TexCam (depth 0), GeissCam (depth 1), Camera (depth 2)

// TexCam is used to generate texture and binds it to MatA
// GeissCam uses MatA and MatB to generate texture and binds it to MatC
// Camera uses MatC to display final result
// Render textures are swapped between MatB and MatC to feedback the result into the GeissCam combine

public class GeissController : MonoBehaviour
{
    public Camera SourceCam;
    
    public Material MatA;   // Combine source A
    public Material MatB;   // Combine source B
    public Material MatC;   // Combine result

    public RenderTexture[] RT;

    int Index = 0;

    Camera GeissCam; // current camera

    public RenderTexture RTA { get { return RT[0]; } }
    public RenderTexture RTB { get { return RT[Index == 0 ? 1 : 2]; } }
    public RenderTexture RTC { get { return RT[Index == 0 ? 2 : 1]; } }

    private void Start()
    {
        GeissCam = GetComponent<Camera>();
        SourceCam.SetTargetBuffers(RTA.colorBuffer, RTA.depthBuffer);
    }

    void OnPreRender()
    {
        MatA.mainTexture = RTA;
        MatB.mainTexture = RTB;
        MatC.mainTexture = RTC;
        GeissCam.SetTargetBuffers(RTC.colorBuffer, RTC.depthBuffer);
    }

    public void OnPostRender()
    {
        // Cycles buffers
        Index = ++Index % 2;
    }
}
