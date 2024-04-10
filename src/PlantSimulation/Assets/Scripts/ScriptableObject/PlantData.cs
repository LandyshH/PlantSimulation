using Assets.Scripts.Enum;
using Assets.Scripts.ProceduralGeneration.Sunflower.Enum;
using UnityEngine;

[CreateAssetMenu]
public class PlantData : ScriptableObject
{
    public float stemHeight;
    public float stemWidth;
    public float leafWidth;
    public float leafHeight;
    public int leafCount;
    public int petalCount;
    public LeafAppearance leafAppearance;
    public StemAppearance stemAppearance;
    public PlantGrowthStage Stage;
}
