using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dime : Coin
{
    protected override void ApplyCollisionObjects()
    {
        collidableController.primaryCollisionObjects.Add(dimeRegion);
        collidableController.secondaryCollisionObjects.AddRange(new List<GameObject> {nickelRegion, pennyRegion, quarterRegion});    
    }
}
