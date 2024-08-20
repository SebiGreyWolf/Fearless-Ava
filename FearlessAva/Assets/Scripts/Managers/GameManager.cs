using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<Item> allItems = new List<Item>();
    public List<Quest> allQuests = new List<Quest>();

    public Transform cameraChangePoint;
    public CinemachineConfiner2D confiner;
    public Collider2D CameraBoundsUp;
    public Collider2D CameraBoundsDown;

    public CinemachineVirtualCamera virtualCamera;
    public float OverworldViewSize = 9.25f;
    public float UnderworldViewSize = 12f;

    public PlayerData playerData;
    public float overworldJump = 9;
    public float undergroundJump = 13;

    //Doing GameManaging n' Stuff just like Screen loading
    private Player player;
    private void Start()
    {
        foreach (var item in allItems)
        {
            item.ResetItemState();
        }
        foreach (var quest in allQuests)
        {
            quest.ResetQuestState();
        }
        player = FindObjectOfType<Player>();
        playerData.jumpHeight = overworldJump;
        playerData.RecalculateJumpVariables();
        if (virtualCamera.m_Lens.Orthographic)
        {
            virtualCamera.m_Lens.OrthographicSize = OverworldViewSize;
        }
    }

    private void Update()
    {
        if (player.gameObject.transform.position.y > cameraChangePoint.transform.position.y)
        {
            confiner.m_BoundingShape2D = CameraBoundsUp;
            playerData.jumpHeight = overworldJump;
            playerData.RecalculateJumpVariables();
            if (virtualCamera.m_Lens.Orthographic)
            {
                virtualCamera.m_Lens.OrthographicSize = OverworldViewSize;
            }
        }
        else
        {
            confiner.m_BoundingShape2D = CameraBoundsDown;
            playerData.jumpHeight = undergroundJump;
            playerData.RecalculateJumpVariables();
            if (virtualCamera.m_Lens.Orthographic)
            {
                virtualCamera.m_Lens.OrthographicSize = UnderworldViewSize;
            }
        }
    }
}
