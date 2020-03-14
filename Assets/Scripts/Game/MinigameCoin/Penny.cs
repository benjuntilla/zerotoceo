using System.Collections.Generic;
using UnityEngine;

public class Penny : Coin
{
    protected override void ApplyCollisionObjects()
    {
        collidable.primaryCollisionObjects.Add(pennyRegion);
        collidable.secondaryCollisionObjects.AddRange(new List<GameObject> {dimeRegion, nickelRegion, quarterRegion});    
    }
}
