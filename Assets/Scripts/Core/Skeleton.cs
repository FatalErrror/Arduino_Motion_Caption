﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class Skeleton : MonoBehaviour
    {
        public enum Indexs : int
        {
            Head = 0,
            Spine = 1,

            LeftArm = 2,
            LeftElbow = 3,
            LeftWrist = 4,
            RightArm = 5,
            RightElbow = 6,
            RightWrist = 7,

            LeftLeg = 8,
            LeftKnee = 9,
            LeftAnkle = 10,
            RightLeg = 11,
            RightKnee = 12,
            RightAnkle = 13,


            Lenght = 14
        }

        [Header("General")]
        [SerializeField] private Transform _root;
        [SerializeField] private Transform _head;
        [SerializeField] private Transform _spine;

        [Header("Up")]
        [Header("Right")]
        [SerializeField] private Transform _rightArm;
        [SerializeField] private Transform _rightElbow;
        [SerializeField] private Transform _rightWrist;
        [Header("Down")]
        [SerializeField] private Transform _rightLeg;
        [SerializeField] private Transform _rightKnee;
        [SerializeField] private Transform _rightAnkle;


        [Header("Up")]
        [Header("Left")]
        [SerializeField] private Transform _leftArm;
        [SerializeField] private Transform _leftElbow;
        [SerializeField] private Transform _leftWrist;
        [Header("Down")]
        [SerializeField] private Transform _leftLeg;
        [SerializeField] private Transform _leftKnee;
        [SerializeField] private Transform _leftAnkle;


        public Transform[] bones { get { return _bones; } }
        public Transform root { get { return _root; } }

        private Transform[] _bones = new Transform[(int)Indexs.Lenght];


        private void Awake()
        {
            _bones[(int)Indexs.Head] = _head;
            _bones[(int)Indexs.Spine] = _spine;

            _bones[(int)Indexs.LeftArm] = _leftArm;
            _bones[(int)Indexs.LeftElbow] = _leftElbow;
            _bones[(int)Indexs.LeftWrist] = _leftWrist;
            _bones[(int)Indexs.RightArm] = _rightArm;
            _bones[(int)Indexs.RightElbow] = _rightElbow;
            _bones[(int)Indexs.RightWrist] = _rightWrist;

            _bones[(int)Indexs.LeftLeg] = _leftLeg;
            _bones[(int)Indexs.LeftKnee] = _leftKnee;
            _bones[(int)Indexs.LeftAnkle] = _leftAnkle;
            _bones[(int)Indexs.RightLeg] = _rightLeg;
            _bones[(int)Indexs.RightKnee] = _rightKnee;
            _bones[(int)Indexs.RightAnkle] = _rightAnkle;

            for (int i = 0; i < _bones.Length; i++)
            {
                if (!_bones[i])
                {
                    _bones[i] = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
                    _bones[i].parent = _root;
                    _bones[i].localPosition = Vector3.zero;
                    GameObject.Destroy(_bones[i].GetComponent<MeshFilter>());
                    GameObject.Destroy(_bones[i].GetComponent<MeshRenderer>());
                    GameObject.Destroy(_bones[i].GetComponent<BoxCollider>());
                }
            }
        }
    }
}