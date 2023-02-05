using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public GameObject dicePrefab;
    public AnimationRecorder animRecorder;

    [Header("Initial State")]
    public int generateNumber = 8;
    public float maxForce = 50;

    [Header("Final Result")]
    public List<Elements> alteredResult;

    [Header("Debug")]
    public List<GameObject> diceList;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F12))
        {
            //Remove all dicePrefabs
            foreach (var dicePrefab in diceList)
            {
                Destroy(dicePrefab);
            }
            diceList.Clear();

            //Generate dicePrefabs
            for (int i = 0; i < generateNumber; i++)
            {
                GenerateDice();
            }

            //Start the simulation
            //animRecorder.SetRecordTargets(diceList);
            //each dice
        }
    }

    private void GenerateDice()
    {
        //Randomize X, Y, Z position in the bounding box
        float x = transform.position.x + Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2);
        float y = transform.position.y + Random.Range(-transform.localScale.y / 2, transform.localScale.y / 2);
        float z = transform.position.z + Random.Range(-transform.localScale.z / 2, transform.localScale.z / 2);
        Vector3 initialPosition = new Vector3(x, y, z);

        //Randomize initial rotation
        Quaternion initialRotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

        //Add force
        Vector3 initialForce = new Vector3(Random.Range(0, maxForce), -Random.Range(0, maxForce), Random.Range(0, maxForce));

        //Generate dice, Set the position and rotation
        GameObject temp     = Instantiate(dicePrefab, initialPosition, initialRotation);
        Rigidbody rb        = temp.GetComponent<Rigidbody>();
        rb.velocity         = initialForce;

        diceList.Add(temp);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
