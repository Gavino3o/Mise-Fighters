using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AutoTimer : MonoBehaviour
{
    public double _timerPeriod = 0.00;
    public double _currentTime = 0.00;

    private bool _isActive;
    public bool _isRepeating = false;
    public bool _playOnStart = false;

    public UnityEvent _timeout;  

    public AutoTimer(double timerPeriod, bool isRepeating, bool _playOnStart)
    {
        _timerPeriod = timerPeriod;
        _isRepeating = isRepeating;
        _timeout = new UnityEvent();

        if (_playOnStart)
        {
            _isActive = true;
        }

        enabled = false;
    }


    void Update()
    {
        if (!_isActive)
            return;

        if (_timerPeriod <= 0.00)
        {
            StopTimer();
            return;
        }

        _currentTime += Time.deltaTime;

        if (_currentTime > _timerPeriod)
        {

            if (_isRepeating)
            {
                _currentTime = 0.00;
            }
            else
            {
                StopTimer();
            }

            _timeout?.Invoke();

        }

    }

    public void StartTimer()
    {
        enabled = true;
        _isActive = true;
        _currentTime = 0.00;
    }

    public void PauseTimer()
    {
        _isActive = false;
    }

    public void StopTimer()
    {
        enabled = false;
        _currentTime = 0;
        _isActive = false;
    }

    public void ContinueTimer()
    {
        _isActive = true;
    }
}
