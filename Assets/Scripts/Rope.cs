using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] HingeJoint2D hookJoint;
    [SerializeField] private GameObject linkPrefab;

    // TODO: give it an actual value
    [SerializeField] int nbLinks;
    Vector2 anchorPos;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void GenerateRope(GameObject connectedGameObject1, GameObject connectedGameObject2)
    {
        hookJoint.connectedBody = connectedGameObject1.GetComponent<Rigidbody2D>();
        Rigidbody2D previousRb = hookJoint.attachedRigidbody;
        anchorPos = new Vector2(0, -linkPrefab.GetComponent<SpriteRenderer>().bounds.size.y);

        for (float i=0; i < nbLinks; i++)
        {
            GameObject link = Instantiate(linkPrefab, transform.position, Quaternion.identity);
            link.transform.parent = transform;
            HingeJoint2D joint = link.GetComponent<HingeJoint2D>();
            joint.connectedBody = previousRb;
            joint.connectedAnchor = anchorPos;
            previousRb = link.GetComponent<Rigidbody2D>();
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
}
