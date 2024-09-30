using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DisplayManager : MonoBehaviour
{
    [SerializeField] int displayCount;
    List<PlayerInfo> playerInfos = new List<PlayerInfo>();

    [SerializeField] List<CarController> carControllers;

    void Start()
    {
        if (displayCount <= 0 || displayCount > 4)
        {
            Debug.LogError("Number of displays should be at least 1 and no more than 4");
        }

        if (carControllers.Count <= 0)
        {
            Debug.LogError("Number of cars should be at least 1");
        }

        for (int i = 0; i < Camera.allCamerasCount; i++)
        {
            PlayerInfo localPlayerInfo = new PlayerInfo();

            localPlayerInfo.camera = Camera.allCameras[i];
            //localPlayerInfo.gamepad = Gamepad.all[i];

            playerInfos.Add(localPlayerInfo);
        }

        SetCameras();
    }

    void SetCameras()
    {
        List<Rect> settings = new List<Rect>();
        List<int> playerDisplays = new List<int>();

        if (displayCount == 1 && carControllers.Count == 2) // 1 screen 2 players
        {
            settings.Add(new Rect(0, 0, 1, 0.5f));
            playerDisplays.Add(0);
            settings.Add(new Rect(0, 0.5f, 1, 0.5f));
            playerDisplays.Add(0);
        }

        if (displayCount == 1 && carControllers.Count == 4) // 1 screen 4 players
        {
            settings.Add(new Rect(0   , 0   , 0.5f, 0.5f));
            playerDisplays.Add(0);
            settings.Add(new Rect(0   , 0.5f, 0.5f, 0.5f));
            playerDisplays.Add(0);
            settings.Add(new Rect(0.5f, 0   , 0.5f, 0.5f));
            playerDisplays.Add(0);
            settings.Add(new Rect(0.5f, 0.5f, 0.5f, 0.5f));
            playerDisplays.Add(0);
        }

        if (displayCount == 2 && carControllers.Count == 2) // 2 screens 2 players
        {
            settings.Add(new Rect(0, 0, 1, 1));
            playerDisplays.Add(0);
            settings.Add(new Rect(0, 0, 1, 1));
            playerDisplays.Add(1);
        }

        if (displayCount == 2 && carControllers.Count == 4) // 2 screens 4 players
        {
            settings.Add(new Rect(0, 0, 1, 0.5f));
            playerDisplays.Add(0);
            settings.Add(new Rect(0, 0.5f, 1, 0.5f));
            playerDisplays.Add(0);
            settings.Add(new Rect(0, 0, 1, 0.5f));
            playerDisplays.Add(1);
            settings.Add(new Rect(0, 0.5f, 1, 0.5f));
            playerDisplays.Add(1);
        }

        if (displayCount == 4 && carControllers.Count == 4) // 4 screens 4 players
        {
            settings.Add(new Rect(0, 0, 1, 1));
            playerDisplays.Add(0);
            settings.Add(new Rect(0, 0, 1, 1));
            playerDisplays.Add(1);
            settings.Add(new Rect(0, 0, 1, 1));
            playerDisplays.Add(2);
            settings.Add(new Rect(0, 0, 1, 1));
            playerDisplays.Add(3);
        }

        for (int i = 0; i < settings.Count; i++)
        {
            playerInfos[i].camera.rect = settings[i];
            playerInfos[i].camera.targetDisplay = playerDisplays[i];
        }
    }

    public struct PlayerInfo
    {
        public Camera camera;
        public Gamepad gamepad;
    }
}
