using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTouchScript : MonoBehaviour
{
    public int collisionCount = 0;
    private string wheelName;
    private int wheelNum;
    private ScooterScript scooter;

    // Start is called before the first frame update
    void Start()
    {
        wheelName = gameObject.name;
        switch (wheelName)
        {
            case "Back Wheel":
                wheelNum = 0;
                break;

            case "Front Wheel":
                wheelNum = 1;
                break;
        }
        scooter = GameObject.Find("Scooter").GetComponent<ScooterScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        ++collisionCount;
        scooter.SendMessage("WheelTouchedGround", wheelNum);
    }
    private void OnCollisionExit(Collision collision)
    {
        --collisionCount;
        if(collisionCount == 0)
        {
            scooter.SendMessage("WheelLeftGround", wheelNum);
        }
    }
}
