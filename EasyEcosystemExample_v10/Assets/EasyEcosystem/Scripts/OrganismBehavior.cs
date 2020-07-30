using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganismBehavior : MonoBehaviour
{
    public float lifeTime;
    public float lifeTimeRandomSeed;
    public float fertility;
    public float reproductionFrequency;
    public int popMin;
    public int popMax;
    public float pollFrequency;
    public GameObject popController;
    public string speciesTag;

    public float timerLife;
    private float timerBirth;
    private int popCount;

    // Start is called before the first frame update
    void Start()
    { }
    // Update is called once per frame
    void Update()
    { }

    public void OrganismStart()
    {
        timerLife = lifeTime + Random.Range(0, lifeTimeRandomSeed);
        timerBirth = reproductionFrequency;
        
    }
    
    public void OrganismUpdate()
    {
        timerLife -= Time.deltaTime;
        timerBirth -= Time.deltaTime;
        //If time since creation is greater than reproduction frequencey, 
        //roll a chance based on fertility to decide if organism will give birth.
        if (timerBirth <= 0.0f) {
            BirthDecision(popController);
            timerBirth = reproductionFrequency;
        }
        //If time since creation is greather than lifetime, check if population is greater than popMax
        //if this evaluates to true, then the gameobject should die
        if (timerLife <= 0.0f) {
            Death(popController);
        }


    }

    //contacts the population controller and returns the count of game objects with that tag.
    public int CountPop(GameObject controller, string tag) { 
        PopulationController popController = controller.GetComponent(typeof(PopulationController)) as PopulationController;
        return popController.GetPops()[tag];
    }

    void BirthDecision(GameObject controller) {
        if (CountPop(controller, speciesTag) < popMax && Random.Range(0, 1) < fertility) {
            Birth();
        }
        else if (CountPop(controller, speciesTag) >= popMax ) {
            Debug.Log(speciesTag + "Population max reached, infertile");
        }
    }

    //blank method, to be overriden by extension classes with their own method for spawning offspring;
    public virtual void Birth() {
        

    }

    void Death(GameObject controller) {
        if (CountPop(controller, gameObject.tag) > popMin)
        {
            GameObject.Destroy(gameObject);
        }
        else {
            Debug.Log(speciesTag + " Species endagered, no death");
            timerLife = lifeTime;
        }
    }
}
