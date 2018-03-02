using UnityEngine;
using System.Collections;

//PIXELBOY BY @WTFMIG EAT A BUTT WORLD BAHAHAHAHA POOP MY PANTS
[ExecuteInEditMode]
[AddComponentMenu ("Image Effects/PixelBoy")]
/*PIXELBOY BY @WTFMIG EAT A BUTT WORLD BAHAHAHAHA POOP MY PANTS
* "Yes that is his actual copyright"
* Used to pixelate the screen in a fairly cheap way */
public class PixelBoy : MonoBehaviour
{
    public int w = 720;
    int h;
    public Camera Camera;

    protected void Start ()
    {
        if (!SystemInfo.supportsImageEffects) {
            enabled = false;
            return;
        }
        // + + + +  Added by Louis Lombardo IV + + + +
        if (Camera == null) {
            try { 
                Camera = GetComponent<Camera> ();
            } catch (UnassignedReferenceException e) {
                Debug.Log(e);
            }
        }
        if (Camera == null) {
            this.enabled = false;
        }
        // = = = = = = = = = = = = = = = = = = = = = =
    }

    void Update ()
    {

        float ratio = ((float)Camera.pixelHeight / (float)Camera.pixelWidth);
        h = Mathf.RoundToInt (w * ratio);

    }

    void OnRenderImage (RenderTexture source, RenderTexture destination)
    {
        source.filterMode = FilterMode.Point;
        RenderTexture buffer = RenderTexture.GetTemporary (w, h, -1);
        buffer.filterMode = FilterMode.Point;
        Graphics.Blit (source, buffer);
        Graphics.Blit (buffer, destination);
        RenderTexture.ReleaseTemporary (buffer);
    }
}