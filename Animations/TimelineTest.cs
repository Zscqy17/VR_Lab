using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class TimelineTest : MonoBehaviour
{
    PlayableDirector playableDirector;

    [SerializeField]
    TimelineAsset[] timelineAssets;

    int index = 0;

    void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            if (playableDirector.state == PlayState.Paused)
            {
                if (index < timelineAssets.Length)
                {
                    playableDirector.Play(timelineAssets[index++]);
                }
            }
    }
}
