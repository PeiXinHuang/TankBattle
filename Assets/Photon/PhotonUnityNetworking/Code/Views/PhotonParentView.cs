// ----------------------------------------------------------------------------
// <copyright file="PhotonTransformView.cs" company="Exit Games GmbH">
//   PhotonNetwork Framework for Unity - Copyright (C) 2020 Exit Games GmbH
// </copyright>
// <summary>
//   Component to synchronize parent PhotonView and parenting via PUN PhotonView.
// </summary>
// <author>developer@exitgames.com</author>
// ----------------------------------------------------------------------------

using UnityEngine;

namespace Photon.Pun
{
    [AddComponentMenu("Photon Networking/Photon Parent View")]
    public class PhotonParentView : MonoBehaviourPun, IPunObservable
    {
        PhotonView parentView;

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext((int)(parentView == null ? 0 : parentView.ViewID));
            }

            else
            {
                int newView = (int)stream.ReceiveNext();
                if (newView != parentView.ViewID)
                {
                    PhotonView view = PhotonNetwork.GetPhotonView(newView);
                    transform.parent = view ? parentView.transform.parent : null;
                    parentView = view;
                }
            }
        }
    }
}
