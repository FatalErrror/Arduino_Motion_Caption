using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public class MotionCaption : MonoBehaviour
    {
        public const string PLAYERPREF_NAME = "PortReader";

        public DataReceivedEvents Log;


        public Quaternion GetSensorRotation(byte sensorNumber)
        {
            if (sensorNumber >= _sensors.Length) return Quaternion.identity;
            Vector4 Data = _sensors[sensorNumber].Rotation;
            return  new Quaternion(Data.y, -Data.x, Data.z, -Data.w);
        }

        public void ResetDMP()
        {
            for (int i = 0; i < _sensors.Length; i++)
            {
                _sensors[i].ResetDMP();
            }
        }

        public void Calibrate()
        {
            StartCoroutine(CalibrateCoroutine());
        }

        public void RestartPortReader()
        {
            StopPortReader();
            StartPortReader();
        }

        public void SetSensorUseFilter(bool b)
        {
            for (int i = 0; i < _sensors.Length; i++)
            {
                _sensors[i].UseFilter = b;
            }
        }

        public void SetSensorFilterThrashold(float f)
        {
            for (int i = 0; i < _sensors.Length; i++)
            {
                _sensors[i].Thrashold = f;
            }
        }

        //==================================================================
        private const byte SENSORS_COUNT = 8;

        private VirtualSensor[] _sensors = new VirtualSensor[SENSORS_COUNT];
        private PortReader _portReader;
        private Queue<string> _data = new Queue<string>();

        private void Awake()
        {
            for (int i = 0; i < _sensors.Length; i++)
            {
                _sensors[i] = new VirtualSensor();
            }
            StartPortReader();
        }

        private void Update()
        {
            DataParser();
        }

        private void StartPortReader()
        {
            _portReader = new PortReader(PlayerPrefs.GetString(PLAYERPREF_NAME));
            if (_portReader != null)
            {
                _portReader.DataReceived += AsyncDataParser;
                try
                {
                    _portReader.Begin();
                }
                catch (System.Exception e)
                {
                    Log.Log("ERROR: " + e.Message);
                }
            }
        }

        private void StopPortReader()
        {
            if (_portReader != null)
            {
                _portReader.DataReceived -= AsyncDataParser;
                try
                {
                    _portReader.End();
                    _portReader.End();
                }
                catch (System.Exception e)
                {
                    Log.Log("ERROR: " + e.Message);
                }
            }
        }

        private void AsyncDataParser(string data)
        {
            _data.Enqueue(data);
        }

        private void DataParser()
        {
            for (int i = 0; i < _data.Count; i++)
            {
                var stream = _data.Dequeue();
                if (stream[0] == '|')
                {
                    Log.UpdateLogField(stream);
                    ParaseSensorsData(stream);
                }
                else
                {
                    if (stream[0] == '>' || stream[0] == '.' || stream[0] == '*')
                    {
                        Log.AddToPrevLog(stream);
                        Log.AddToLogField(stream);
                    }
                    else
                    {
                        Log.Log(stream);
                        Log.UpdateLogField(stream);
                    }
                }
            }
            for (int i = 0; i < _sensors.Length; i++)
            {
                _sensors[i].UpdateTransform();
            }
        }

        private void ParaseSensorsData(string data)
        {
            string[] datas = data.Split('|');
            for (int i = 1; i < datas.Length; i++)
            {
                string[] data1 = datas[i].Split('=');
                int index = int.Parse(data1[0]);
                string[] data2 = data1[1].Split('_');
                float[] data3 = new float[data2.Length - 1];
                for (int j = 0; j < data3.Length; j++)
                {
                    data3[j] = StrToF(data2[j + 1]);
                }

                _sensors[index].UpdateData(new SensorData(long.Parse(data2[0]), data3));
            }
        }

        private float StrToF(string value)
        {
            return float.Parse(value, CultureInfo.GetCultureInfo("en-GB"));
        }
        
        private IEnumerator<WaitForSecondsRealtime> CalibrateCoroutine()
        {
            yield return new WaitForSecondsRealtime(4f);
            ResetDMP();
        }
    }

    public struct SensorData
    {
        public long deltaTime;
        public float gx;
        public float gy;
        public float gz;
        public float ax;
        public float ay;
        public float az;

        public SensorData(long deltaTime, float[] data)
        {
            this.deltaTime = deltaTime;
            gx = data[0];
            gy = data[1];
            gz = data[2];
            ax = data[3];
            ay = data[4];
            az = data[5];
        }
    }

    [Serializable]
    public class StringUnityEvent : UnityEvent<string> { }

    [Serializable]
    public class DataReceivedEvents
    {
        public StringUnityEvent LogEvent;
        public StringUnityEvent AddToPrevLogEvent;
        public StringUnityEvent UpdateLogFieldEvent;
        public StringUnityEvent AddToLogFieldEvent;

        public DataReceivedEvents()
        {
            LogEvent = new StringUnityEvent();
            AddToPrevLogEvent = new StringUnityEvent();
            UpdateLogFieldEvent = new StringUnityEvent();
            AddToLogFieldEvent = new StringUnityEvent();
        }

        public void Log(string data)
        {
            LogEvent.Invoke(data);
        }

        public void AddToPrevLog(string data)
        {
            AddToPrevLogEvent.Invoke(data);
        }

        public void UpdateLogField(string data)
        {
            UpdateLogFieldEvent.Invoke(data);
        }

        public void AddToLogField(string data)
        {
            AddToLogFieldEvent.Invoke(data);
        }
    }
}
