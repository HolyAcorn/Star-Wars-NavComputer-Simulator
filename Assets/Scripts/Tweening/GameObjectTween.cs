using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwNavComp
{
    public enum GameObjectAnimationTypes
    {
        Move,
        Scale,
        ScaleX,
        ScaleY,
        Rotate,
        Fade
    }

    public class GameObjectTween : MonoBehaviour
    {
        public GameObject objectToAnimate;
        SpriteRenderer spriteRenderer;


        public GameObjectAnimationTypes animationType;
        public LeanTweenType easeType;
        public float duration;
        public float delay;

        public bool loop;
        public bool pingPong;

        public bool startPositionOffset;
        public Vector3 from;
        public Vector3 to;

        private LTDescr _tweenObject;

        public bool showOnEnable;
        public bool workOnDisable;

        public void OnEnable()
        {
            if (showOnEnable) Show();
        }

        public void Show()
        {
            HandleTween();
        }

        public void HandleTween()
        {
            if (objectToAnimate == null) objectToAnimate = gameObject;
            spriteRenderer = objectToAnimate.GetComponent<SpriteRenderer>();

            switch (animationType)
            {
                case GameObjectAnimationTypes.Move:
                    MoveAbsolute();
                    break;
                case GameObjectAnimationTypes.Scale:
                    Scale();
                    break;
                case GameObjectAnimationTypes.ScaleX:
                    break;
                case GameObjectAnimationTypes.ScaleY:
                    break;
                case GameObjectAnimationTypes.Rotate:
                    break;
                case GameObjectAnimationTypes.Fade:
                    Fade();
                    break;
            }

            _tweenObject.setDelay(delay);
            _tweenObject.setEase(easeType);

            if (loop) _tweenObject.loopCount = int.MaxValue;
            if (pingPong) _tweenObject.setLoopPingPong();

        }

        public void Fade()
        {
            if (startPositionOffset) spriteRenderer.color = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b, from.x);

            _tweenObject = LeanTween.alphaVertex(objectToAnimate, to.x, duration);
        }

        public void MoveAbsolute()
        {
            objectToAnimate.transform.position = from;

            _tweenObject = LeanTween.move(objectToAnimate, to, duration);
        }

        public void Scale()
        {
            if (startPositionOffset) objectToAnimate.transform.localScale = from;
            _tweenObject = LeanTween.scale(objectToAnimate, to, duration);
        }

        public void Rotate()
        {

        }
    }
}
