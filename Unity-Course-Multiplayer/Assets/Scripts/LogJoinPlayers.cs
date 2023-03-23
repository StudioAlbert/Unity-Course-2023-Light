using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class LogJoinPlayers : MonoBehaviour
{

    [SerializeField] private Collider2D _confiner2D;
    
    [SerializeField] 
    [Tooltip("Layers by player orders")]
    private List<LayerMask> _layers;

    public void OnPlayerJoin(PlayerInput input)
    {
        Debug.Log("Player join : " + input.playerIndex + " : ");

        // Confine to that place
        if(_confiner2D != null)
            input.GetComponentInChildren<CinemachineConfiner2D>().m_BoundingShape2D = _confiner2D;

        Camera camera = input.GetComponentInChildren<Camera>();
        camera.cullingMask &=  ~(1 << _layers[input.playerIndex]);
        camera.gameObject.layer = _layers[input.playerIndex].value;
        
        CinemachineVirtualCamera vCam = input.GetComponentInChildren<CinemachineVirtualCamera>();
        vCam.gameObject.layer = _layers[input.playerIndex].value;

    }
    
    
}
