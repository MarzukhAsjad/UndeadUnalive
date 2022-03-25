using System;
using System.Collections;
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
        public UnityEvent<float, float> changeEvent;

        private GameObject _bar;
        private Slider _barSlider;
        private RectTransform _barRect;
        private GameObject _decreaseBar;
        private Slider _decreaseBarSlider;
        private RectTransform _decreaseBarRect;
        private float _trackingMax;
        private float _tracking;
        private float _defaultBarWidth;
        private float _defaultBarPosX;
        
        private Coroutine _decreaseAnimation;


        private void Start()
        {
            _bar = gameObject;

            var wtf = GetComponentsInChildren<MonoBehaviour>();
            
            _decreaseBar = GetComponentsInChildren<MonoBehaviour>().FirstOrDefault(r => r.CompareTag("DecreaseBar"))!.gameObject;
            
            _trackingMax = defaultMax;
            _tracking = defaultValue;
            
            _barSlider = _bar.GetComponent<Slider>();
            _barRect = _barSlider.GetComponent<RectTransform>();
            _decreaseBarSlider = _decreaseBar.GetComponent<Slider>();
            _decreaseBarRect = _decreaseBarSlider.GetComponent<RectTransform>();
            _defaultBarWidth = _barRect.sizeDelta.x;
            _defaultBarPosX = _barRect.anchoredPosition.x;
            
            changeEvent.AddListener(OnChange);
            OnChange(_tracking, _trackingMax);
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

            _barSlider.value = newValue / newMaxValue;

            if (Math.Abs(_tracking - newValue) > float.Epsilon)
            {
                print(_tracking - newValue);
                _tracking = newValue;
                if (_decreaseAnimation != null)
                {
                    StopCoroutine(_decreaseAnimation);
                }

                _decreaseAnimation = StartCoroutine(DoDecreaseAnimation(2, 1));
            }
            else
            {
                _decreaseBarSlider.value = newValue / newMaxValue;
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
                var delta = startValue - endValue;
                var step = GameManager.Instance.TargetFPS * duration;
                for (var i = 0; i <= step; ++i)
                {
                    _decreaseBarSlider.value = startValue - (i / step) * delta;
                    yield return new WaitForEndOfFrame();
                }
            }

            _decreaseAnimation = null;
        }
    }
}
