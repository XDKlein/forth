using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace forth
{
    [Serializable]
    public class InputControls
    {
        private KeyCode moveLeft = KeyCode.A;
        private KeyCode moveRight = KeyCode.W;
        private KeyCode moveUp = KeyCode.W;
        private KeyCode moveDown = KeyCode.S;

        private KeyCode zoomIn = KeyCode.Z;
        private KeyCode zoomOut = KeyCode.X;

        private KeyCode leftClick = KeyCode.Mouse0;
        private KeyCode rightClick = KeyCode.Mouse1;

        public KeyCode MoveLeft
        {
            get
            {
                return moveLeft;
            }

            set
            {
                moveLeft = value;
            }
        }

        public KeyCode MoveRight
        {
            get
            {
                return moveRight;
            }

            set
            {
                moveRight = value;
            }
        }

        public KeyCode MoveUp
        {
            get
            {
                return moveUp;
            }

            set
            {
                moveUp = value;
            }
        }

        public KeyCode MoveDown
        {
            get
            {
                return moveDown;
            }

            set
            {
                moveDown = value;
            }
        }

        public KeyCode ZoomIn
        {
            get
            {
                return zoomIn;
            }

            set
            {
                zoomIn = value;
            }
        }

        public KeyCode ZoomOut
        {
            get
            {
                return zoomOut;
            }

            set
            {
                zoomOut = value;
            }
        }

        public KeyCode LeftClick
        {
            get
            {
                return leftClick;
            }

            set
            {
                leftClick = value;
            }
        }

        public KeyCode RightClick
        {
            get
            {
                return rightClick;
            }

            set
            {
                rightClick = value;
            }
        }
    }
}