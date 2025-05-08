using System;
using System.Collections;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerNetwork : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("HUD")]
    [SerializeField] private CameraPOV cameraPOV;
    private void Start()
    {
        // Manager
        if (!isLocalPlayer && cameraPOV != null)
            cameraPOV.gameObject.SetActive(false);
        NetworkIdentity netId = GetComponentInParent<NetworkIdentity>();
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        if (!isLocalPlayer) return;
        CmdSendPosition(transform.position);
    }

    [Command]
    void CmdSendPosition(Vector3 pos)
    {
        RpcUpdatePosition(pos);
    }
    [ClientRpc]
    void RpcUpdatePosition(Vector3 pos)
    {
        if (isLocalPlayer) return;
        transform.position = pos;
    }
    
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        if (cameraPOV.gameObject != null)
        {
            cameraPOV.gameObject.SetActive(true);
            Debug.Log("[PlayerController] Câmera ativada.");
        }
    }

}
