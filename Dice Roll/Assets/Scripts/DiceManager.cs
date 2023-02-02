using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public GameObject dicePrefab;
    public int generateNumber = 8;
    public List<GameObject> diceList;

    [Header("Initial State")]
    public float maxForce = 50;
    public float maxTorque = 50;

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
        Vector3 initialTorque = new Vector3(Random.Range(0, maxTorque), Random.Range(0, maxTorque), Random.Range(0, maxTorque));

        //Generate dice, Set the position and rotation
        GameObject temp     = Instantiate(dicePrefab, initialPosition, initialRotation);
        Rigidbody rb        = temp.GetComponent<Rigidbody>();
        rb.velocity         = initialForce;
        rb.AddTorque(initialTorque);

        diceList.Add(temp);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
