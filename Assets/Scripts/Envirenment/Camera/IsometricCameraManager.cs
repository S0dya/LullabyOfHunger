using System.Linq;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricCameraManager : SingletonMonobehaviour<IsometricCameraManager>
{
    [Header("settings")]
    public float TimeBetweenFindingNearestEnemy = 1;

    //local 
    CinemachineVirtualCamera _vCam;

    Transform _playerTrasnf;

    Dictionary<Transform, Enemy> _enemiesDict = new Dictionary<Transform, Enemy>();

    //bools
    bool _isFollowingEnemy;

    //threshold 
    Transform _curCameraPosTransf;

    KeyValuePair<Transform, Enemy> _curNearestKvp;

    //cors
    Coroutine _changeToNearestEnemyCor;

    protected override void Awake()
    {
        base.Awake();

        _vCam = GetComponent<CinemachineVirtualCamera>();
    }

    void Start()
    {
        _vCam.LookAt = GameObject.FindGameObjectWithTag("CameraLookAtTarget").transform;
        _playerTrasnf = LevelManager.Instance.GetPlayerTransform();
    }

    void MoveCamera(Transform followTransf) => _vCam.Follow = followTransf;
    void MoveCamera() => MoveCamera(_curCameraPosTransf);


    //cors
    IEnumerator ChangeToNearestEnemyCor()
    {
        while (true)
        {
            _curNearestKvp = _enemiesDict.OrderBy(kvp => Vector3.Distance(kvp.Key.position, _playerTrasnf.position)).First();

            MoveCamera(_curNearestKvp.Key);

            yield return new WaitForSeconds(TimeBetweenFindingNearestEnemy);
        }
    }


    //outside methods
    public void NewPositionForCameraFollow(Transform newCameraPosTransf)
    {
        _curCameraPosTransf = newCameraPosTransf;
        if (!_isFollowingEnemy) MoveCamera();
    }
    public void NewPositionForCameraFollow(Transform newCameraPosTransf, Enemy enemy)
    {
        if (!_enemiesDict.ContainsKey(newCameraPosTransf))
        {
            _enemiesDict.Add(newCameraPosTransf, enemy);

            if (_enemiesDict.Count == 1)
            {
                _isFollowingEnemy = true;
                MoveCamera(newCameraPosTransf);
            }
            else if (_enemiesDict.Count == 2)
            {
                _isFollowingEnemy = true;
                _changeToNearestEnemyCor = StartCoroutine(ChangeToNearestEnemyCor());
            }
        }
    }

    public void RemoveEnemyFollow(Transform enemyTransf)
    {
        _enemiesDict.Remove(enemyTransf);

        if (_enemiesDict.Count == 0)
        {
            _isFollowingEnemy = false;
            if (_changeToNearestEnemyCor != null) StopCoroutine(_changeToNearestEnemyCor);

            MoveCamera();
        }
    }
}
