using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Player;
using UnityEngine;

public class BootsCutscene : MonoBehaviour
{
    public CinemachineVirtualCamera bootsCam;
    public PlayerMechanics playerMechanics;
    public void RunCoroutine()
    {
        StartCoroutine(ChangeCameras());
    }
    private IEnumerator ChangeCameras()
    {
        playerMechanics.DisableMovement();
        bootsCam.Priority = 20;
        yield return new WaitForSeconds(3);
        bootsCam.Priority = 5;
        yield return new WaitForSeconds(1);
        playerMechanics.EnableMovement();
    }
}
