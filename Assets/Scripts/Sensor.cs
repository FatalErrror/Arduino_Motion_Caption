using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public byte SensorNumber = 0;
    public Core.MotionCaption MotionCaption;

    public bool Restart, SetPort;
    public string Port;

    /*private void Start()
    {
        PlayerPrefs.SetString(Core.MotionCaption.PLAYERPREF_NAME, "COM11");
        PlayerPrefs.Save();
    }*/
    // Update is called once per frame
    void Update()
    {
        if (SetPort)
        {
            SetPort = false;
            PlayerPrefs.SetString(Core.MotionCaption.PLAYERPREF_NAME, Port);
            PlayerPrefs.Save();
        }
        if (Restart)
        {
            Restart = false;
            MotionCaption.RestartPortReader();
        }

        transform.localRotation = MotionCaption.GetSensorRotation(SensorNumber);
    }
}
