using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager2 : MonoBehaviour
{
    public GameObject dicePrefab;
    public AnimationRecorder animRecorder;
    public int generateAmount = 8;
    public List<Elements> targetedResult;
    public List<DiceData> diceDataList;

    public void ThrowTheDice()
    {
        GenerateDice(generateAmount);

        //Generate list of dices, then put it into the simulation
        List<GameObject> diceList = new List<GameObject>();
        for (int i = 0; i < generateAmount; i++)
        {
            diceList.Add(diceDataList[i].diceObject);
        }
        animRecorder.StartSimulation(diceList);

        //Record the dice roll result
        for (int i = 0; i < generateAmount; i++)
        {
            int result = diceDataList[i].diceLogic.FindFaceResult();
        }

        //Reset and Alter the result FOR NOW, all Cryo
        animRecorder.ResetToInitialState();
        for (int i = 0; i < targetedResult.Count; i++)
        {
            diceDataList[i].diceLogic.RotateDice(((int)targetedResult[i]));
        }

        animRecorder.PlayRecording();
    }

    private void GenerateDice(int count)
    {
        //Object pooling. Only generate dices we need
        if(count > diceDataList.Count)
        {
            int diceToGenerate = count - diceDataList.Count;
            for (int i = 0; i < diceToGenerate; i++)
            {
                DiceData newDiceData = new DiceData(Instantiate(dicePrefab));
                diceDataList.Add(newDiceData);
            }
        }
        //Otherwise, just teleport the dice far away from the arena.
        //We can fetch them later if we want
        else if(count < diceDataList.Count)
        {
            int diceToDispose = diceDataList.Count - count;
            for (int i = diceDataList.Count - diceToDispose - 1;
                     i < diceDataList.Count; i++)
            {
                diceDataList[i].diceObject.transform.position = Vector3.down * 10000;
            }
        }

        //Set the position and rotation
        for (int i = 0; i < count; i++)
        {
            InitialState initial = SetInitialState();

            diceDataList[i].diceLogic.Reset();
            diceDataList[i].diceUI.Reset();
            diceDataList[i].diceObject.transform.position = initial.position;
            diceDataList[i].diceObject.transform.rotation = initial.rotation;
            diceDataList[i].rb.useGravity = true;
            diceDataList[i].rb.isKinematic = false;
            diceDataList[i].rb.velocity = initial.force;
            diceDataList[i].rb.AddTorque(initial.torque, ForceMode.VelocityChange);
        }
    }

    private InitialState SetInitialState()
    {
        //Randomize X, Y, Z position in the bounding box
        float x = transform.position.x + Random.Range(-transform.localScale.x / 2,
                                                       transform.localScale.x / 2);
        float y = transform.position.y + Random.Range(-transform.localScale.y / 2,
                                                       transform.localScale.y / 2);
        float z = transform.position.z + Random.Range(-transform.localScale.z / 2,
                                                       transform.localScale.z / 2);
        Vector3 position = new Vector3(x, y, z);

        x = Random.Range(0, 360);
        y = Random.Range(0, 360);
        z = Random.Range(0, 360);
        Quaternion rotation = Quaternion.Euler(x, y, z);

        x = Random.Range(0, 25);
        y = Random.Range(0, 25);
        z = Random.Range(0, 25);
        Vector3 force = new Vector3(x, -y, z);

        x = Random.Range(0, 50);
        y = Random.Range(0, 50);
        z = Random.Range(0, 50);
        Vector3 torque = new Vector3(x, y, z);

        return new InitialState(position, rotation, force, torque);
    }

    [System.Serializable]
    /// <summary>
    /// The data containing all references to all dices
    /// so we only need to do GetComponent call once in the script
    /// </summary>
    public struct DiceData
    {
        public GameObject diceObject;
        public Rigidbody rb;
        public Dice diceLogic;
        public DiceUI diceUI;

        public DiceData(GameObject diceObject)
        {
            this.diceObject = diceObject;
            this.rb         = diceObject.GetComponent<Rigidbody>();
            this.diceLogic  = diceObject.transform.GetChild(0).GetComponent<Dice>();
            this.diceUI     = diceObject.GetComponent<DiceUI>();
            this.rb.maxAngularVelocity = 1000;
        }
    }

    [System.Serializable]
    /// <summary>
    /// This is a struct to hold all data needed to initialize the dice
    /// </summary>
    public struct InitialState
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 force;
        public Vector3 torque;

        public InitialState(Vector3 position, Quaternion rotation,
                            Vector3 force, Vector3 torque)
        {
            this.position = position;
            this.rotation = rotation;
            this.force = force;
            this.torque = torque;
        }
    }
}

public enum Elements
{
    Cryo, Dendro, Electro, Anemo, Omni, Geo, Pyro, Hydro, Any
}
