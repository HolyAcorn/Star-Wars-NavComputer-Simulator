using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwNavComp
{
    public class TimerHelper
    {
        public static TimerHelper Create(Action action, float timer)
        {

            GameObject gameObject = new GameObject("FunctionTimer", typeof(MonoBehaviourHook));
            TimerHelper timerHelper = new TimerHelper(action, timer, gameObject);


            gameObject.GetComponent<MonoBehaviourHook>().onUpdate = timerHelper.Update;

            return timerHelper;
        }

        // Dummy class to have access to Monobehaviour functions
        public class MonoBehaviourHook: MonoBehaviour
        {
            public Action onUpdate;
            private void Update()
            {
                if(onUpdate != null) onUpdate();
            }
        }



        private Action action;
        private float timer;
        private GameObject gameObject;
        private bool isDestroyed;
        
        private TimerHelper(Action action, float timer, GameObject gameObject)
        {
            this.action = action;
            this.timer = timer;
            this.gameObject = gameObject;
            isDestroyed = false;
        }

        public void Update()
        {
            if (isDestroyed) return;

            timer -= Time.deltaTime;
            if (timer < 0) action();
        }


        private void DestroySelf()
        {
            isDestroyed = true;
            UnityEngine.Object.Destroy(gameObject);
        }
    }
}
