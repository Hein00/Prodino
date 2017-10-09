﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : BaseBullet
{
    private bool? isRight = null;
    private bool? isCenter = null;

    protected override void Start()
    {
        base.Start();
        if (GameManager.instance.currentGameMode == GameMode.TOPDOWN)
        {
            AssignDirection();
        }
    }

    protected override void Update()
    {
        base.Update();
        DestroyGameobject(Register.instance.propertiesPlayer.bulletDestructionMargin);
    }

    protected void AssignDirection()
    {
        if (transform.position.x > Register.instance.player.transform.position.x)
        {
            isRight = true;
            isCenter = false;
        }
        else if (transform.position.x < Register.instance.player.transform.position.x)
        {
            isRight = false;
            isCenter = false;
        }
        else if (transform.position.x == Register.instance.player.transform.position.x)
        {
            isRight = false;
            isCenter = true;
        }
    }

    protected override void Move()
    {
        if (GameManager.instance.currentGameMode == GameMode.SIDESCROLL)
        {
            if ((isRight == null))
            {
                transform.Translate(direction * Register.instance.propertiesPlayer.bulletpeed * Time.deltaTime, Space.World);
            }
            else
            {
                if (isRight.Value && !isCenter.Value)
                {
                    transform.Translate(Vector3.right * Register.instance.propertiesPlayer.bulletpeed * Time.deltaTime, Space.World);
                }
                else if (!isRight.Value && !isCenter.Value)
                {
                    transform.Translate(Vector3.left * Register.instance.propertiesPlayer.bulletpeed * Time.deltaTime, Space.World);
                }
                else if (!isRight.Value && isCenter.Value)
                {
                    transform.Translate(Vector3.forward * Register.instance.propertiesPlayer.bulletpeed * Time.deltaTime, Space.World);
                }
            }
        }
        else if (GameManager.instance.currentGameMode == GameMode.TOPDOWN)
        {
            transform.Translate(direction * Register.instance.propertiesPlayer.bulletpeed * Time.deltaTime, Space.World);
        }
    }
}
