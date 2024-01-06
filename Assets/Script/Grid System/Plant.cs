using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Creatable
{
    [SerializeField] PlantStats stats;
    //float plantTime;
    PlantStats.PlantStage currentPlantStage;
    //Sprite currentImage;
    public GameObject spriteObject;

    // Start is called before the first frame update
    void Awake()
    {
        //plantTime = Time.deltaTime;
        currentPlantStage = PlantStats.PlantStage.SEEDLING;
        spriteObject.transform.position = new Vector3(this.gameObject.transform.position.x, stats.seedlingYOffset, this.gameObject.transform.position.z);
        spriteObject.transform.localScale = new Vector3(stats.seedlingScale, stats.seedlingScale, 1);
        StartCoroutine(AdvancePlantStage());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    private IEnumerator AdvancePlantStage()
    {
        while (true)
        {
            if (currentPlantStage == PlantStats.PlantStage.SEEDLING)
            {
                yield return new WaitForSeconds(stats.seedlingGrowTime);
                Debug.Log("Plant should be growing");
                currentPlantStage = PlantStats.PlantStage.SPROUT;
                this.GetComponentInChildren<SpriteRenderer>().sprite = stats.sproutImage;
                spriteObject.transform.position = new Vector3(this.gameObject.transform.position.x, stats.seedlingYOffset, this.gameObject.transform.position.z);
                spriteObject.transform.localScale = new Vector3(stats.seedlingScale, stats.seedlingScale, 1);
            }
            else if(currentPlantStage == PlantStats.PlantStage.SPROUT)
            {
                yield return new WaitForSeconds(stats.sproutGrowTime);
                currentPlantStage = PlantStats.PlantStage.JUVENILE;
                this.GetComponentInChildren<SpriteRenderer>().sprite = stats.juvenileImage;
                spriteObject.transform.position = new Vector3(this.gameObject.transform.position.x, stats.juvenileYOffset, this.gameObject.transform.position.z);
                spriteObject.transform.localScale = new Vector3(stats.juvenileScale, stats.juvenileScale, 1);
            }
            else if(currentPlantStage == PlantStats.PlantStage.JUVENILE)
            {
                yield return new WaitForSeconds(stats.juvenileGrowTime);
                currentPlantStage = PlantStats.PlantStage.MATURE;
                this.GetComponentInChildren<SpriteRenderer>().sprite = stats.matureImage;
                spriteObject.transform.position = new Vector3(this.gameObject.transform.position.x, stats.matureYOffset, this.gameObject.transform.position.z);
                spriteObject.transform.localScale = new Vector3(stats.matureScale, stats.matureScale, 1);
            }
            else if(currentPlantStage == PlantStats.PlantStage.MATURE)
            {
                StopCoroutine(AdvancePlantStage());
                yield break;
            }
        }
        
    }
}
