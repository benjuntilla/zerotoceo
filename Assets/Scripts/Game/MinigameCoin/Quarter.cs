using System.Collections.Generic;
using UnityEngine;

public class Quarter : Coin
{
    protected override void ApplyCollisionObjects()
    {
        collidable.primaryCollisionObjects.Add(quarterRegion);
        collidable.secondaryCollisionObjects.AddRange(new List<GameObject> {dimeRegion, pennyRegion, nickelRegion});    
    }
}
