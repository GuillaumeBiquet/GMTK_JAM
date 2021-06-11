using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] Rigidbody2D hookRb;
    [SerializeField] private GameObject linkPrefab;
    [SerializeField] private GameObject connectedGameObject;
    [SerializeField] int nbLinks;
    Vector2 anchorPos;

    // Start is called before the first frame update
    void Start()
    {
        GenerateRope();
    }

    void GenerateRope()
    {
        Rigidbody2D previousRb = hookRb;
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
        SetConnectedGameObject(previousRb);
    }

    void SetConnectedGameObject(Rigidbody2D endRb)
    {
        HingeJoint2D joint = connectedGameObject.AddComponent<HingeJoint2D>();
        joint.connectedBody = endRb;
        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = Vector3.zero;
        joint.connectedAnchor = anchorPos;
    }
}
