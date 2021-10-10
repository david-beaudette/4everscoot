using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterScript : MonoBehaviour
{
    public Vector2Int wheelOnGround = new Vector2Int(1, 1);

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void WheelTouchedGround(int wheelNum)
    {
        wheelOnGround[wheelNum] = 1;
    }
    void WheelLeftGround(int wheelNum)
    {
        wheelOnGround[wheelNum] = 0;
    }
    public bool IsWheelOnGround(int wheelNum)
    {
        return wheelOnGround[wheelNum] == 1;
    }
    public bool IsScootOnGround()
    {
        return wheelOnGround[0] == 1 || wheelOnGround[1] == 1;
    }

}
