using Assets.Scripts.Enum;
using Assets.Scripts.ProceduralGeneration.Sunflower.Enum;
using Assets.Scripts.ProceduralGeneration.Sunflower.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ProceduralSunflower : MonoBehaviour
{
    [SerializeField] float stemHeight;
    [SerializeField] float stemWidth;
    [SerializeField] float leafWidth;
    [SerializeField] float leafHeight;
    [SerializeField] int leafCount;
    [SerializeField] int petalCount;
    [SerializeField] LeafAppearance leafAppearance;
    [SerializeField] StemAppearance stemAppearance;
    [SerializeField] PlantGrowthStage Stage;

    public readonly PlantData plantData;


    public SunflowerObjects sunflowerObjects;
    private GameObject sunflower;

    private Vector3 lastStemScale; 

    private void Update()
    {
        if (sunflower != null && sunflower.transform.localScale != lastStemScale)
        {
            lastStemScale = sunflower.transform.localScale;
            RebuildFlower();
        }
    }

    private void Awake()
    {
        Build();
    }

    private void Build()
    {
        switch (Stage)
        {
            case PlantGrowthStage.Embryonic:
                CreateEmbryonicSunflower();
                break;
            case PlantGrowthStage.Juvenile:
                CreateJuvenileSunflower();
                break;
            case PlantGrowthStage.MaturityAndReproduction:
                CreateMatureSunflower();
                break;
            case PlantGrowthStage.Senile:
                CreateSenileSunflower();
                break;
        }
    }

    private void RebuildFlower()
    {
        Destroy(sunflower);
        Build();
    }

    private void CreateLeafs(GameObject stem, GameObject leafPrefab)
    {
        List<Vector3> leafPositions = new List<Vector3>();

        for (int i = 0; i < leafCount; i++)
        {
            float angle = Random.Range(0f, 360f);
            Quaternion rotation = Quaternion.Euler(-60f, angle, 0f);

            float minHeight = stemHeight * 0.05f;
            float maxHeight = stemHeight * 0.25f;

            Debug.Log(minHeight.ToString() + " " + maxHeight.ToString());

            Vector3 leafPosition = stem.transform.position + Vector3.up * Random.Range(minHeight, maxHeight);

            bool tooClose = false;
            foreach (Vector3 existingPosition in leafPositions)
            {
                if (Vector3.Distance(leafPosition, existingPosition) < 0.1f) // 0.2f мин допустимое расстояние между листьями
                {
                    tooClose = true;
                    break;
                }
            }

            if (tooClose)
            {
                continue;
            }

            leafPositions.Add(leafPosition);

            GameObject leaf = Instantiate(leafPrefab, sunflower.transform);
            leaf.transform.localRotation = rotation;
            leaf.transform.position = leafPosition;
            leaf.transform.localScale = new Vector3(leafWidth, leafHeight, 0.5f);
        }
    }

    private void CreatePetals(GameObject bud)
    {
        for (int i = 0; i < petalCount; i++)
        {
            float angle = i * (360f / petalCount);
            Quaternion rotation = Quaternion.Euler(0f, 90f, angle);

            Vector3 petalPosition = bud.transform.position + rotation * Vector3.right * 0.3f + Vector3.left * 0.05f;
            GameObject petal = Instantiate(sunflowerObjects.PetalPrefab, sunflower.transform);

            petal.transform.localRotation = rotation;
            petal.transform.position = petalPosition;
        }
    }

    private GameObject CreateStem()
    {
        var stem = Instantiate(sunflowerObjects.StemPrefab, sunflower.transform);
        stem.transform.localScale = new Vector3(stemWidth, stemWidth, stemHeight);
        stem.transform.localPosition = Vector3.zero;

        return stem;
    }

    private GameObject CreateBud()
    {
        var bud = Instantiate(sunflowerObjects.BudPrefab, sunflower.transform);
        bud.transform.localPosition = new Vector3(0f, stemHeight * 0.25f, 0f);

        return bud;
    }

    private void CreateEmbryonicSunflower()
    {
        sunflower = new GameObject("Sunflower");
        
        var stem = CreateStem();

        //на верхушке стебля
        for (int i = 0; i < 2; i++)
        {
            float angle = i * (360f / 2);
            Quaternion rotation = Quaternion.Euler(235f, angle, 0f);

            Vector3 leafPosition = stem.transform.position + Vector3.up * stemHeight * 0.25f;
            GameObject leaf = Instantiate(sunflowerObjects.LeafPrefab, sunflower.transform);

            leaf.transform.localRotation = rotation;
            leaf.transform.position = leafPosition;

            leaf.transform.localScale = new Vector3(leafWidth, leafHeight, 0.5f);
        }
    }

    private void CreateJuvenileSunflower()
    {
        sunflower = new GameObject("Sunflower");

        var stem = CreateStem();

        for (int i = 0; i < 2; i++)
        {
            float angle = i * (360f / 2);
            Quaternion rotation = Quaternion.Euler(235f, angle, 0f);

            Vector3 leafPosition = stem.transform.position + Vector3.up * stemHeight * 0.25f;
            GameObject leaf = Instantiate(sunflowerObjects.LeafPrefab, sunflower.transform);

            leaf.transform.localRotation = rotation;
            leaf.transform.position = leafPosition;

            leaf.transform.localScale = new Vector3(leafWidth, leafHeight, 0.5f);
        }

        CreateLeafs(stem, sunflowerObjects.LeafPrefab);
    }

    private void CreateMatureSunflower()
    {
        sunflower = new GameObject("Sunflower");

        var stem = CreateStem();

        var bud = CreateBud();

        CreateLeafs(stem, sunflowerObjects.LeafPrefab);

        CreatePetals(bud);

        lastStemScale = sunflower.transform.localScale;
    }

    private void CreateSenileSunflower()
    {
        sunflower = new GameObject("Sunflower");

        var stem = CreateStem();

        var bud = CreateBud();

        CreateLeafs(stem, sunflowerObjects.LeafPrefab);

        CreatePetals(bud);
    }
}
