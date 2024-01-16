using System;
using System.Collections;
using UnityEngine;

namespace Components
{
    public class RectTransformMover : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;

        [SerializeField] private Vector3 _startPosition;
        [SerializeField] private Vector3 _onScreenPosition;
        [SerializeField] private Vector3 _endPosition;
        [SerializeField] private float _secondsToMove;

        private Coroutine _moveRoutine;
        private Action _onMoveEndedCallback;

        public void MoveIn()
        {
            Move(_startPosition, _onScreenPosition, _secondsToMove);
        }

        public void MoveOut(Action onMoveEndedCallback = null)
        {
            _onMoveEndedCallback = onMoveEndedCallback;

            Move(_onScreenPosition, _endPosition, _secondsToMove);
        }

        private void Move(Vector3 startPosition, Vector3 endPosition, float timeToMove)
        {
            _moveRoutine ??= StartCoroutine(MoveRoutine(startPosition, endPosition, timeToMove));
        }

        private IEnumerator MoveRoutine(Vector3 startPosition, Vector3 endPosition, float timeToMove)
        {
            _rectTransform.anchoredPosition = startPosition;

            float elapsedTime = 0f;

            while (!EndPositionReached(endPosition))
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp(elapsedTime / timeToMove, 0f, 1f);
                t = Mathf.SmoothStep(0f, 1f, t);

                _rectTransform.anchoredPosition = Vector3.Lerp(startPosition, endPosition, t);

                yield return null;
            }

            _moveRoutine = null;

            _onMoveEndedCallback?.Invoke();
            _onMoveEndedCallback = null;
        }

        private bool EndPositionReached(Vector3 endPosition)
        {
            return Vector3.Distance(_rectTransform.anchoredPosition, endPosition) < 0.01f;
        }
    }
}