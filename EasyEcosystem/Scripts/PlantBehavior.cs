using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBehavior : OrganismBehavior
{
    public float germinationForce;
    public GameObject offspringType;

    // Start is called before the first frame update
    void Start()
    {
        OrganismStart();
    }

    // Update is called once per frame
    void Update()
    {
        OrganismUpdate();
    }
    
    public override void Birth() {
        base.Birth();
        GameObject clone = Instantiate(offspringType, transform.position + new Vector3(0,5,0), transform.rotation);
        clone.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-1* germinationForce, germinationForce), Random.Range(0.0f, germinationForce), Random.Range(-1 * germinationForce, germinationForce));
    }
}
