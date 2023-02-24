using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictDice : MonoBehaviour
{
    public Rigidbody rb;

    [Header("Initial State")]
    public Vector3 initialPosition = Vector3.zero;
    public Vector3 initialRotation;
    public Vector3 initialForce;
    public Vector3 initialTorque;

    [Header("Prediction")]
    public Elements predictedTarget = Elements.None;
    public float simulationTime = 5;

    [Header("Guarantee Alteration")]
    public Elements guaranteeTarget = Elements.None;
    public List<ElementData> elementData;


    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            //Initialize the dice initial state
            Initialize();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            //Simulate it
            StartSimulation();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            //Alter the result
            AlterTheResult();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            //Do the roll
            StartActualPhysics();
        }
    }

    private void Initialize()
    {
        //Randomize position and rotation
        rb.useGravity = false;
        initialPosition = Vector3.up * 5;
        initialRotation = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

        //Add force
        initialForce = new Vector3(Random.Range(0, 50), -Random.Range(0, 50), Random.Range(0, 50));
        initialTorque = new Vector3(Random.Range(0, 50), Random.Range(0, 50), Random.Range(0, 50));

        //Set the position and rotation
        transform.position = initialPosition;
        transform.rotation = Quaternion.Euler(initialRotation);
    }

    private void Reset()
    {
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        transform.position = initialPosition;
        transform.rotation = Quaternion.Euler(initialRotation);
    }

    private void StartActualPhysics()
    {
        //Reset();
        rb.useGravity = true;
        rb.velocity = initialForce;
        rb.angularVelocity = initialTorque;
    }

    private void StartSimulation()
    {
        Reset();

        Physics.simulationMode = SimulationMode.Script;
        rb.useGravity = true;
        rb.velocity = initialForce;
        rb.angularVelocity = initialTorque;

        for (int i = 0; i < simulationTime * 50; i++)       //250 frames of simulation
        {

            Physics.Simulate(Time.fixedDeltaTime);
        }

        // Predict the final position of the dice
        //predictPosition = rb.transform.position;
        //predictRotation = rb.transform.rotation.eulerAngles;
        Physics.simulationMode = SimulationMode.FixedUpdate;

        FindTopFace();
        Reset();
    }

    private void FindTopFace()
    {
        //Since we have all child objects for each face,
        //We just need to find the highest Y value
        int indexFaceTop = 0;
        for (int i = 1; i < elementData.Count; i++)
        {
            if (elementData[indexFaceTop].heightDetector.transform.position.y < elementData[i].heightDetector.transform.position.y)
            {
                indexFaceTop = i;
            }
        }

        predictedTarget = elementData[indexFaceTop].element;
    }

    private void AlterTheResult()
    {
        if(guaranteeTarget != Elements.None)
        {
            Reset();

            Vector2 rotateTo = Vector3.zero;

            //Find element of the same name
            for (int i = 0; i < elementData.Count; i++)
            {
                if(elementData[i].element == predictedTarget)
                {
                    //Find the value for that element
                    if (guaranteeTarget == Elements.Cryo) rotateTo = elementData[i].elementRelativeOrientation.Cryo;
                    if (guaranteeTarget == Elements.Dendro) rotateTo = elementData[i].elementRelativeOrientation.Dendro;
                    if (guaranteeTarget == Elements.Electro) rotateTo = elementData[i].elementRelativeOrientation.Electro;
                    if (guaranteeTarget == Elements.Anemo) rotateTo = elementData[i].elementRelativeOrientation.Anemo;
                    if (guaranteeTarget == Elements.Omni) rotateTo = elementData[i].elementRelativeOrientation.Omni;
                    if (guaranteeTarget == Elements.Geo) rotateTo = elementData[i].elementRelativeOrientation.Geo;
                    if (guaranteeTarget == Elements.Pyro) rotateTo = elementData[i].elementRelativeOrientation.Pyro;
                    if (guaranteeTarget == Elements.Hydro) rotateTo = elementData[i].elementRelativeOrientation.Hydro;
                }
            }

            //Rotate by that amount, with x and y flipped. But why? I don't know
            transform.Rotate(rotateTo);
            //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + rotation);
        }
    }

    /// <summary>
    /// This lists all elements and the index order from 0-7
    /// </summary>
    public enum Elements
    {
        Cryo, Dendro, Electro, Anemo, Omni, Geo, Pyro, Hydro, None
    }

    [System.Serializable]
    public struct ElementData
    {
        public Elements element;
        public GameObject heightDetector;
        public ElementRelativeOrientation elementRelativeOrientation;
    }

    /// <summary>
    /// This struct details all rotation needed to spin the dice to a certain face
    /// </summary>
    [System.Serializable]
    public struct ElementRelativeOrientation
    {
        public Vector2 Cryo;
        public Vector2 Dendro;
        public Vector2 Electro;
        public Vector2 Anemo;
        public Vector2 Omni;
        public Vector2 Geo;
        public Vector2 Pyro;
        public Vector2 Hydro;
    }

}
