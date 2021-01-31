using UnityEngine;

namespace Core
{
    public class ControlableCharacter : MonoBehaviour
    {
        [Header("Main")]
        public Skeleton CharactersSkeleton;
        public MotionCaption MotionCaption;

        [Header("Options")]
        public byte[] CompareMap;
        public float[] YOffset;

        private Transform[] _boneContainrs;


        // Start is called before the first frame update
        void Start()
        {
            if (!MotionCaption) throw new UnityException("The field 'FromSkeleton' is null, when it can't be null");
            if (!CharactersSkeleton) throw new UnityException("The field 'CharactersSkeleton' is null, when it can't be null");

            _boneContainrs = new Transform[CharactersSkeleton.bones.Length];

            for (int i = 0; i < _boneContainrs.Length; i++)
            {
                _boneContainrs[i] = CreateEmptyObject();
                Transform bone = _boneContainrs[i];
                
                bone.parent = CharactersSkeleton.bones[i].parent;
                bone.position = CharactersSkeleton.bones[i].position;
                bone.Rotate(new Vector3(0, i < YOffset.Length ? 180 - YOffset[i] : 0, 0), Space.World);
                CharactersSkeleton.bones[i].parent = bone;
            }
        }

        private Transform CreateEmptyObject()
        {
            GameObject t1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject.Destroy(t1.GetComponent<MeshFilter>());
            GameObject.Destroy(t1.GetComponent<MeshRenderer>());
            GameObject.Destroy(t1.GetComponent<BoxCollider>());
            t1.name = "container";
            return t1.transform;
        }

        // Update is called once per frame
        void Update()
        {
            for (byte i = 0; i < _boneContainrs.Length; i++)
            {
                _boneContainrs[i].rotation = MotionCaption.GetSensorRotation(i < CompareMap.Length ? CompareMap[i] : i);
                _boneContainrs[i].Rotate(new Vector3(0, i < YOffset.Length ? -YOffset[i] : 0, 0), Space.World);
            }
        }
    }
}