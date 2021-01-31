using UnityEngine;

namespace Core
{
    public class VirtualSensor
    {
        public Vector4 Rotation { get { return _transform; } }

        public bool UseFilter = true;
        public float Thrashold = 0.015f;

        
        private const float TO_RAD = 0.01745329252f;
        private const float TO_DEG_PER_SEC = 16.384f;

        private MadgwickAHRS _dmp;
        private Vector4 _transform;

        private Filters.Vector4Filters.ThrasholdFilterL2 _thrasholdFilter;

        // Start is called before the first frame update
        public VirtualSensor()
        {
            _dmp = new MadgwickAHRS();
            _thrasholdFilter = new Filters.Vector4Filters.ThrasholdFilterL2(Thrashold);
        }

        public void ResetDMP()
        {
            _dmp = new MadgwickAHRS();
        }

        private Vector4 Filtrating(Vector4 data)
        {
            _thrasholdFilter.Thrashold = Thrashold;
            _thrasholdFilter.NewValue(data);

            if (UseFilter)
            {
                return _thrasholdFilter.GetValue();
            }
            return data;
        }

        public void UpdateData(SensorData data)
        {
            data.gx = data.gx * TO_RAD / TO_DEG_PER_SEC;
            data.gy = data.gy * TO_RAD / TO_DEG_PER_SEC;
            data.gz = data.gz * TO_RAD / TO_DEG_PER_SEC;
            _dmp.UpdateIMU(data.deltaTime / 1000f, data.gx, data.gy, data.gz, data.ax, data.ay, data.az);
        }

        public void UpdateTransform()
        {
            Vector4 Data = _dmp.GetQuaternion();
            Data = Filtrating(Data);
            _transform = Data;
        }
    }
}