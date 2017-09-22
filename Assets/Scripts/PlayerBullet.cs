﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : BaseBullet
{
    private bool? isRight = null;
    private bool? isCenter = null;

    private void Start()
    {
        if (GameManager.instance.currentGameMode == GameMode.TOPDOWN)
        {
            AssignDirection();
        }
    }

    protected void AssignDirection()
    {
        if (transform.position.x > GameManager.instance.player.transform.position.x)
        {
            isRight = true;
            isCenter = false;
        }
        else if (transform.position.x < GameManager.instance.player.transform.position.x)
        {
            isRight = false;
            isCenter = false;
        }
        else if (transform.position.x == GameManager.instance.player.transform.position.x)
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
                transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
            }
            else
            {
                if (isRight.Value && !isCenter.Value)
                {
                    transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
                }
                else if (!isRight.Value && !isCenter.Value)
                {
                    transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
                }
                else if (!isRight.Value && isCenter.Value)
                {
                    transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.World);
                }
            }
        }
        else if (GameManager.instance.currentGameMode == GameMode.TOPDOWN)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
        }
    }
}
