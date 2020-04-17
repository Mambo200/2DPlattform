using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MainEnemyAir : MainEnemy
{
    public override GroundType Type => GroundType.AIR;

    public override void Update()
    {
        base.Update();
    }
}
