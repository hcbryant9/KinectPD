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
                rightHandPos[0] = body.Joints[Windows.Kinect.JointType.HandRight].Position.X;
                rightHandPos[1] = body.Joints[Windows.Kinect.JointType.HandRight].Position.Y;
                rightHandPos[2] = body.Joints[Windows.Kinect.JointType.HandRight].Position.Z;

                //if left hand is above head -> complete init
                //minimizes glitches involving walking into camera
                if (leftHandPos[1] > headPos[1] && !initialized)
                {
                    relativePos[0] = body.Joints[Windows.Kinect.JointType.Head].Position.X;
                    relativePos[1] = body.Joints[Windows.Kinect.JointType.Head].Position.Y;
                    relativePos[2] = body.Joints[Windows.Kinect.JointType.Head].Position.Z;
                    
                    initialized = true;
                }
                break;
            }
        }

        //if right hand is above head -> uninit
        //minimizes glitches involving walking away from camera
        if (rightHandPos[1] > headPos[1] && initialized)
        {
            initialized = false;
        }
        if (initialized)
        {
            //Debug.Log("REL : : X: " + relativePos[0] + ", Y: " + relativePos[1] + ", Z: " + relativePos[2]);
            //Debug.Log("HEAD : : X: " + headPos[0] + ", Y: " + headPos[1] + ", Z: " + headPos[2]);

            //left and right motion
            float magnitudeLR = Mathf.Abs(headPos[0] - relativePos[0]);

            //forward and back motion
            float magnitudeFB = Mathf.Abs(headPos[2] - relativePos[2]);


            if (magnitudeLR > 0.2)
            {
                if (headPos[0] > relativePos[0])
                {
                    
                    pdPatch.SendFloat("test", magnitudeLR*400);
                  
                    Debug.Log("you went right and sent a message to PD (maybe) here is frequency you sent: " + magnitudeLR * 400);
                    //if the player leans to the right, go right
                    //player.transform.Translate(-player.transform.right * 0.125f, Space.World);
                }
                else
                {
                    pdPatch.SendFloat("test", magnitudeLR * 400);

                    Debug.Log("you went right and sent a message to PD (maybe) here is frequency you sent: " + magnitudeLR * 400);
                    //if the player leans to the left, go left
                    //player.transform.Translate(player.transform.right * 0.125f, Space.World);

                }
            }
            else if (magnitudeFB > 0.1)
            {
                if (headPos[2] > relativePos[2])
                {
                    //player.transform.Translate(player.transform.forward * 0.125f, Space.World);
                }
                else
                {
                    //player.transform.Translate(-player.transform.forward * 0.125f, Space.World);
                }
                
            }
            

        }
    }
}
