using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HeroBase {

    public int Level;
    public bool IsBettacked;
    public float Speed;

    protected Vector3 moveToPos;
    public Transform transform;
    protected Animator animator;

    protected abstract void Idle();
    protected abstract void SetMovePos();
    protected abstract void HeroMove();
    protected abstract void HeroAttack();
    public abstract void HeroBeAttacked();
    protected abstract void Death();


}
