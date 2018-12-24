using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : HeroBase {

    private float time;
    private float latetime;
    private Vector3 moveToPos2;
    private Vector3 tranformForward;    //用于防止目标旋转
    private Vector3 tranformForward2;
    private Vector3 tempMoveToPos;
    private Vector3 removeMovePos;      //用于移除移动目标点的坐标
    private bool isAttack;
    public void Init(Transform transform)
    {
        this.transform = transform;
        Level = DataManage.Instance.Level;
        Speed = (1.5 + Level * 0.195f) > 5.4 ? 5.4f : (1.5f + Level * 0.195f);
        animator = transform.GetComponent<Animator>();
        removeMovePos = new Vector3(1000, 0, 0);
        moveToPos = removeMovePos;
        moveToPos2 = removeMovePos;
        isAttack = false;
        //IsBettacked = false;
    }

    public void Init()
    {
        Level = GameManage.Instance.player.Level;
        Speed = (1.5 + Level * 0.195f) > 5.4 ? 5.4f : (1.5f + Level * 0.195f);
        moveToPos = removeMovePos;
        moveToPos2 = removeMovePos;

        animator.SetBool("Idle", true);
        animator.SetBool("Move", false);
        animator.SetBool("Attack", false);
    }

    // Update is called once per frame
    public void Update()
    {
        time += Time.deltaTime;
        if (time >= Random.Range(0.5f, 1.0f))
        {
            SetMovePos();
            isAttack = true;
            time = 0;
        }

        if (animator.GetBool("Attack"))    //攻击条件Attack为false并且进入准备攻击状态
        {
            HeroAttack();
        }
        else if (Vector3.Distance(transform.position, moveToPos) >= 0.03f && !animator.GetBool("Attack") && moveToPos != removeMovePos)
        {
            HeroMove();
        }
    }

    private void EnterAttack()
    {
        animator.SetBool("Move", false);
        animator.SetBool("Attack", true);
        isAttack = false;
        AudioSourceManage.Instance.audioSource.Play();
    }

    protected override void HeroAttack()
    {
        animator.SetBool("Move", false);
        animator.SetBool("Attack", true);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("dandaocike gongji") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
        {
            Idle();
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("dandaocike gongji") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f &&
            Vector3.Distance(transform.position, GameManage.Instance.player.transform.position) <= 1.4f &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.2f &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f &&
            Vector3.Angle(transform.forward, GameManage.Instance.player.transform.position - transform.position) <= 40) //攻击状态，附近，前方，攻击生效
        {
            GameManage.Instance.player.HeroBeAttacked();
            Idle();
        }
    }

    public override void HeroBeAttacked()
    {
        //IsBettacked = true;
        Idle();                             //被攻击站立
        moveToPos = removeMovePos;      //移动位置移除
        Death();
       // IsBettacked = false;
    }

    protected override void HeroMove()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Move", true);

        if (moveToPos2 != removeMovePos && Vector3.Distance(transform.position,moveToPos2)>0.35f)    //中间点移动
        {
            transform.position = Vector3.MoveTowards(transform.position, moveToPos2, Time.deltaTime * Speed);
            transform.forward = tranformForward2;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, moveToPos, Time.deltaTime * Speed);
            transform.forward = tranformForward;
            moveToPos2 = removeMovePos;
        }
 

        if (moveToPos != removeMovePos)
        {
            if (Vector3.Distance(transform.position, moveToPos) < 0.03f && isAttack)
            {
                moveToPos = removeMovePos;                      //目标点移除
                tranformForward = GameManage.Instance.player.transform.position - transform.position;
                if (Level<20)                                   //方向设置   
                {
                    tranformForward = Quaternion.AngleAxis(Random.Range(-80/Level/4, 80/Level/4), Vector3.up) * tranformForward;
                    transform.forward = tranformForward;                     
                }
                else
                {
                    transform.forward = tranformForward;        //方向设置   
                }
                EnterAttack();                                  //进入攻击状态
            }
        }
    }

    protected override void Idle()
    {
        animator.SetBool("Idle", true);
        animator.SetBool("Attack", false);
    }

    protected override void SetMovePos()
    {
        float x = GameManage.Instance.player.transform.position.x;                                       //地点传入
        float z = GameManage.Instance.player.transform.position.z;

        tempMoveToPos = new Vector3(
            Mathf.Clamp(x + Random.Range(-2.0f, 2.0f), -1.8f, 1.8f),
            transform.position.y,
            Mathf.Clamp(z + Random.Range(-2.0f, 2.0f), -4.5f, 4.5f));

        if (Vector3.Distance(tempMoveToPos, transform.position) > 0.35f)                                 //终止点设置
            moveToPos = tempMoveToPos;

        if (Level>10)
        {
            if (Vector3.Distance(GameManage.Instance.player.transform.position, transform.position) > 3f)       //中间点设置
            {
                float distance = Vector3.Distance(GameManage.Instance.player.transform.position, transform.position) / 3.0f;
                moveToPos2 = (transform.position + GameManage.Instance.player.transform.position) / 2.0f;
                moveToPos2 = new Vector3(
                Mathf.Clamp(moveToPos2.x + Random.Range(-1.4f * distance, 1.4f * distance), -1.8f, 1.8f),
                transform.position.y,
                Mathf.Clamp(moveToPos2.z + Random.Range(-1.4f * distance, 1.4f * distance), -4.5f, 4.5f));
                tranformForward2 = moveToPos2 - transform.position;
            }
            else
            {
                moveToPos2 = removeMovePos;
            }
        }

        if (!animator.GetBool("Attack"))                                                                 //防止攻击的时候设置目标点转向
        {
            transform.forward = moveToPos - transform.position; ;
        }
        tranformForward = moveToPos - transform.position;
    }

    protected override void Death()
    {
        GameManage.Instance.player.Level++;
        transform.gameObject.SetActive(false);
        UIManage.Instance.KillFun();

    }
}
