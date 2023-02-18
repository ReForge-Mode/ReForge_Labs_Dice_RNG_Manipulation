using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRecorder : MonoBehaviour
{
    public DiceManager2 diceManager;

    [Header("Recording Variables")]
    public int recordingFrameLength = 5 * 50;  //In frames
    public List<GameObject> objectsToRecord;
    public List<RecordingData> recordingDataList;

    //Debug
    private Coroutine playback = null;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            PlayRecording();
        }
    }

    public void StartSimulation(List<GameObject> targets)
    {
        if (playback != null)
        {
            StopCoroutine(playback);
            playback = null;
        }

        recordingDataList.Clear();
        objectsToRecord.Clear();
        objectsToRecord = targets;

        EnablePhysics();
        GetInitialState();
        StartRecording();
    }

    private void GetInitialState()
    {
        foreach (var gameObject in objectsToRecord)
        {
            Vector3 initialPosition = gameObject.transform.position;
            Quaternion initialRotation = gameObject.transform.rotation;

            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            rb.maxAngularVelocity = 1000;

            RecordingData data = new RecordingData(rb, initialPosition,
                                                       initialRotation);
            recordingDataList.Add(data);
        }
    }

    private void StartRecording()
    {
        Physics.simulationMode = SimulationMode.Script;

        //Begin recording position and rotation for every frame
        for (int i = 0; i < recordingFrameLength; i++)
        {
            //For every gameObject
            for (int j = 0; j < objectsToRecord.Count; j++)
            {
                Vector3 position = objectsToRecord[j].transform.position;
                Quaternion rotation = objectsToRecord[j].transform.rotation;
                bool isContactWithArena = diceManager.diceDataList[j].diceUI.isContactWithFloor;
                bool isContactWithDice = diceManager.diceDataList[j].diceUI.isContactWithDice;
                bool isNotMoving = CheckObjectHasStopped(diceManager.diceDataList[j].rb);

                RecordedFrame frame = new RecordedFrame(position, rotation, isContactWithArena,
                                                        isContactWithDice, isNotMoving);
                recordingDataList[j].recordedAnimation.Add(frame);
            }
            Physics.Simulate(Time.fixedDeltaTime);
        }

        Physics.simulationMode = SimulationMode.FixedUpdate;
    }

    public void PlayRecording()
    {
        if (playback == null && recordingDataList.Count > 0)
        {
            playback = StartCoroutine(PlayAnimation());
        }
    }

    private IEnumerator PlayAnimation()
    {
        DisablePhysics();
        ResetToInitialState();

        //Play the animation frame by frame
        for (int i = 0; i < recordingFrameLength; i++)
        {
            //For every objects
            for (int j = 0; j < recordingDataList.Count; j++)
            {
                Vector3 position = recordingDataList[j].recordedAnimation[i].position;
                Quaternion rotation = recordingDataList[j].recordedAnimation[i].rotation;
                objectsToRecord[j].transform.position = position;
                objectsToRecord[j].transform.rotation = rotation;

                //Play Sound whenever contact happens
                if(recordingDataList[j].recordedAnimation[i].isContactWithArena)
                {
                    diceManager.diceDataList[j].diceUI.PlaySoundRollLow();
                }
                if (recordingDataList[j].recordedAnimation[i].isContactWithDice)
                {
                    diceManager.diceDataList[j].diceUI.PlaySoundRollHigh();
                }

                //When the dice stops rolling, lit the texture
                if(recordingDataList[j].recordedAnimation[i].isNotMoving == true)
                {
                    diceManager.diceDataList[j].diceUI.ShowDiceResult();
                }
            }
            yield return new WaitForFixedUpdate();
        }

        playback = null;
    }

    #region
    //Just for funsies, I added a rewind mechanic
    //private Coroutine rewind = null;

    //public void RewindRecording()
    //{
    //    if (rewind == null && recordingDataList.Count > 0)
    //    {
    //        rewind = StartCoroutine(RewindAnimation());
    //    }
    //}

    //private IEnumerator RewindAnimation()
    //{
    //    //Disable Rigidbody
    //    for (int i = 0; i < recordingDataList.Count; i++)
    //    {
    //        recordingDataList[i].rb.useGravity = false;
    //        recordingDataList[i].rb.isKinematic = true;
    //    }

    //    //Play the animation frame by frame
    //    for (int i = recordingFrameLength - 1; i > 0; i--)
    //    {
    //        //For every objects
    //        for (int j = 0; j < recordingDataList.Count; j++)
    //        {
    //            Vector3 position = recordingDataList[j].recordedAnimation[i].position;
    //            Quaternion rotation = recordingDataList[j].recordedAnimation[i].rotation;

    //            objectsToRecord[j].transform.position = position;
    //            objectsToRecord[j].transform.rotation = rotation;
    //        }
    //        yield return new WaitForFixedUpdate();
    //    }

    //    rewind = null;
    //}
    #endregion

    /// <summary>
    /// For optimization, this function is to check if the dice has stopped moving.
    /// We can then stop recording this dice.
    /// </summary>
    /// <param name="rb"></param>
    /// <returns></returns>
    public bool CheckObjectHasStopped(Rigidbody rb)
    {
        if (rb.velocity == Vector3.zero &&
            rb.angularVelocity == Vector3.zero)
        {
            return true;
        }
        else return false;
    }

    public void ResetToInitialState()
    {
        for (int i = 0; i < objectsToRecord.Count; i++)
        {
            objectsToRecord[i].transform.position = recordingDataList[i].initialPosition;
            objectsToRecord[i].transform.rotation = recordingDataList[i].initialRotation;
        }
    }

    public void EnablePhysics()
    {
        //Enable Rigidbody
        for (int i = 0; i < recordingDataList.Count; i++)
        {
            recordingDataList[i].rb.useGravity = true;
            recordingDataList[i].rb.isKinematic = false;
        }
    }

    public void DisablePhysics()
    {
        //Disable Rigidbody
        for (int i = 0; i < recordingDataList.Count; i++)
        {
            recordingDataList[i].rb.useGravity = false;
            recordingDataList[i].rb.isKinematic = true;
        }
    }


    [System.Serializable]
    public struct RecordedFrame
    {
        public Vector3 position;
        public Quaternion rotation;
        public bool isContactWithArena;
        public bool isContactWithDice;
        public bool isNotMoving;

        public RecordedFrame(Vector3 position, Quaternion rotation,
                             bool isContactWithArena, bool isContactWithDice,
                             bool isNotMoving)
        {
            this.position = position;
            this.rotation = rotation;
            this.isContactWithArena = isContactWithArena;
            this.isContactWithDice = isContactWithDice;
            this.isNotMoving = isNotMoving;
        }
    }

    [System.Serializable]
    public struct RecordingData
    {
        public Rigidbody rb;
        public Vector3 initialPosition;
        public Quaternion initialRotation;
        public List<RecordedFrame> recordedAnimation;

        public RecordingData(Rigidbody rb, Vector3 initialPosition,
                             Quaternion initialRotation)
        {
            this.rb = rb;
            this.initialPosition = initialPosition;
            this.initialRotation = initialRotation;
            this.recordedAnimation = new List<RecordedFrame>();
        }
    }
}
