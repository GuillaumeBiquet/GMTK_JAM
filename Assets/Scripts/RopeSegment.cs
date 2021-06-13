using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSegment : MonoBehaviour
{
    [SerializeField] float maxHP = 3f;
    [SerializeField] Color maxHpColor;
    [SerializeField] Color minHpColor;
    public float hp;

    SpriteRenderer spriteRenderer;
    HingeJoint2D joint;
    Rigidbody2D rb;

    RopeSegment aboveSegment;
    RopeSegment belowSegment;
    Rope rope;

    public RopeSegment AboveSegment { get { return aboveSegment;  } }
    public RopeSegment BelowSegment { get { return belowSegment;  } set { belowSegment = value; } }
    public HingeJoint2D Joint { get { return joint; } }
    public Rigidbody2D Rb { get { return rb; } }

    public void SetUp(Rope _rope, Rigidbody2D aboveRb, Vector2 anchorPos)
    {
        rope = _rope;
        joint = GetComponent<HingeJoint2D>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hp = maxHP;

        joint.connectedBody = aboveRb;
        joint.connectedAnchor = anchorPos;
        if (aboveRb.GetComponent<RopeSegment>() != null)
        {
            aboveSegment = aboveRb.GetComponent<RopeSegment>();
            aboveSegment.BelowSegment = this;
        }
    }

    public void SetUp(Rope _rope)
    {
        rope = _rope;
        joint = GetComponent<HingeJoint2D>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hp = maxHP;
    }

    private void Update()
    {

    }

    public void TakeDamage(float damage)
    {
        if (rope == null) {
            return;
        }

        hp -= damage;

        if(hp <= 0) 
        {
            GameManager.Instance.crystals += 25;
            rope.DestroySelf();
        }
        else
        {
            spriteRenderer.color = Color.Lerp(minHpColor, maxHpColor, Mathf.PingPong(hp / maxHP, 1));
        }

    }


    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }


}
