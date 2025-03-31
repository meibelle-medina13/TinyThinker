using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Credits_Manager : MonoBehaviour
{
    public VideoPlayer player;
    double time;
    // Start is called before the first frame update
    void Start()
    {
        player.loopPointReached += EndReached;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(player.playbackSpeed);
        if (player.playbackSpeed < 1)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        vp.playbackSpeed = vp.playbackSpeed / 10.0F;
    }
}
