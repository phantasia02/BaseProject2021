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
        }

        [SerializeField]
        private EProjectileType _ProjectileType =  EProjectileType.eBullet;


        public EProjectileType ProjectileType => _ProjectileType;
    }
}
