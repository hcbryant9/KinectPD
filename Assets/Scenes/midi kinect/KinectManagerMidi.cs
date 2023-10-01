using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;
public class KinectManagerMidi : MonoBehaviour
{
    public LibPdInstance pdPatch;
    GameObject player;
    private BodySourceManager bodyManager;

    private float[] headPos = new float[3];
    private float[] leftHandPos = new float[3];
    private float[] rightHandPos = new float[3];
    private float[] relativePos = new float[3];
    private bool initialized = false;
    private bool isLRCoroutineRunning = false;
    private bool isFBCoroutineRunning = false;

    private double[] melodyNotes = {261.6,293.6,329.6,349.2,391.9,440,493.8};
    
    private double[] bassNotes = { 87.31, 87.30, 97.99, 110, 123.4, 130.8, 130.81 };

    private int lrIndex = 3;
    private int fwIndex = 3;
    void Start()
    {
        bodyManager = GetComponent<BodySourceManager>();
        player = this.gameObject;
    }

    void Update()
    {
        
        //if we can't find a body just return
        if (bodyManager == null)
            return;

        Windows.Kinect.Body[] data = bodyManager.GetData();
        if (data == null)
            return;

        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }
            if (body.IsTracked)
            {
                //head data
                headPos[0] = body.Joints[Windows.Kinect.JointType.Head].Position.X;
                headPos[1] = body.Joints[Windows.Kinect.JointType.Head].Position.Y;
                headPos[2] = body.Joints[Windows.Kinect.JointType.Head].Position.Z;

                //left hand data
                leftHandPos[0] = body.Joints[Windows.Kinect.JointType.HandLeft].Position.X;
                leftHandPos[1] = body.Joints[Windows.Kinect.JointType.HandLeft].Position.Y;
                leftHandPos[2] = body.Joints[Windows.Kinect.JointType.HandLeft].Position.Z;

                //right hand data
                //X value
                rightHandPos[0] = body.Joints[Windows.Kinect.JointType.HandRight].Position.X;
                //Y value
                rightHandPos[1] = body.Joints[Windows.Kinect.JointType.HandRight].Position.Y;
                //Z value
                rightHandPos[2] = body.Joints[Windows.Kinect.JointType.HandRight].Position.Z;
               

                //if left hand is above head -> complete init
                //minimizes glitches involving walking into camera
                if (leftHandPos[0] > rightHandPos[0] && !initialized)
                {
                    relativePos[0] = body.Joints[Windows.Kinect.JointType.Head].Position.X;
                    relativePos[1] = body.Joints[Windows.Kinect.JointType.Head].Position.Y;
                    relativePos[2] = body.Joints[Windows.Kinect.JointType.Head].Position.Z;
                    
                    initialized = true;
                    pdPatch.SendBang("triggerON");
                    
                }
                break;
            }
        }

        //if right hand is above head -> uninit
        //minimizes glitches involving walking away from camera
        if (rightHandPos[1] > headPos[1] && leftHandPos[1] > headPos[1] && initialized)
        {
            //turning off the patch
            pdPatch.SendBang("triggerOFF");
            Debug.Log("TRIGGERED OFF");
            initialized = false;
        }
        if (initialized)
        {
            //Debug.Log("REL : : X: " + relativePos[0] + ", Y: " + relativePos[1] + ", Z: " + relativePos[2]);
            //Debug.Log("HEAD : : X: " + headPos[0] + ", Y: " + headPos[1] + ", Z: " + headPos[2]);
            //Debug.Log("DISTANCE OF HANDS IN THE Y: " + magnitudeHandsY);
            //Debug.Log("L: "+leftHandPos);
            //Debug.Log("R:" +rightHandPos);
            //Debug.Log("INITIALIZED BODY TRACKING DATA");



            float magnitudeHandsX = Mathf.Abs(rightHandPos[0] - leftHandPos[0]);
            float magnitudeHandsY = Mathf.Abs(rightHandPos[1] - leftHandPos[1]);
            
            //left and right motion
            float magnitudeLR = Mathf.Abs(headPos[0] - relativePos[0]);

            //forward and back motion
            float magnitudeFB = Mathf.Abs(headPos[2] - relativePos[2]);

            
            if (magnitudeHandsX < 0.2f && magnitudeHandsX > 0f)
            {
                pdPatch.SendFloat("FB", (float)melodyNotes[0]);
                Debug.Log("0");
            }
            else if (magnitudeHandsX < 0.4f && magnitudeHandsX > 0.2f)
            {
                pdPatch.SendFloat("FB", (float)melodyNotes[1]);
                Debug.Log("1");
            }
            else if (magnitudeHandsX < 0.6f && magnitudeHandsX > 0.4f)
            {
                pdPatch.SendFloat("FB", (float)melodyNotes[2]);
                Debug.Log("2");
            }
            else if (magnitudeHandsX < 0.8f && magnitudeHandsX > 0.6f)
            {
                pdPatch.SendFloat("FB", (float)melodyNotes[3]);
                Debug.Log("3");
            }
            else if (magnitudeHandsX < 1f && magnitudeHandsX > 0.8f)
            {
                pdPatch.SendFloat("FB", (float)melodyNotes[4]);
                Debug.Log("4");
            }
            else if (magnitudeHandsX < 1.2f && magnitudeHandsX > 1f)
            {
                pdPatch.SendFloat("FB", (float)melodyNotes[5]);
                Debug.Log("5");
            }
            else if (magnitudeHandsX < 1.4f && magnitudeHandsX > 1.2f)
            {
                pdPatch.SendFloat("FB", (float)melodyNotes[6]);
                Debug.Log("6");
            }
            else
            {
                pdPatch.SendFloat("FB", (float)melodyNotes[0]);
                Debug.Log("base - magnitude : " + magnitudeHandsX);
            }

            

            // now for the movement in the y
            if (magnitudeHandsY < 0.2f && magnitudeHandsY > 0f)
            {
                pdPatch.SendFloat("LR", (float)bassNotes[0]);
                Debug.Log("0");
            }
            else if (magnitudeHandsY < 0.4f && magnitudeHandsY > 0.2f)
            {
                pdPatch.SendFloat("LR", (float)bassNotes[1]);
                Debug.Log("1");
            }
            else if (magnitudeHandsY < 0.6f && magnitudeHandsY > 0.4f)
            {
                pdPatch.SendFloat("LR", (float)bassNotes[2]);
                Debug.Log("2");
            }
            else if (magnitudeHandsY < 0.8f && magnitudeHandsY > 0.6f)
            {
                pdPatch.SendFloat("LR", (float)bassNotes[3]);
                Debug.Log("3");
            }
            else if (magnitudeHandsY < 1f && magnitudeHandsY > 0.8f)
            {
                pdPatch.SendFloat("LR", (float)bassNotes[4]);
                Debug.Log("4");
            }
            else if (magnitudeHandsY < 1.2f && magnitudeHandsY > 1f)
            {
                pdPatch.SendFloat("LR", (float)bassNotes[5]);
                Debug.Log("5");
            }
            else if (magnitudeHandsY < 1.4f && magnitudeHandsY > 1.2f)
            {
                pdPatch.SendFloat("LR", (float)bassNotes[6]);
                Debug.Log("6");
            }
            else
            {
                pdPatch.SendFloat("LR", (float)bassNotes[0]);
                Debug.Log("base - magnitude : " + magnitudeHandsY);
            }



        }
    }
    
}
