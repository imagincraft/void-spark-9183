using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

/// <summary>
/// All audios that in the game are managed as AudioSource objects here, accessed via GameManager.
/// Ex: GameManager.Instance.AudioService.PlayAudio(AudioType.InGameBackgroundMusic);
///
/// Added IAudioService interface for easy modification later
/// </summary>

public interface IAudioService
{
    void PlayAudio(AudioType audioType); // Simplified to use AudioType only
    void StopAudio(AudioSource source);
    void StopAllAudio();
    void RemoveAudioSource(AudioSource source);

    // Expose AudioSource properties
   
    AudioSource PanelOpenSource { get; }
    AudioSource PanelCloseSource { get; }
    AudioSource ButtonClickSource { get; }
    AudioSource ImageClickingSource { get; }
    AudioSource ErrorSource { get; }
}

public class AudioManager : MonoBehaviour, IAudioService
{
    // Existing AudioSource fields remain unchanged
    [SerializeField] private AudioSource inGameBackgroundMusicSource;
    [SerializeField] private AudioSource panelOpenSource;
    [SerializeField] private AudioSource panelCloseSource;
    [SerializeField] private AudioSource buttonClickSource;
    [SerializeField] private AudioSource imageclickingSource;
    [SerializeField] private AudioSource errorSource;


    private List<AudioSource> activeAudioSources = new List<AudioSource>();

    // Volume settings for each category (0 to 100, normalized to 0-1 for AudioSource)
    private float musicVolume = 0.5f;
    private float soundEffectVolume = 0.5f;
    private float voiceOverVolume = 0.5f;
    private float ambientSoundVolume = 0.5f;

    void Awake()
    {
        RegisterAudioSource(inGameBackgroundMusicSource);
        RegisterAudioSource(panelOpenSource);
        RegisterAudioSource(panelCloseSource);
        RegisterAudioSource(buttonClickSource);
        RegisterAudioSource(imageclickingSource);
        RegisterAudioSource(errorSource);


    
    }

    private void RegisterAudioSource(AudioSource source)
    {
        if (source != null && !activeAudioSources.Contains(source))
        {
            activeAudioSources.Add(source);
            Debug.Log($"Registered AudioSource: {source.name}");
        }
    }

  

    private AudioSource GetAudioSourceForType(AudioType audioType)
    {
        switch (audioType)
        {

            case AudioType.PanelOpen: return panelOpenSource;
            case AudioType.PanelClose: return panelCloseSource;
            case AudioType.ButtonClick: return buttonClickSource;
            case AudioType.ImageClicking: return imageclickingSource;
            case AudioType.Error: return errorSource;
    
            default: return null;
        }
    }
    public void PlayAudio(AudioType audioType)
    {
        AudioSource source = GetAudioSourceForType(audioType);
        if (source == null || source.clip == null)
        {
            Debug.LogWarning($"AudioSource or clip is null for audioType: {audioType}");
            return;
        }

      

        // Configure audio settings
        switch (audioType)
        {

            case AudioType.ButtonClick:
            case AudioType.PanelOpen:
            case AudioType.PanelClose:
            case AudioType.ImageClicking:
            case AudioType.Error:

                source.loop = false;
                break;
        }

        if (source.isPlaying)
        {
            source.Stop();
        }
        source.Play();
    }

 
    public void StopAudio(AudioSource source)
    {
        if (source != null && activeAudioSources.Contains(source))
        {
            source.Stop();
        }
    }

    public void StopAllAudio()
    {
        foreach (var source in activeAudioSources)
        {
            if (source != null && source.isPlaying)
            {
                source.Stop();
            }
        }
    }

    public void RemoveAudioSource(AudioSource source)
    {
        if (source != null && activeAudioSources.Contains(source))
        {
            StopAudio(source);
            activeAudioSources.Remove(source);
        }
    }

   

    void OnDestroy()
    {
        StopAllAudio();
        activeAudioSources.Clear();
    }

    // Interface properties remain unchanged

    public AudioSource PanelOpenSource => panelOpenSource;
    public AudioSource PanelCloseSource => panelCloseSource;
    public AudioSource ButtonClickSource => buttonClickSource;
    public AudioSource ImageClickingSource => imageclickingSource;
    public AudioSource ErrorSource => errorSource;

}
public enum AudioType
{
   
    PanelOpen,
    PanelClose,
    ButtonClick,
    ImageClicking,
    Error,

}