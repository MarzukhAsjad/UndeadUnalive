using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = System.Object;

namespace Utilities
{
    public class TimeoutList : MonoBehaviour
    {
        private class ListElement
        {
            public object Key;
            public object Data;
            public Action Destructor;
            public float StartTime;
            public float Duration;
            public float EndTime;
        }

        private readonly LinkedList<ListElement> _elements = new();
        private readonly Dictionary<object, ListElement> _elementMap = new();

        private void Update()
        {
            var currentTime = Time.fixedTime;
            while (_elements.Count > 0 && _elements.First().EndTime < currentTime)
            {
                var targetElement = _elements.First();
                targetElement.Destructor?.Invoke();
                _elementMap.Remove(targetElement.Key);
                _elements.RemoveFirst();
            }
        }

        private void InsertValueToList(ListElement element)
        {
            var insertAfter = _elements.LastOrDefault(e => e.EndTime < element.EndTime);
            if (insertAfter is null)
            {
                _elements.AddFirst(element);
            }
            else
            {
                _elements.AddAfter(_elements.Find(insertAfter), element);
            }
        }

        public bool HasElement(object key)
        {
            return _elementMap.ContainsKey(key);
        }
        
        public void RenewElements(object key, object data, float timeout = 1, Action destructor = null)
        {
            var currentTime = Time.fixedTime;

            if (_elementMap.TryGetValue(key, out var element))
            {
                element.Data = data ?? element.Data;
                element.Destructor = destructor ?? element.Destructor;
                element.StartTime = currentTime;
                element.EndTime = currentTime + timeout;
                element.Duration = timeout;

                if (_elements.Count > 1)
                {
                    _elements.Remove(element);
                    InsertValueToList(element);
                }
            }
            else
            {
                var newValue = new ListElement
                {
                    Key = key,
                    Data = data,
                    Destructor = destructor,
                    StartTime = currentTime,
                    EndTime = currentTime + timeout,
                    Duration = timeout
                };

                InsertValueToList(newValue);
                _elementMap.Add(key, newValue);
            }
        }

        public void ForEach(Action<object, object> func)
        {
            foreach (var listElement in _elements) func(listElement.Key, listElement.Data);
        }
    }
}
