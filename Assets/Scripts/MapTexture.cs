using UnityEngine;
using System.Collections;

public class MapTexture : MonoBehaviour
{
    public float lat =  35.710071f;
    public float lon = 139.810707f;

    private STATE state = STATE.ENABLE;

    int maxWait = 20;

    public int size = 512;
    public int zoom = 14;
    public WWW DownloadImg;

    public enum STATE
    {
        ENABLE,
        WAIT,
        END
    }

    void Start()
    {
        state = STATE.ENABLE;
        InvokeRepeating("RetrieveGPSData", 0, 5);
        Debug.Log("Initiallized");
    }

    IEnumerator Download(string url)
    {
        Debug.Log("Download");
        Debug.Log("Download URL : " + url);

        DownloadImg = new WWW(url);
        yield return DownloadImg; // Wait for download to complete

        int textureSize = 512;
        GetComponent<UnityEngine.UI.Image>().sprite = Sprite.Create(DownloadImg.texture, new Rect(0, 0, textureSize, textureSize), Vector2.zero);

        state = STATE.ENABLE;

        Debug.Log("Downloaded");
    }

    void RetrieveGPSData()
    {
        if (state == STATE.ENABLE)
        {
            state = STATE.WAIT;

            Debug.Log("Refresh");
            var url = "http://maps.googleapis.com/maps/api/staticmap";
            var qs = "";
            qs += "center=" + string.Format("{0},{1}", lat, lon);
            qs += "&zoom=" + zoom.ToString();
            qs += "&size=" + string.Format("{0}x{0}", size);
            qs += "&scale=" + "2";
            qs += "&maptype=" + "roadmap";
            qs += "&sensor=false";

            Debug.Log("Refreshed");

            StartCoroutine(Download(url + "?" + qs));
        }
    }
}
