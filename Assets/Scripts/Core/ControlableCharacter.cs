using UnityEngine;

namespace Core
{
    public class ControlableCharacter : MonoBehaviour
    {
        [Header("Main")]
        public Skeleton CharactersSkeleton;
        public MotionCaption MotionCaption;

        [Header("Options")]
        public bool[] IsControable;
        public byte[] CompareMap;

        private Transform[] _boneContainrs;


        // Start is called before the first frame update
        void Start()
        {
            if (!MotionCaption) throw new UnityException("The field 'FromSkeleton' is null, when it can't be null");
            if (!CharactersSkeleton) throw new UnityException("The field 'CharactersSkeleton' is null, when it can't be null");

            _boneContainrs = new Transform[CharactersSkeleton.bones.Length];

            for (int i = 0; i < _boneContainrs.Length; i++)
            {
                _boneContainrs[i] = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
                GameObject.Destroy(_boneContainrs[i].GetComponent<MeshFilter>());
                GameObject.Destroy(_boneContainrs[i].GetComponent<MeshRenderer>());
                GameObject.Destroy(_boneContainrs[i].GetComponent<BoxCollider>());

                _boneContainrs[i].parent = CharactersSkeleton.bones[i].parent;
                _boneContainrs[i].position = CharactersSkeleton.bones[i].position;

                if (i < IsControable.Length)
                {
                    if (IsControable[i])
                    {
                        _boneContainrs[i].parent = CharactersSkeleton.root;
                        _boneContainrs[i].Rotate(new Vector3(0, 180, 0), Space.World);
                    }
                }

                CharactersSkeleton.bones[i].parent = _boneContainrs[i];
            }
        }

        // Update is called once per frame
        void Update()
        {
            for (byte i = 0; i < _boneContainrs.Length; i++)
            {
                _boneContainrs[i].rotation = MotionCaption.GetSensorRotation(i < CompareMap.Length ? CompareMap[i] : i);
            }
        }
    }
}