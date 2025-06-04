using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace HUIFramework.Common
{
    public class TimerSystem : SingletonMonoBase<TimerSystem>
    {
        private Dictionary<long, TimerNode> timers = null;
        private List<TimerNode> remove_timers = null;
        private List<TimerNode> new_add_timers = null;
        private long auto_inc_id = 1;
        
        public override async UniTask InitAsync()
        {
            this.timers = new Dictionary<long, TimerNode>();
            this.auto_inc_id = 1;

            this.remove_timers = new List<TimerNode>();
            this.new_add_timers = new List<TimerNode>();
            await UniTask.DelayFrame(1);
        }

        public long Schedule(Action func, int repeat, float duration, float delay = 0.0f)
        {
            TimerNode timer = new TimerNode();
            timer.callback = func;
            timer.repeat = repeat;
            timer.duration = duration;
            timer.delay = delay;
            timer.passedTime = timer.duration;
            timer.isRemoved = false;

            timer.timerId = this.auto_inc_id;
            this.auto_inc_id++;

            this.new_add_timers.Add(timer);
            return timer.timerId;
        }

        public long ScheduleOnce(float dutration, Action func)
        {
            return Schedule(func, 1, dutration);
        }

        public void Unschedule(long timerId)
        {
            if (!this.timers.ContainsKey(timerId))
            {
                return;
            }

            TimerNode timer = this.timers[timerId];
            timer.isRemoved = true;
        }

        private void Update()
        {
            float dt = Time.deltaTime;

            for (int i = 0; i < this.new_add_timers.Count; i++)
            {
                this.timers.Add(this.new_add_timers[i].timerId, this.new_add_timers[i]);
            }

            this.new_add_timers.Clear();
            foreach (TimerNode timer in this.timers.Values)
            {
                if (timer.isRemoved)
                {
                    this.remove_timers.Add(timer);
                    continue;
                }

                timer.passedTime += dt;
                if (timer.passedTime >= (timer.delay + timer.duration))
                {
                    timer.callback();
                    timer.repeat--;
                    timer.passedTime -= (timer.delay + timer.duration);
                    timer.delay = 0;

                    if (timer.repeat == 0)
                    {
                        timer.isRemoved = true;
                        this.remove_timers.Add(timer);
                    }
                }
            }

            for (int i = 0; i < this.remove_timers.Count; i++)
            {
                this.timers.Remove(this.remove_timers[i].timerId);
            }

            this.remove_timers.Clear();
        }
    }

    public class TimerNode
    {
        public Action callback;
        public float duration;
        public float delay;
        public int repeat;
        public float passedTime;
        public bool isRemoved;
        public long timerId;
    }
}