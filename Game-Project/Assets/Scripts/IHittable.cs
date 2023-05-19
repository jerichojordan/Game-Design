using UnityEngine;

public interface IHittable
{
    public void RecieveHit(RaycastHit2D hit, float damage);
}

