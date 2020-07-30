using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulationController : MonoBehaviour
{
    public Text populationReadOut;
    public float pollFrequency;
    public string[] Tags;
    public List<GameObject> Spawns;
    public int startingPops;
    public float spawnRadius;

    private float timer;
    private Dictionary<string, int> PopCounts;
    private UnityEngine.AI.NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = transform.position;
        Cursor.visible = true;
        timer = pollFrequency;
        PopCounts = new Dictionary<string, int>();
        InitializePopDictionary(Tags);
        SpawnStartingPops();
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0.0f)
        {
            PopCensus(Tags);
            timer = pollFrequency;
        }

    }

    void InitializePopDictionary(string[] Tags) {
        foreach (string tag in Tags) {
            PopCounts.Add(tag, GameObject.FindGameObjectsWithTag(tag).Length);
        }
    }

    void PopCensus(string[] Tags) {
        string a = "";
        foreach (string tag in Tags) {
            PopCounts[tag] = GameObject.FindGameObjectsWithTag(tag).Length;
            a = string.Concat(a, tag + ": " + PopCounts[tag] + "\n");
        }
        if (populationReadOut != null) {
            if (PopCounts.ContainsValue(0))
            {
                populationReadOut.text = "Game Over";
            }
            else
            {
                populationReadOut.text = a;
            }
        }

        
    }

    public Dictionary<string, int> GetPops() {
        return PopCounts;
    }
    public void SpawnStartingPops() {
        foreach (GameObject spawn in Spawns) {
            for (int i = 0; i < startingPops; i++)
            {
                Vector3 p = gameObject.transform.position;
                Vector3 randomPosition = (Random.insideUnitSphere * spawnRadius) + p;
                UnityEngine.AI.NavMeshHit hit;
                UnityEngine.AI.NavMesh.SamplePosition(randomPosition, out hit, spawnRadius, 1);
                GameObject baby = Instantiate(spawn, hit.position, transform.rotation);
            }
        }
    }
}
