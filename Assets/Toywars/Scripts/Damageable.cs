using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Damageable {
    void takeDamage(float dmg, bool isPlayerShot);
}
