using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AnimationRecorder : MonoBehaviour
{
    [Header("Recording Variables")]
    public int recordingFrameLength = 5 * 50;  //In frames
    public List<GameObject> objectsToRecord;
    public List<RecordingData> recordingDataList;

    //Debug
    private Coroutine playback = null;

    //private void Start()
    //{
    //    GetInitialState();
    //    StartCoroutine(StartRecording());
    //}

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

    public void SetRecordTargets(List<GameObject> targets)
    {
        objectsToRecord = targets;
        GetInitialState();
        StartRecording();
        //StartCoroutine(StartRecording());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F11))
        {
            PlayRecording();
        }

        if (Input.GetKeyDown(KeyCode.F10))
        {
            RewindRecording();
        }
    }

    public void GetInitialState()
    {
        foreach (var gameObject in objectsToRecord)
        {
            Vector3 initialPosition = gameObject.transform.position;
            Quaternion initialRotation = gameObject.transform.rotation;

            Rigidbody rb = gameObject.GetComponent<Rigidbody>();

            recordingDataList.Add(new RecordingData(rb, initialPosition, initialRotation));
        }
    }

    public void ResetToInitialState()
    {
        for (int i = 0; i < objectsToRecord.Count; i++)
        {
            objectsToRecord[i].transform.position   = recordingDataList[i].initialPosition;
            objectsToRecord[i].transform.rotation   = recordingDataList[i].initialRotation;
        }
    }

    public void StartRecording()
    {
        //Clear the recorded animation
        foreach (var data in recordingDataList)
        {
            data.recordedAnimation.Clear();
        }

        Physics.simulationMode = SimulationMode.Script;

        //Begin recording position and rotation for every frame
        for (int i = 0; i < recordingFrameLength; i++)
        {
            //For every gameObject
            for (int j = 0; j < objectsToRecord.Count; j++)
            {
                Vector3 position = objectsToRecord[j].transform.position;
                Quaternion rotation = objectsToRecord[j].transform.rotation;
                recordingDataList[j].recordedAnimation.Add(new RecordedFrame(position, rotation));
            }
            Physics.Simulate(Time.fixedDeltaTime);
        }

        Physics.simulationMode = SimulationMode.FixedUpdate;

        DisablePhysics();
        ResetToInitialState();
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
            }
            yield return new WaitForFixedUpdate();
        }

        EnablePhysics();

        playback = null;
    }

    #region
    //Just for funsies, I added a rewind mechanic
    private Coroutine rewind = null;

    public void RewindRecording()
    {
        if (rewind == null && recordingDataList.Count > 0)
        {
            rewind = StartCoroutine(RewindAnimation());
        }
    }

    private IEnumerator RewindAnimation()
    {
        //Disable Rigidbody
        for (int i = 0; i < recordingDataList.Count; i++)
        {
            recordingDataList[i].rb.useGravity = false;
            recordingDataList[i].rb.isKinematic = true;
        }

        //Play the animation frame by frame
        for (int i = recordingFrameLength - 1; i > 0; i--)
        {
            //For every objects
            for (int j = 0; j < recordingDataList.Count; j++)
            {
                Vector3 position = recordingDataList[j].recordedAnimation[i].position;
                Quaternion rotation = recordingDataList[j].recordedAnimation[i].rotation;

                objectsToRecord[j].transform.position = position;
                objectsToRecord[j].transform.rotation = rotation;
            }
            yield return new WaitForFixedUpdate();
        }

        rewind = null;
    }
    #endregion

    /// <summary>
    /// For optimization, this function is to check if the dice has stopped moving.
    /// We can then stop recording this dice.
    /// </summary>
    /// <param name="rb"></param>
    /// <returns></returns>
    public bool CheckObjectHasStopped(Rigidbody rb)
    {
        if (rb.velocity == Vector3.zero && rb.angularVelocity == Vector3.zero)
        {
            return true;
        }
        else return false;
    }


    [System.Serializable]
    public struct RecordedFrame
    {
        public Vector3 position;
        public Quaternion rotation;

        public RecordedFrame(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
    }

    [System.Serializable]
    public struct RecordingData
    {
        public Rigidbody rb;
        public Vector3 initialPosition;
        public Quaternion initialRotation;
        public List<RecordedFrame> recordedAnimation;

        public RecordingData(Rigidbody rb, Vector3 initialPosition, Quaternion initialRotation)
        {
            this.rb = rb;
            this.initialPosition = initialPosition;
            this.initialRotation = initialRotation;
            this.recordedAnimation = new List<RecordedFrame>();
        }
    }
}
