using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    public static ResolutionManager Instance { get; private set; }
    public Vector2[] resolutions;
    public int startResolution;
    public bool fullScreen;
    private int _resSpot;
    private Vector2 _resolution;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this); 
        else Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        _resSpot = startResolution;
        _resolution = resolutions[_resSpot];
        SetResolution(_resolution, fullScreen);
    }

    public void SetResolution(Vector2 res, bool full)
    {
        fullScreen = full;
        _resolution = res;
        Screen.SetResolution((int)_resolution.x, (int)_resolution.y, fullScreen);
    }

    public Vector2 GetResolution()
    {
        return resolutions[_resSpot];
    }

    public void SetResSpot(int startResolution)
    {
        _resSpot = startResolution;
    }

    public int GetResLength()
    {
        return resolutions.Length;
    }

    public int GetResSpot()
    {
        return _resSpot;
    }
}