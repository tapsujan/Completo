using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public void Damage(Vector3 force, int damage);
}
