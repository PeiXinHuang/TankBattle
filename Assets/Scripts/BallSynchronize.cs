using UnityEngine;
using System.Collections;
using Photon.Pun;


public class BallSynchronize : MonoBehaviourPun
{
    Vector3 realPosition = Vector3.zero;
    Quaternion realRotation = Quaternion.identity;
    new Rigidbody rigidbody;
    Vector3 realPosition1 = Vector3.zero;
    Vector3 realVelocity1 = Vector3.zero;
    Quaternion realRotation1 = Quaternion.identity;

    Vector3 realVelocity2 = Vector3.zero;

    private void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
    }

    public void Update()
    {
        if (!photonView.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, realPosition, 0.1f);
            transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, 0.1f);
        }
    }

    public void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            rigidbody.position = Vector3.Lerp(rigidbody.position, realPosition1, 0.1f);
            rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, realVelocity1, 0.1f);
            rigidbody.rotation = Quaternion.Lerp(rigidbody.rotation, realRotation1, 0.1f);

            rigidbody.angularVelocity = Vector3.Lerp(rigidbody.angularVelocity, realVelocity2, 0.1f);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

            stream.SendNext(rigidbody.position);
            stream.SendNext(rigidbody.rotation);
            stream.SendNext(rigidbody.velocity);

            stream.SendNext(rigidbody.angularVelocity);
        }
        else
        {
            realPosition = (Vector3)stream.ReceiveNext();
            realRotation = (Quaternion)stream.ReceiveNext();


            realPosition1 = (Vector3)stream.ReceiveNext();
            realRotation1 = (Quaternion)stream.ReceiveNext();
            realVelocity1 = (Vector3)stream.ReceiveNext();

            realVelocity2 = (Vector3)stream.ReceiveNext();
        }
    }
}