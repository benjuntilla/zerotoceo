using System.Collections.Generic;
using UnityEngine;

public class Nickel : Coin
{
    protected override void ApplyCollisionObjects()
    {
        collidable.primaryCollisionObjects.Add(nickelRegion);
        collidable.secondaryCollisionObjects.AddRange(new List<GameObject> {dimeRegion, pennyRegion, quarterRegion});    
    }
}
