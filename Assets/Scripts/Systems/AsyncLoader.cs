using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

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
	private static event Action _onComplete;
	private static bool _complete = false;
	private static float _progress = 0.0f;

	public static bool Complete { get { return _complete; } }
	public static float Progress { get { return _progress; } }

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
			_progress = 1.0f;
			_pending.Clear();
			yield break;
		}

		float percentCompleteByFullSections = 0.0f;
		int outOf = 0;

		var running = new Queue<RoutineInfo>(_pending);
		_pending.Clear();

		foreach (var routineInfo in running)
			outOf += routineInfo.weight;

		while (running.Count != 0)
		{
			var routineInfo = running.Dequeue();
			var routine = routineInfo.routine;

			while (routine.MoveNext())
			{
				if (routineInfo.progress != null)
				{
					var routinePercent = routineInfo.progress() * (float)routineInfo.weight / (float)outOf;
					_progress = percentCompleteByFullSections + routinePercent;
					OnInitUpdate(Progress);
				}

				yield return routine.Current;
			}

			percentCompleteByFullSections += (float)routineInfo.weight / (float)outOf;
			_progress = percentCompleteByFullSections;
			OnInitUpdate(Progress);
		}

		if (!_completedWithoutError)
			Debug.Log("A fatal error occurred while running initialization. Please check your logs and fix the error.");

		_complete = true;

		if (_onComplete != null)
		{
			_onComplete.Invoke();
		}
		else
		{
			Debug.Log("OnComplete Callback is null, possibly in editor for testing use.");
		}
	}

	/// <summary>
	/// Reset all static variables.  To be used when the game is resetting.
	/// </summary>
	public static void ResetStaticVariables()
	{
		_onComplete = null;
		_complete = false;
		_progress = 0.0f;
	}

	public static void CallOnComplete(Action callback)
	{
		if (Complete)
		{
			callback();
		}
		else
		{
			_onComplete += callback;
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