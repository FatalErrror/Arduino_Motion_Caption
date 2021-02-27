using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PPTX", menuName = "ScriptableObjects/PPTX", order = 1)]
[SerializeField]
public class PPTX : ScriptableObject
{
    [SerializeField] public string PPTXName;
    [SerializeField] public Sprite[] Slides;
}
