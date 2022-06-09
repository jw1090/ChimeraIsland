using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AsyncLoader : MonoBehaviour
{
    private class RoutineInfo
    {
        public RoutineInfo(IEnumerator routine, int weight, Func<float> progress)
        {
            this.routine = routine;
            this.weight = weight;
            this.progress = progress;
        }

        public readonly IEnumerator routine;
        public readonly int weight;
        public readonly Func<float> progress;
    }

    protected virtual void OnInitComplete() { }
    protected virtual void OnInitUpdate(float percentComplete) { }
    protected virtual void OnInitError(int reasonCode, string reasonDebug) { }

    private Queue<RoutineInfo> _pending = new Queue<RoutineInfo>();
    private bool _completedWithoutError = true;

    protected event Action OnLoadingCompleted;

    protected bool Complete { get; private set; } = false;
    protected float Progress { get; private set; } = 0.0f;

    protected void Enqueue(IEnumerator routine, int weight, Func<float> progress = null)
    {
        _pending.Enqueue(new RoutineInfo(routine, weight, progress));
    }

    protected abstract void Awake();

    private IEnumerator Start()
    {
        if (Complete)
        {
            // at 100% double check if this is 1 or 100;
            Progress = 1.0f;
            _pending.Clear();
            yield break;
        }

        float percentCompleteByFullSections = 0.0f;
        int outOf = 0;

        var running = new Queue<RoutineInfo>(_pending);
        _pending.Clear();

        foreach (var routineInfo in running)
        {
            outOf += routineInfo.weight;
        }

        while (running.Count != 0)
        {
            var routineInfo = running.Dequeue();
            var routine = routineInfo.routine;

            while (routine.MoveNext())
            {
                if (routineInfo.progress != null)
                {
                    var routinePercent = routineInfo.progress() * (float)routineInfo.weight / (float)outOf;
                    Progress = percentCompleteByFullSections + routinePercent;
                    OnInitUpdate(Progress);
                }

                yield return routine.Current;
            }

            percentCompleteByFullSections += (float)routineInfo.weight / (float)outOf;
            Progress = percentCompleteByFullSections;
            OnInitUpdate(Progress);
        }

        if (!_completedWithoutError)
        {
            Debug.LogError("A fatal error occurred while running initialization. Please check your logs and fix the error.");
        }

        Complete = true;

        if (OnLoadingCompleted != null)
        {
            OnLoadingCompleted.Invoke();
        }
        else
        {
            Debug.Log("OnComplete Callback is null, possibly in editor for testing use.");
        }
    }

    // Reset all variables. To be used when the game is resetting.
    protected virtual void ResetVariables()
    {
        OnLoadingCompleted = null;
        Complete = false;
        Progress = 0.0f;
    }

    protected void CallOnComplete_Internal(Action callback)
    {
        if (Complete)
        {
            callback();
        }
        else
        {
            OnLoadingCompleted += callback;
        }
    }

    protected void TriggerError(int reasonCode, string reasonDebug)
    {
        Debug.Log(reasonDebug);

        _pending.Clear();
        _completedWithoutError = false;

        OnInitError(reasonCode, reasonDebug);
    }
}