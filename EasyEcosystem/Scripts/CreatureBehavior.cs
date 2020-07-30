using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class CreatureBehavior : OrganismBehavior
{

    
    public int preyPopMin;
    public float fertilityReductionFactor;
    public float wanderTime;
    public float spawnRadius;
    public float pursuitTime;
    public float eatingRange;
    public float pursuitRange;
    public float wanderRadius;
    public float lifeExtensionBase;
    public float lifeExtensionFactor;
    public float wanderPointProximity;
    public GameObject offspringType;
    public float germinationForce;
    public bool procreatesOnEat;
    public List<string> preyList;
    public List<string> helperList;
    public List<string> hurterList;

    private UnityEngine.AI.NavMeshAgent agent;
    private string prey;
    private string helper;
    private string hurter;
    private bool inPursuit;
    private GameObject target;
    private float timer;
    private int mealsEaten;

    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        inPursuit = false;
        OrganismStart();
        Wander();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        OrganismUpdate();
        if (target == null)
        {
            Wander();
        }
        else if (inPursuit)
        {
            Pursue();
        }
        else {
            Wander();
        }
    }

    public override void Birth()
    {
        base.Birth();

        if (!procreatesOnEat) {
            if (CountPop(popController, speciesTag) < popMax && Random.Range(0, 1) < fertility)
            {
                GameObject clone = Instantiate(offspringType, gameObject.transform.position, transform.rotation);}
        }
    }
    public void OnTriggerEnter(Collider col)
    {
        HandleCollission(col);
    }
    public void OnTriggerExit(Collider col)
    {
        HandleCollission(col);
    }

    public void HandleCollission(Collider col) {
        string tag = col.gameObject.tag;
        if (preyList.Contains(tag)) {
            prey = tag;
            if (!inPursuit && CountPop(popController, prey) > preyPopMin)
            {
                target = col.gameObject;
                HandlePrey();
            }
        }
        else if (helperList.Contains(tag))
        {
            //HandleHelper();
        }
        else if (hurterList.Contains(tag))
        {
            //HandleHurter();
        }
    }

    public void HandlePrey() {
        ResetTimer();
        inPursuit = true;
        }

    public void ResetTimer()
    {
        timer = 0.0f;
    }
    public void Pursue() {
        if (agent.enabled == true) { 
            agent.destination = target.transform.position;
            if (timer >= pursuitTime)
            {
                ResetTimer();
                inPursuit = false;
                target = null;

            }
            else if (agent.remainingDistance <= eatingRange)
            {
                Eat();
            }
        }
    }

    public void Wander() {
        if (agent.enabled == true) {
            if (timer >= wanderTime || agent.remainingDistance <= wanderPointProximity)
            {
                ResetTimer();
                SetRandomDestination();
            }
        }

    }
    public void SetRandomDestination() {
        if (agent.enabled == true) {
            Vector3 p = gameObject.transform.position;
            Vector3 randomPosition = (Random.insideUnitSphere * wanderRadius) + p;
            UnityEngine.AI.NavMeshHit hit;
            UnityEngine.AI.NavMesh.SamplePosition(randomPosition, out hit, wanderRadius, 1 << NavMesh.GetAreaFromName("Walkable"));
            agent.destination = hit.position;
        }

    }
    public void Eat() {
        if (CountPop(popController, prey) > preyPopMin)
        {
            mealsEaten++;
            float extension = lifeExtensionBase / (1 + (lifeExtensionFactor * mealsEaten));
            timerLife += extension;
            if (target != null)
            {
                inPursuit = false;
                GameObject.Destroy(target);
                CreatureBirth();
            }
        }
        else {
            Wander();
        }
    }
    public void CreatureBirth()
    {
        if (procreatesOnEat) {
            if (CountPop(popController, speciesTag) < popMax && Random.Range(0, 1) < fertility)
            {
                GameObject clone = Instantiate(offspringType, transform.position, transform.rotation);
                fertility = fertility * fertilityReductionFactor;
            }
        }
    }
}
