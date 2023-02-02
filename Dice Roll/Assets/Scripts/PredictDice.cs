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
    public List<RecordedFrame> recordedAnimation;
    public Coroutine playback = null;

    [Header("Guarantee Alteration")]
    public Elements guaranteeTarget = Elements.None;
    public List<ElementData> elementData;
    public Vector3 rotateAlteration;


    private void Awake()
    {
        Initialize();
        StartSimulation();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            //Initialize the dice initial state
            Initialize();
            StartSimulation();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            Reset();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            AlterTheResult();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            StartAnimation();
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

        recordedAnimation.Clear();

        //Record the delta
        Quaternion lastFrameRotation = transform.rotation;
        for (int i = 0; i < simulationTime * 50; i++)       //250 frames of simulation
        {
            Quaternion deltaRotation = Quaternion.Euler(transform.rotation.eulerAngles - lastFrameRotation.eulerAngles);
            recordedAnimation.Add(new RecordedFrame(transform.position, deltaRotation));
            lastFrameRotation = transform.rotation;
            Physics.Simulate(Time.fixedDeltaTime);

            if (CheckHasStopped()) break;
        }

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

    private void StartAnimation()
    {
        if (playback == null)
        {
            playback = StartCoroutine(PlayAnimation());
        }
    }

    private IEnumerator PlayAnimation()
    {
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        for (int i = 0; i < recordedAnimation.Count; i++)
        {
            transform.position = recordedAnimation[i].position;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + recordedAnimation[i].deltaRotation.eulerAngles);
            yield return null;
        }

        playback = null;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = true;
        rb.isKinematic = false;
    }

    private void AlterTheResult()
    {
        if(guaranteeTarget != Elements.None)
        {
            Reset();

            rotateAlteration = Vector3.zero;

            //Find element of the same name
            for (int i = 0; i < elementData.Count; i++)
            {
                if(elementData[i].element == predictedTarget)
                {
                    //Find the value for that element
                    if (guaranteeTarget == Elements.Cryo) rotateAlteration = elementData[i].elementRelativeOrientation.Cryo;
                    if (guaranteeTarget == Elements.Dendro) rotateAlteration = elementData[i].elementRelativeOrientation.Dendro;
                    if (guaranteeTarget == Elements.Electro) rotateAlteration = elementData[i].elementRelativeOrientation.Electro;
                    if (guaranteeTarget == Elements.Anemo) rotateAlteration = elementData[i].elementRelativeOrientation.Anemo;
                    if (guaranteeTarget == Elements.Omni) rotateAlteration = elementData[i].elementRelativeOrientation.Omni;
                    if (guaranteeTarget == Elements.Geo) rotateAlteration = elementData[i].elementRelativeOrientation.Geo;
                    if (guaranteeTarget == Elements.Pyro) rotateAlteration = elementData[i].elementRelativeOrientation.Pyro;
                    if (guaranteeTarget == Elements.Hydro) rotateAlteration = elementData[i].elementRelativeOrientation.Hydro;
                }
            }

            //Rotate by that amount, with x and y flipped. But why? I don't know
            transform.Rotate(rotateAlteration);
            //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + rotation);
        }
    }

    public bool CheckHasStopped()
    {
        if (rb.velocity == Vector3.zero && rb.angularVelocity == Vector3.zero)
        {
            return true;
        }
        else return false;
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

    [System.Serializable]
    public struct RecordedFrame
    {
        public Vector3 position;
        public Quaternion deltaRotation;

        public RecordedFrame(Vector3 position, Quaternion deltaRotation)
        {
            this.position = position;
            this.deltaRotation = deltaRotation;
        }
    }
}
