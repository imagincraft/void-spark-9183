using System;
using UnityEngine;
using System.Collections.Generic;

public static class LevelEvents
{
    public static Action<LevelData> OnLevelLoaded;
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

    public LevelData CurrentLevel { get; private set; }
    private int currentIndex = 0;

    private void Awake()
    {
   // ResetSave();
    }
    public void ResetSave()
    {
        PlayerPrefs.DeleteKey("MemoryGameSave");
        PlayerPrefs.Save();
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
        LoadLevel(currentIndex + 1);
        UnlockNextLevel();
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