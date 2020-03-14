using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quarter : Coin
{
    protected override void ApplyCollisionObjects()
    {
        collidableController.primaryCollisionObjects.Add(quarterRegion);
        collidableController.secondaryCollisionObjects.AddRange(new List<GameObject> {dimeRegion, pennyRegion, nickelRegion});    
    }
}
