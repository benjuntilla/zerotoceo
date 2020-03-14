using System.Collections.Generic;
using UnityEngine;

public class Dime : Coin
{
    protected override void ApplyCollisionObjects()
    {
        collidable.primaryCollisionObjects.Add(dimeRegion);
        collidable.secondaryCollisionObjects.AddRange(new List<GameObject> {nickelRegion, pennyRegion, quarterRegion});    
    }
}
