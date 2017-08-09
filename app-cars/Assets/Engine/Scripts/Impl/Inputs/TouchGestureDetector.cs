using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Commons.Inputs
{
    // http://answers.unity3d.com/questions/600148/detect-swipe-in-four-directions-android.html
    class TouchGestureDetector : InputDetector
    {
        private const int MARGIN_IN_DEGREE = 10;

        private bool swiping = false;
        private bool eventSent = false;
        private Vector2 lastPosition;

        void Update()
        {
            if (Input.touchCount == 0)
                return;

            if (Input.GetTouch(0).deltaPosition.sqrMagnitude != 0)
            {
                if (swiping == false)
                {
                    swiping = true;
                    lastPosition = Input.GetTouch(0).position;
                    return;
                }
                else
                {
                    if (!eventSent)
                    {
                        if (Right != null && Left != null && Up != null && Down != null)
                        {
                            Vector2 direction = Input.GetTouch(0).position - lastPosition;

                            double radAngle = Math.Atan2(direction.y, direction.x);
                            double angle = radAngle * 180.0 / Math.PI;

                            if (Right != null && Left != null && Up != null && Down != null)
                            {
                                GUILayout.Button("angle="+angle);
                                if (angle > 0 + MARGIN_IN_DEGREE && angle <= 90 - MARGIN_IN_DEGREE)
                                    Up();
                                else if (angle > 90 + MARGIN_IN_DEGREE && angle <= 180 - MARGIN_IN_DEGREE)
                                    Left();
                                else if (angle > 180 + MARGIN_IN_DEGREE && angle <= 270 - MARGIN_IN_DEGREE)
                                    Down();
                                else if (angle > 270 + MARGIN_IN_DEGREE && angle <= 360 - MARGIN_IN_DEGREE)
                                    Right();
                            }

                            eventSent = true;
                        }
                    }
                }
            }
            else
            {
                swiping = false;
                eventSent = false;
            }
        }
    }

}

