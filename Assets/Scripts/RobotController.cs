using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    //Vector3 rot = Vector3.zero;
    Vector3 rot = new Vector3(0, 180, 0);
    float rotSpeed = 40f;
    Animator anim;

    public VoiceController voiceController;

    // Use this for initialization
    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        gameObject.transform.eulerAngles = rot;
    }

    // Update is called once per frame
    void Update()
    {
        CheckVoice();
        gameObject.transform.eulerAngles = rot;
    }

    void CheckVoice()
    {
        //Walk
        if (voiceController.robotWork == "walk")
        {
            anim.SetBool("Walk_Anim", true);
        }
        else if (voiceController.robotWork == "stop")
        {
            anim.SetBool("Walk_Anim", false);
        }
        // Rotate Left
        if (voiceController.robotWork == "left")
        {
            rot[1] -= rotSpeed * Time.fixedDeltaTime;
        }
        else if (voiceController.robotWork == "left stop")
        {
            rot[1] -= 0f;
        }
        // Rotate Right
        if (voiceController.robotWork == "right")
        {
            rot[1] += rotSpeed * Time.fixedDeltaTime;
        }
        else if (voiceController.robotWork == "right stop")
        {
            rot[1] += 0f;
        }
        // Roll
        if (voiceController.robotWork == "rolling")
        {
            if (!anim.GetBool("Roll_Anim"))
            {
                anim.SetBool("Roll_Anim", true);
            }
        }
        if (voiceController.robotWork == "rolling stop")
        {
            if (anim.GetBool("Roll_Anim"))
            {
                anim.SetBool("Roll_Anim", false);
            }
        }
        // Close
        if (voiceController.robotWork == "close")
        {
            if (anim.GetBool("Open_Anim"))
            {
                anim.SetBool("Open_Anim", false);
            }
        }
        // Open
        if (voiceController.robotWork == "open")
        {
            if (!anim.GetBool("Open_Anim"))
            {
                anim.SetBool("Open_Anim", true);
            }
        }

        /*
        // Walk
        if (Input.GetKey(KeyCode.W))
        {
            anim.SetBool("Walk_Anim", true);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            anim.SetBool("Walk_Anim", false);
        }

        // Rotate Left
        if (Input.GetKey(KeyCode.A))
        {
            rot[1] -= rotSpeed * Time.fixedDeltaTime;
        }

        // Rotate Right
        if (Input.GetKey(KeyCode.D))
        {
            rot[1] += rotSpeed * Time.fixedDeltaTime;
        }

        // Roll
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (anim.GetBool("Roll_Anim"))
            {
                anim.SetBool("Roll_Anim", false);
            }
            else
            {
                anim.SetBool("Roll_Anim", true);
            }
        }

        // Close
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!anim.GetBool("Open_Anim"))
            {
                anim.SetBool("Open_Anim", true);
            }
            else
            {
                anim.SetBool("Open_Anim", false);
            }
        }
        */
    }
}
