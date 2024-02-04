using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Plant Stats", fileName = "PlantStats")]
public class PlantStats : ScriptableObject
{
    [Header("Basic info")]
    [SerializeField] public string plantName;
    [SerializeField] public int maxHealth;
    [SerializeField] public int seedlingAirPollutionBonus;
    [SerializeField] public int sproutAirPollutionBonus;
    [SerializeField] public int juvenileAirPollutionBonus;
    [SerializeField] public int matureAirPollutionBonus;
    [SerializeField] public GameObject cost;
    public enum PlantStage { SEEDLING, SPROUT, JUVENILE, MATURE}

    [Header("Images")]
    [SerializeField] public Sprite seedlingImage;
    [SerializeField] public Sprite sproutImage;
    [SerializeField] public Sprite juvenileImage;
    [SerializeField] public Sprite matureImage;

    [SerializeField] public float seedlingTileOffset;
    [SerializeField] public float sproutTileOffset;
    [SerializeField] public float juvenileTileOffset;
    [SerializeField] public float matureTileOffset;

    [Header("Scale on Different Growth Stages")]
    [SerializeField] public float seedlingScale;
    [SerializeField] public float sproutScale;
    [SerializeField] public float juvenileScale;
    [SerializeField] public float matureScale;

    [Header("Growth Time for Growth Stages")]
    [SerializeField] public float seedlingGrowTime;
    [SerializeField] public float sproutGrowTime;
    [SerializeField] public float juvenileGrowTime;

}
