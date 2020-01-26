using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penny : Coin
{
    protected override void ApplyCollisionObjects()
    {
        collidableController.primaryCollisionObjects.Add(pennyRegion);
        collidableController.secondaryCollisionObjects.AddRange(new List<GameObject> {dimeRegion, nickelRegion, quarterRegion});    
    }
}
