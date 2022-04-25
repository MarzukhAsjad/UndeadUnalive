using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Interface.Controller
{
    public class BarController : MonoBehaviour
    {
        public float defaultMax;
        public float defaultValue;
        public float animationTime = 2;
        public UnityEvent<float, float> changeEvent;

        private GameObject _bar;
        private Slider _barSlider;
        private RectTransform _barRect;
        private GameObject _decreaseBar;
        private Slider _decreaseBarSlider;
        private RectTransform _decreaseBarRect;
        private float _trackingMax;
        private float _trackingPercentage;
        private float _defaultBarWidth;
        private float _defaultBarPosX;

        private Coroutine _decreaseAnimation;


        private void Start()
        {
            _bar = gameObject;

            var wtf = GetComponentsInChildren<MonoBehaviour>();

            _decreaseBar = GetComponentsInChildren<MonoBehaviour>().FirstOrDefault(r => r.CompareTag("DecreaseBar"))!
                .gameObject;

            _trackingMax = defaultMax;
            _trackingPercentage = defaultValue / defaultMax;

            _barSlider = _bar.GetComponent<Slider>();
            _barRect = _barSlider.GetComponent<RectTransform>();
            _decreaseBarSlider = _decreaseBar.GetComponent<Slider>();
            _decreaseBarRect = _decreaseBarSlider.GetComponent<RectTransform>();
            _defaultBarWidth = _barRect.sizeDelta.x;
            _defaultBarPosX = _barRect.anchoredPosition.x;

            changeEvent.AddListener(OnChange);
            OnChange(_trackingPercentage * defaultMax, _trackingMax);
        }

        private void OnChange(float newValue, float newMaxValue)
        {
            var max = newMaxValue;

            if (Math.Abs(_trackingMax - max) > float.Epsilon)
            {
                _trackingMax = max;

                var newWidth = _defaultBarWidth * (max / defaultMax);

                var temAnchor = _barRect.anchoredPosition;
                temAnchor.x = _defaultBarPosX + (newWidth - _defaultBarWidth) / 2;
                _barRect.anchoredPosition = temAnchor;
                _decreaseBarRect.anchoredPosition = temAnchor;


                var temSizeDelta = _barRect.sizeDelta;
                temSizeDelta.x = newWidth;
                _barRect.sizeDelta = temSizeDelta;
                _decreaseBarRect.sizeDelta = temSizeDelta;
            }

            if (_decreaseBarSlider.value < _barSlider.value)
            {
                _trackingPercentage = _decreaseBarSlider.value = _barSlider.value;
            }

            _barSlider.value = newValue / newMaxValue;

            var valueDiff = _trackingPercentage - _barSlider.value;
            if (Math.Abs(valueDiff) > float.Epsilon)
            {
                if (valueDiff > 0)
                {
                    _trackingPercentage = _barSlider.value;
                    if (_decreaseAnimation != null)
                    {
                        StopCoroutine(_decreaseAnimation);
                    }

                    _decreaseAnimation = StartCoroutine(DoDecreaseAnimation(animationTime, 1));
                }
            }
        }

        private IEnumerator DoDecreaseAnimation(float duration, float delay = 0)
        {
            yield return new WaitForSeconds(delay);

            var startValue = _decreaseBarSlider.value;
            var endValue = _barSlider.value;

            if (endValue > startValue) _decreaseBarSlider.value = endValue;
            else
            {
                float timer = 0;

                var delta = startValue - endValue;
                while (timer < duration)
                {
                    _decreaseBarSlider.value = startValue - (timer / duration) * delta;

                    if (_barSlider.value > _decreaseBarSlider.value)
                    {
                        _trackingPercentage = _barSlider.value;
                        _decreaseAnimation = null;
                        yield break;
                    }

                    yield return new WaitForEndOfFrame();
                    timer += Time.deltaTime;
                }

                _decreaseBarSlider.value = endValue;
            }

            _trackingPercentage = _barSlider.value;
            _decreaseAnimation = null;
        }
    }
}