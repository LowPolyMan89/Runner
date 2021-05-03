using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCameraController : MonoBehaviour
{
    public Animator CameraAnimator;

    private void Start()
    {
        CameraAnimator.SetInteger("CameraPosition", 1);
    }

    public void ChangeCameraPosition(int positionIndx)
    {
        CameraAnimator.SetInteger("CameraPosition", positionIndx);
    }
}
