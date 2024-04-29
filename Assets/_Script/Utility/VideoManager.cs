using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

namespace BogBog.Utility
{
    public class VideoManager : MonoBehaviour
{
    public delegate void OnVideoEnded();
    public OnVideoEnded m_onVideoEnded;
    
    public bool fade = false;
    public ushort audioTrackCount = 1;

    [Header("Video Component")]
    public VideoPlayer videoPlayer;
    public RawImage videoPreview;
    public AudioSource audioSource;
    public GameObject pause;
    public VideoClip[] clips;

    [Header("Control Panel")]
    public Slider videoProgressBar;

    public bool isSliding;
    public bool closeOnEnded = false;

    private void VideoLoopPointReached(VideoPlayer source)
    {
        if(closeOnEnded) Close();

        if (m_onVideoEnded != null)
            m_onVideoEnded();
    }

    private void OnEnable()
    {
        if (videoPlayer == null) videoPlayer = GetComponent<VideoPlayer>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (videoPreview == null) videoPreview = GetComponent<RawImage>();

        videoPlayer.loopPointReached += VideoLoopPointReached;
        videoPreview.color = new Color(1, 1, 1, 0);
        Play(clips[0],0);
    }

    private void OnDisable()
    {
        videoPlayer.loopPointReached -= VideoLoopPointReached;
    }

    public void Stop(bool isHide = false)
    {
        if (videoPlayer.isPlaying)
            videoPlayer.Stop();

        if (isHide)
            gameObject.SetActive(false);
    }

    public double fraction;
    public void TogglePlayingVideo()
    {
        if (videoPlayer.isPlaying)
        {
            Debug.Log("Pause");
            videoPlayer.Pause();
        }
        else
        {
            Debug.Log("Player");
            videoPlayer.Play();
        }
    }

    public void Seek()
    {
        //Debug.Log(videoProgressBar.value);
    }

    public void Play(VideoClip clip)
    {
        gameObject.SetActive(true);
        
        StartCoroutine(_PlayVideo(clip, 0));
    }
    public void Play(VideoClip clip, float second)
    {
        gameObject.SetActive(true);
        StartCoroutine(_PlayVideo(clip, second));
        if (pause)
            pause.SetActive(false);

    }
    IEnumerator _PlayVideo(VideoClip clip, float starttime)
    {
        Color color = videoPreview.color;
        float alpha = 0;

        if (fade)
        {
            videoPreview.color = new Color(color.r, color.g, color.b, alpha);
        }


        videoPlayer.clip = clip;

        if (!videoPlayer.isPrepared)
            videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        if (audioSource != null)
        {
            videoPlayer.controlledAudioTrackCount = audioTrackCount;
            videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
            videoPlayer.EnableAudioTrack(0, true);
            videoPlayer.SetTargetAudioSource(0, audioSource);
        }


        videoPreview.texture = videoPlayer.texture;
        videoPlayer.time = starttime;
        videoPlayer.Play();

        if (fade)
        {
            while (alpha < 1)
            {
                alpha += 2f * Time.deltaTime;
                videoPreview.color = new Color(color.r, color.g, color.b, alpha);
                yield return null;
            }
        }
        else
        {
            videoPreview.color = Color.white;
        }

        while (videoPlayer.isPlaying)
        {

            yield return null;
        }

    }

    public void Close()
    {
        gameObject.SetActive(false);
        fraction = 0;
        if (videoProgressBar)
        {
            videoProgressBar.value = (long)fraction;
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if (videoPlayer == null) return;
        if (videoPlayer.clip == null || !videoPlayer.isPlaying)
        {
            return;
        }

        if (videoProgressBar)
        {
            if (!isSliding)
            {
                fraction = (double)videoPlayer.frame / (double)videoPlayer.clip.frameCount;
                videoProgressBar.value = (float)fraction;
            }

        }
    }
}
}

