using UnityEngine;

public class CameraBlur : MonoBehaviour
{

    public Material Blur;
    public int Iteration;
    public int DownRes;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        int width = source.width >> DownRes;
        int height = source.height >> DownRes;
        RenderTexture rt = RenderTexture.GetTemporary(width, height);
        Graphics.Blit(source, rt);

        for (int i = 0; i < Iteration; i++)
        {
            RenderTexture rt2 = RenderTexture.GetTemporary(width, height);
            Graphics.Blit(rt, rt2, Blur);
            RenderTexture.ReleaseTemporary(rt);
            rt = rt2;
        }

        Graphics.Blit(rt, destination);
        RenderTexture.ReleaseTemporary(rt);
    }

}
