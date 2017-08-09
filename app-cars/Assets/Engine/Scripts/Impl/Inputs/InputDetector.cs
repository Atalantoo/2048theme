using UnityEngine;
using System;

namespace Commons.Inputs
{
    public class InputDetector : MonoBehaviour
    {
        public delegate void Move();
        public Move Left;
        public Move Right;
        public Move Up;
        public Move Down;
    }
}