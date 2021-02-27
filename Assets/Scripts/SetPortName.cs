using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPortName : MonoBehaviour
{
    public Core.MotionCaption MotionCaption;

    public string Name;
    public bool SetName;
    public bool Restart;


    // Update is called once per frame
    void Update()
    {
        if (SetName)
        {
            SetName = false;
            PlayerPrefs.SetString(Core.MotionCaption.PLAYERPREF_NAME, Name);
            PlayerPrefs.Save();
        }
        if (Restart)
        {
            Restart = false;
            MotionCaption.RestartPortReader();
        }
    }
}
