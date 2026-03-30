using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VideoPlayerFadeOut : MonoBehaviour
{
    private RawImage videoText;
    float videoAlpha = 1;
    public float fadeSpeed = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        videoText = GetComponent<RawImage>();
        yield return new WaitForSecondsRealtime(5f);
        while (videoAlpha >= 0)
        {

            yield return null;
        }
        videoText.color = new Color(1f, 1f, 1f, videoAlpha);
    }

    void FixedUpdate()
    {
        // videoAlpha -= fadeSpeed * Time.fixedDeltaTime;

    }

}
