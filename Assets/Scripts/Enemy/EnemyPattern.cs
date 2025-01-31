﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using PunchGear.Entity;

namespace PunchGear.Enemy
{
    public class EnemyPattern : MonoBehaviour, IPlaceableEntity
    {
        public GameObject bullet;
        public GameObject spawnPosition;

        public int height = 4; // 위아래 위치, 임시 세팅

        private Player _player;
        private Coroutine _attackCoroutine;

        // 여기서 속도 수동 조작 가능
        public float slow = 1.2f;
        public float normal = 1.0f;
        public float fast = 0.8f;
        public float duration = 1f;
        public float term = 1.5f;
        public float smoothTime = 0.2f;

        [field: SerializeField]
        public bool IsMoving { get; private set; }

        [field: SerializeField]
        public EntityPosition Position { get; private set; }

        private NobilityAnimationController _animationController;

        private readonly List<IAttackPattern> _attackPatterns = new List<IAttackPattern>();

        private void Awake()
        {
            _animationController = GetComponent<NobilityAnimationController>();
            _attackPatterns.Add(new AttackPattern1(this, _animationController));
            _attackPatterns.Add(new AttackPattern2(this, _animationController));
            _attackPatterns.Add(new AttackPattern3(this, _animationController));
            _attackPatterns.Add(new AttackPattern4(this, _animationController));
            _attackPatterns.Add(new AttackPattern5(this, _animationController));
            _attackPatterns.Add(new AttackPattern6(this, _animationController));

            _player = FindFirstObjectByType<Player>();
            if (_player == null)
            {
                throw new NullReferenceException("Cannot find any player component");
            }
            Debug.Log("Player detected");
        }

        private void Start()
        {
            Position = EntityPosition.Bottom;
            EntityPositionHandler.Instance.SetPosition(this, EntityPosition.Bottom);
            ProjectileLauncher.Instance.BulletLauncherOrigin = gameObject;
            _attackCoroutine = StartCoroutine(Pattern());
        }

        private void OnDisable()
        {
            if (_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine);
            }
        }

        public IEnumerator MoveOppositePosition() // 위치 반전 기계 에디션
        {
            IsMoving = true; // 이동 시작
            EntityPosition targetPosition = Position switch
            {
                EntityPosition.Bottom => EntityPosition.Top,
                EntityPosition.Top => EntityPosition.Bottom,
                _ => throw new InvalidOperationException("Undefined value")
            };
            yield return EntityPositionHandler.Instance.SmoothDampPosition(transform, targetPosition, duration, smoothTime);
            Position = targetPosition;
            IsMoving = false;
        }

        [Obsolete]
        public IEnumerator Launch(float speed)
        {
            GameObject bulletObject = Instantiate(bullet, spawnPosition.transform.position, Quaternion.identity);
            Projectile projectile = bulletObject.GetComponent<Projectile>();
            projectile.Position = Position;
            projectile.EnemyOrigin = gameObject;
            projectile.Player = _player;
            yield return new WaitForSeconds(speed); // 시간 지연
        }

        IEnumerator Pattern()
        {
            while (true)
            {
                int randomInt = UnityEngine.Random.Range(0, _attackPatterns.Count);
                IAttackPattern attackPattern = _attackPatterns[randomInt];
                yield return attackPattern.GetPatternCoroutine();
                yield return new WaitForSeconds(term);
            }
        }
    }
}
