using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MainEnemyGround : MainEnemy
{
    public override GroundType Type => GroundType.GROUND;
}
