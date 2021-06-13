using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    const int MIN_JOINTS = 4;
    const int MAX_JOINTS = 8;


    [SerializeField] HingeJoint2D hookJoint;
    [SerializeField] private GameObject linkPrefab;

    Vector2 anchorPos;
    GameObject firstConnectedGameObject;
    GameObject secondConnectedGameObject;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void GenerateRope(GameObject connectedGameObject1, GameObject connectedGameObject2)
    {
        int nbJoints = Random.Range(MIN_JOINTS, MAX_JOINTS);

        firstConnectedGameObject = connectedGameObject1;
        secondConnectedGameObject = connectedGameObject2;
        hookJoint.connectedBody = connectedGameObject1.GetComponent<Rigidbody2D>();
        Rigidbody2D previousRb = hookJoint.attachedRigidbody;
        anchorPos = new Vector2(0, -linkPrefab.GetComponent<SpriteRenderer>().bounds.size.y);

        for (float i=0; i < nbJoints; i++)
        {
            RopeSegment ropeSegment = Instantiate(linkPrefab, transform.position, Quaternion.identity).GetComponent<RopeSegment>();
            ropeSegment.transform.parent = transform;
            ropeSegment.SetUp(this, previousRb, anchorPos);
            previousRb = ropeSegment.Rb;
        }
        SetConnectedGameObject(connectedGameObject2, previousRb);
    }

    void SetConnectedGameObject(GameObject connectedGameObject, Rigidbody2D endRb)
    {
        HingeJoint2D joint = connectedGameObject.AddComponent<HingeJoint2D>();
        joint.connectedBody = endRb;
        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = Vector3.zero;
        joint.connectedAnchor = anchorPos;
    }

    public void DestroySelf()
    {
        if (firstConnectedGameObject && secondConnectedGameObject)
        {
            firstConnectedGameObject.GetComponent<Ship>().DisconnectFromShip(secondConnectedGameObject.GetComponent<Ship>());
        }
        if (secondConnectedGameObject && firstConnectedGameObject)
        {
            secondConnectedGameObject.GetComponent<Ship>().DisconnectFromShip(firstConnectedGameObject.GetComponent<Ship>());
        }
        Destroy(this.gameObject);
    }

}
