using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerControls : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] Sprite muteOn, muteOff;
    [SerializeField] Image VolumeIcon;
    [SerializeField] VideoPlayer videoPlayer;

    private void Start()
    {
        
    }

    public void ToggleMute()
    {
        if (videoPlayer.GetDirectAudioMute(0))
        {
            videoPlayer.SetDirectAudioMute(0, false);
            VolumeIcon.sprite = muteOff;
        }
        else
        {
            videoPlayer.SetDirectAudioMute(0, true);
            VolumeIcon.sprite = muteOn;

        }

    }

    public void OnValueChange()
    {
        videoPlayer.SetDirectAudioVolume(0,volumeSlider.value*0.25f);
    }
}
