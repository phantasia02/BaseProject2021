using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MYgame.Scripts.Scenes.GameScenes.Data
{
    [CreateAssetMenu(
        menuName = "Data/Projectile Data",
        fileName = "ProjectileData")]
    public class CProjectileData : ScriptableObject
    {
        public enum EProjectileType
        {
            eBullet     = 0,
            eRubberBand = 1,
            eBulletDart = 2,
        }

        [SerializeField]
        private EProjectileType _ProjectileType =  EProjectileType.eBullet;

        [SerializeField]
        private float _TouchDis = 0.3f;

        [SerializeField]
        private float _RotateTime = 0.01f;


        public EProjectileType ProjectileType => _ProjectileType;
        public float TouchDis => _TouchDis;
        public float RotateTime => _RotateTime;
    }
}
