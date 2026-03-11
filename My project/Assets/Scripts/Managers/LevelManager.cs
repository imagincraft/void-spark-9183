using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public static class LevelEvents
{
    public static Action<LevelData> OnLevelLoaded;
    public static Action OnLevelCompleted;
}
public interface ILevelManagerService
{
    LevelData CurrentLevel { get; }
    void LoadLevel(int index);
    void LoadNextLevel();
}

public class LevelManager : MonoBehaviour, ILevelManagerService
{
    [SerializeField] private List<LevelData> levels;
    [SerializeField] private GameObject wonPanel;

    public LevelData CurrentLevel { get; private set; }
    private int currentIndex = 0;
    
    private IUiManagerService uiManager;
    private IAudioService audioService;

    private void Awake()
    {
   // ResetSave();
    }

    private void Start()
    {
        uiManager = GameManager.Instance.UiManagerService;
        audioService = GameManager.Instance.AudioService;
    }

    public void ResetSave()
    {
        PlayerPrefs.DeleteKey("MemoryGameSave");
        PlayerPrefs.Save();
    }

    
    private void OnEnable()
    {
        LevelEvents.OnLevelCompleted += HandleLevelCompleted;
    }

    private void OnDisable()
    {
        LevelEvents.OnLevelCompleted -= HandleLevelCompleted;
    }
    
    
    
    public void LoadLevel(int index)
    {
        if (index < 0 || index >= levels.Count)
        {
            Debug.LogError("Invalid level index!");
            return;
        }

        currentIndex = index;
        CurrentLevel = levels[currentIndex];

        // Notify other systems
        LevelEvents.OnLevelLoaded?.Invoke(CurrentLevel);
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadNextLevelDelayed());
    }

    private IEnumerator LoadNextLevelDelayed()
    {
        yield return new WaitForSeconds(1f);

        uiManager.ClosePanel(wonPanel);
        audioService.PlayAudio(AudioType.PanelClose);

        currentIndex++; // move to next level

        if (currentIndex >= levels.Count)
            currentIndex = 0; // loop back to level 1

        LoadLevel(currentIndex);
        UnlockNextLevel();
    }

    
    
    private void HandleLevelCompleted()
    {
        LoadNextLevel();
    }

    
    public void UnlockNextLevel()
    {
        var save = GameManager.Instance.SaveData;

        int maxLevel = levels.Count - 1;

        // Only unlock if not already at max
        if (save.unlockedLevel < maxLevel)
        {
            save.unlockedLevel++;
            SaveSystem.Save(save);
        }
    }


}