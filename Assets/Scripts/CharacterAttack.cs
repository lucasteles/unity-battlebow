using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    ArcherAnimation _animations;
    void Start()
    {
        _animations = GetComponent<ArcherAnimation>();
    }

    void Update()
    {
        if (Input.GetButton("Fire2"))
        {
            _animations.Attack();
        }
    }
}
