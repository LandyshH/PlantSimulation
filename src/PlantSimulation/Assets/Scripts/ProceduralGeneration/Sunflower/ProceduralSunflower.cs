using Assets.Scripts.ProceduralGeneration.Sunflower.Enum;
using Assets.Scripts.ProceduralGeneration.Sunflower.ScriptableObjects;
using UnityEngine;

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

    public SunflowerObjects sunflowerObjects;
    private GameObject sunflower;

    private Vector3 lastStemScale; // Последний размер стебля, чтобы избежать обновления цветка каждый Update

    private void Update()
    {
        // Проверяем, изменился ли размер стебля
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
        sunflower = new GameObject("Sunflower");

        var stem = Instantiate(sunflowerObjects.StemPrefab, sunflower.transform);
        stem.transform.localScale = new Vector3(stemWidth, stemWidth, stemHeight);

        stem.transform.localPosition = Vector3.zero;

        var bud = Instantiate(sunflowerObjects.BudPrefab, sunflower.transform);
        bud.transform.localPosition = new Vector3(0f, stemHeight/4f, 0f);

        for (int i = 0; i < leafCount; i++)
        {
            float angle = i * (360f / leafCount);
            Quaternion rotation = Quaternion.Euler(-60f, angle, 0f);

            Vector3 leafPosition = stem.transform.position + Vector3.up * 0.3f; 
            GameObject leaf = Instantiate(sunflowerObjects.LeafPrefab, sunflower.transform);

            leaf.transform.localRotation = rotation;
            leaf.transform.position = leafPosition;
            // x - ширина 
            //умножение
            leaf.transform.localScale = new Vector3(leafWidth, leafHeight, 0.5f);
        }

        for (int i = 0; i < petalCount; i++)
        {
            float angle = i * (360f / petalCount);
            Quaternion rotation = Quaternion.Euler(0f, 90f, angle);

            Vector3 petalPosition = bud.transform.position + rotation * Vector3.right * 0.3f + Vector3.left * 0.05f; 
            GameObject petal = Instantiate(sunflowerObjects.PetalPrefab, sunflower.transform);

            petal.transform.localRotation = rotation;
            petal.transform.position = petalPosition;
        }

        lastStemScale = sunflower.transform.localScale;
    }

    private void RebuildFlower()
    {
        Destroy(sunflower);
        Build();
    }
}
