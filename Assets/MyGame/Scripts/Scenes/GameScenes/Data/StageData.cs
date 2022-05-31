using UnityEngine;
using LanKuDot.UnityToolBox;
using System.Collections.Generic;

namespace MYgame.Scripts.Scenes.GameScenes.Data
{

    public enum EJumpStatePlayerType
    {
        eNormal = 0,
        eInsert = 1,
    }

    [CreateAssetMenu(
        menuName = "Data/Stage Data",
        fileName = "StageData")]
    public class StageData : ScriptableObject
    {
        [SerializeField]
        private bool _WinMoveWinPos = false;

        [SerializeField]
        private Vector3 _Gravity = new Vector3(0.0f, -9.81f, 0.0f);

        [SerializeField]
        private Vector3 _AddForce = Vector3.zero;

        [SerializeField]
        private float _PredictionTime = 2.0f;

        [SerializeField]
        private float _PredictionBallSize = 1.0f;

        [SerializeField]
        private EJumpStatePlayerType _JumpStatePlayerType =  EJumpStatePlayerType.eNormal;

        [SerializeField]
        private float _RotationAngle = 0.0f;


        public bool WinMoveWinPos => _WinMoveWinPos;
        public Vector3 Gravity => _Gravity;
        public Vector3 AddForce => _AddForce;
        public float PredictionTime => _PredictionTime;
        public float PredictionBallSize => _PredictionBallSize;
        public EJumpStatePlayerType JumpStatePlayerType => _JumpStatePlayerType;
        public float RotationAngle => _RotationAngle;

    }
}
