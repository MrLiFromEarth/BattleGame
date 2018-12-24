using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : HeroBase {

    Vector3 tranformForward;
    Vector3 tempMoveToPos;
    bool isAttack;
    Vector3 removeMovePos;      //用于移除移动目标点的坐标
    // Use this for initialization
    public void Init(Transform transform)
    {
        this.transform = transform;
        Level = DataManage.Instance.Level;
        animator = transform.GetComponent<Animator>();
        Speed = 3f;
        isAttack = false;
        removeMovePos = new Vector3(1000, 0, 0);
        moveToPos = removeMovePos;
        //IsBettacked = false;


        animator.SetBool("Idle", true);
        animator.SetBool("Move", false);
        animator.SetBool("Attack", false);
    }

    public void Init()
    {
        moveToPos = removeMovePos;
    }
	
	// Update is called once per frame
    public void Update () {

        if (Input.GetMouseButtonDown(0))     //点击屏幕并且不在攻击状态下更新移动目标
        {
            SetMovePos();
            isAttack = true;
            //isMove = true;
        }

        if(animator.GetBool("Attack") /*&&
            (animator.GetCurrentAnimatorStateInfo(0).IsName("dandaocike gongji"))*/)
        {
            HeroAttack();
        }
        else if(Vector3.Distance(transform.position, moveToPos) >= 0.03f &&
            !animator.GetBool("Attack")&&
            moveToPos != removeMovePos)
        {
            HeroMove();
        }

        if (animator.GetBool("Idle"))
        {
            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            transform.GetComponent<Rigidbody>().constraints = ~RigidbodyConstraints.FreezePositionZ | ~RigidbodyConstraints.FreezePositionX | ~RigidbodyConstraints.FreezeRotationY;
        }
    }
    protected override void Idle()
    {
        animator.SetBool("Idle", true);
        animator.SetBool("Attack", false);
    }

    protected override void SetMovePos()
    {
        tempMoveToPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));

        tempMoveToPos = new Vector3(Mathf.Clamp(tempMoveToPos.x, -3.0f, 3.0f),
                    transform.position.y,
                    Mathf.Clamp(tempMoveToPos.z, -6.0f, 6.0f));
        if (Vector3.Distance(tempMoveToPos, transform.position) > 0.35f)
            moveToPos = tempMoveToPos;

        if (!animator.GetBool("Attack"))
        {
            transform.forward = moveToPos - transform.position; ;
        }
        tranformForward = moveToPos - transform.position;
    }

    protected override void HeroAttack()
    {
        animator.SetBool("Move", false);
        animator.SetBool("Attack", true);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("dandaocike gongji") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime>1.0f)
        {
                Idle();
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("dandaocike gongji") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f &&
            Vector3.Distance(transform.position, GameManage.Instance.enemy.transform.position) <= 1.4f &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.2f &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f &&
            /*Vector3.Dot(transform.forward, GameManage.Instance.enemy.transform.position-transform.position)>0f*/
            Vector3.Angle(transform.forward, GameManage.Instance.enemy.transform.position - transform.position) <= 40) //攻击状态，附近，前方，攻击生效
        {
            GameManage.Instance.enemy.HeroBeAttacked();
            Idle();
        }
    }

    private void EnterAttack()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Move", false);
        animator.SetBool("Attack", true);
        isAttack = false;
        HeroAttack();
        AudioSourceManage.Instance.audioSource.Play();
    }

    public override void HeroBeAttacked()
    {

        Idle();
        moveToPos = removeMovePos;
        Death();
    }

    protected override void HeroMove()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Move", true);
        transform.position = Vector3.MoveTowards(transform.position, moveToPos,Time.deltaTime * Speed);
        transform.forward = tranformForward;    



        if (moveToPos != removeMovePos)           //攻击后重置移动目标点 放置被撞击后位置偏差重复攻击
        {
            if (Vector3.Distance(transform.position, moveToPos) < 0.03f && isAttack)
            {
                moveToPos = removeMovePos;
                EnterAttack();
            }
        }
    }

    protected override void Death()
    {
        transform.gameObject.SetActive(false);
        if (Level>1)
        {
            Level--;
        }
        UIManage.Instance.KillFun();
    }
}
