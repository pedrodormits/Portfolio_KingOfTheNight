using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region Variables
    [Header("Position")]
    [SerializeField] private List<Transform> _players;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _smoothTime;

    private Vector3 _velocity;
    #endregion

    void FixedUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        if (_players.Count == 0) return;

        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition,
            ref _velocity, _smoothTime);
    }

    Vector3 GetCenterPoint()
    {
        if (_players.Count == 1) return _players[0].position;

        Bounds bounds = new Bounds(_players[0].position, Vector3.zero);
        for (int i = 1; i < _players.Count; i++)
        {
            bounds.Encapsulate(_players[i].position);
        }

        return bounds.center;
    }
}