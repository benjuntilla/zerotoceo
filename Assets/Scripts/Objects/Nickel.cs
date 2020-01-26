using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nickel : Coin
{
    protected override void ApplyCollisionObjects()
    {
        collidableController.primaryCollisionObjects.Add(nickelRegion);
        collidableController.secondaryCollisionObjects.AddRange(new List<GameObject> {dimeRegion, pennyRegion, quarterRegion});    
    }
}
