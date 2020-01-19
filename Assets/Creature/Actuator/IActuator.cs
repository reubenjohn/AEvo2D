using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActuator
{
    int InputCount { get; }
    void Act(ArraySegment<float> activations);
}
