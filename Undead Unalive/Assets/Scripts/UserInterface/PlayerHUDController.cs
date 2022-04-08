using System;
using Characters.Entity;
using Interface.Controller;
using Interface.Player;
using UnityEngine;
using UnityEngine.Events;
using Utilities;
using Random = UnityEngine.Random;

namespace UserInterface
{
    using EnemyType = GameObject;
    public class PlayerHUDController : MonoBehaviourSingleton<PlayerHUDController>
    {
        /*
         *
         *  Prefab
         * 
         */
        [SerializeField] private GameObject gameOverScreen;

        private Camera activeCamera;
        private GameObject _playerHUDObject;
        private GameObject _healthBar;

        private CharacterEntity _playerEntity;

        private readonly UnityEvent<float, float> _healthChangeProxy = new();

        public GameObject enemyIndicatorPrefab;

        /*
         *
         * Enemy indicator
         * 
         */
        private TimeoutList _enemyIndicatorList;

        private void OnEnable()
        {
            _playerHUDObject = GameObject.FindWithTag("PlayerHUD")?.gameObject;
            Debug.Assert(_playerHUDObject != null);

            _playerEntity = GameObject.FindWithTag("Player")?.GetComponent<CharacterEntity>();
            Debug.Assert(_playerEntity != null);

            _healthBar = _playerHUDObject.transform.Find("HealthBar").gameObject;

            _playerEntity.onHealthChanged.AddListener(() => _healthChangeProxy.Invoke(_playerEntity.GetHealth(), _playerEntity.GetDefaultMaxHealth()));
            var barController = _healthBar.AddComponent<BarController>();
            barController.changeEvent = _healthChangeProxy;
            barController.defaultValue = _playerEntity.GetHealth();
            barController.defaultMax = _playerEntity.GetDefaultMaxHealth();
        }

        private void Start()
        {
            activeCamera = Camera.main;
            _enemyIndicatorList = gameObject.AddComponent<TimeoutList>();
        }

        private float timer = 0;

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer > 0.5f)
            {
                var newObj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                var ranPos = Random.insideUnitCircle * 10;
                newObj.transform.position = new Vector3(ranPos.x, 1, ranPos.y);
                newObj.AddComponent<Rigidbody>();
                newObj.AddComponent<PlayerInteractableExplosion>();

                Destroy(newObj, 2.5f);
                EnemyNotifyPlayer(newObj);

                timer = 0;
            }

            _enemyIndicatorList.ForEach((k, d) =>
            {
                FindScreenEdgeLocationForWorldLocation(((GameObject)k).transform.position, 0.9f, out var outScreenPosition, out var outRotationAngleDegrees, out var bIsOnScreen);

                ((RectTransform)d).gameObject.SetActive(!bIsOnScreen);
                
                ((RectTransform)d).anchoredPosition = outScreenPosition;
                ((RectTransform)d).rotation = Quaternion.Euler(0, 0, outRotationAngleDegrees - 90);
            });
        }

        private void FindScreenEdgeLocationForWorldLocation(Vector3 inLocation, float edgePercent, out Vector2 outScreenPosition, out float outRotationAngleDegrees, out bool bIsOnScreen)
        {
            // inspired from https://forums.unrealengine.com/t/easy-offscreen-indicator-blueprint-node/21342?filter=summary

            bIsOnScreen = false;
            outRotationAngleDegrees = 0f;
            Vector2 screenPosition;

            var pixelRect = activeCamera.pixelRect;
            var viewportSize = pixelRect.size;
            var viewportCenter = pixelRect.center;

            var forward = activeCamera.transform.forward;
            var offset = (inLocation - _playerEntity.transform.position).normalized;

            var dotProduct = Vector3.Dot(forward, offset);
            var bLocationIsBehindCamera = dotProduct < 0;

            if (bLocationIsBehindCamera)
            {
                // For behind the camera situation, we cheat a little to put the
                // marker at the bottom of the screen so that it moves smoothly
                // as you turn around. Could stand some refinement, but results
                // are decent enough for most purposes.

                var position = _playerEntity.transform.position;

                var diffVector = inLocation - position;
                var Inverted = diffVector * -1f;
                var newInLocation = position + Inverted;

                screenPosition = activeCamera.WorldToScreenPoint(newInLocation);
                screenPosition.x = viewportSize.x - screenPosition.x;
                screenPosition.y = viewportSize.y - screenPosition.y;
            }
            else
            {
                screenPosition = activeCamera.WorldToScreenPoint(inLocation);
            }

            // Check to see if it's on screen. If it is, ProjectWorldLocationToScreen is all we need, return it.
            if (screenPosition.x >= 0f && screenPosition.x <= viewportSize.x
                                       && screenPosition.y >= 0f && screenPosition.y <= viewportSize.y && !bLocationIsBehindCamera)
            {
                outScreenPosition = screenPosition;
                bIsOnScreen = true;
                return;
            }

            screenPosition -= viewportCenter;

            var angleRadians = Mathf.Atan2(screenPosition.y, screenPosition.x);
            angleRadians -= Mathf.Deg2Rad * 90;

            outRotationAngleDegrees = Mathf.Rad2Deg * angleRadians + 180f;

            var cos = Mathf.Cos(angleRadians);
            var sin = -Mathf.Sin(angleRadians);

            var m = cos / sin;

            var screenBounds = viewportCenter * edgePercent;

            screenPosition = cos > 0 ? new Vector2(screenBounds.y / m, screenBounds.y) : new Vector2(-screenBounds.y / m, -screenBounds.y);

            if (screenPosition.x > screenBounds.x)
            {
                screenPosition = new Vector2(screenBounds.x, screenBounds.x * m);
            }
            else if (screenPosition.x < -screenBounds.x)
            {
                screenPosition = new Vector2(-screenBounds.x, -screenBounds.x * m);
            }

            screenPosition += viewportCenter;
            outScreenPosition = screenPosition;
        }

        public void EnemyNotifyPlayer(EnemyType enemy)
        {
            RectTransform imageRect = null;
            Action destructor = null;

            if (!_enemyIndicatorList.HasElement(enemy))
            {
                imageRect = Instantiate(enemyIndicatorPrefab).GetComponent<RectTransform>();
                imageRect.transform.SetParent(_playerHUDObject.transform);
                destructor = () => { Destroy(imageRect.gameObject); };
            }

            _enemyIndicatorList.RenewElements(enemy, imageRect, 2, destructor);
        }

        public void DoGameOverScreen()
        {
            Instantiate(gameOverScreen);
        }
    }
}
