using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "MemoryGame/Level")]
public class LevelData : ScriptableObject
{
    public string levelName;
    public int rows;
    public int columns;
    public float revealDelay = 1f;
    public float flipBackDelay = 0.6f;
}