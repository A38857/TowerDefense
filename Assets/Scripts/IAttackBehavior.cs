using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public interface IAttackBehavior
{
    void Attack(Transform target);
    void Upgrade(int Level);
}
