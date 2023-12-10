using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerScript : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private bool gotItYet = false;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Debug.Log("PLAYING VIDEO");
            videoPlayer.time = 0f;
            videoPlayer.Play();

            if (!gotItYet) {
                GameObject gameObject = GameObject.Find("GameMaster");
                GameMasterScript script = gameObject.GetComponent<GameMasterScript>();

                script.GotEasterEgg();
            }

            gotItYet = true;
        }
    }
}
