using System;
using System.Linq;
using UnityEngine;

public abstract class Brain : MonoBehaviour
{
    private ISensor[] sensors;
    private IActuator[] actuators;


    private bool hudUpdateRequired = true;


    public void Connect(ISensor[] sensors, IActuator[] actuators)
    {
        this.sensors = sensors;
        this.actuators = actuators;
        this.Build(this.sensors.Sum(sensor => sensor.GetReceptors().Length), this.actuators.Sum(actuator => actuator.InputCount));
    }

    public void Act()
    {
        int receptorsCopied = 0;
        foreach (var sensor in sensors)
        {
            sensor.OnRefresh();
            float[] receptors = sensor.GetReceptors();
            Array.Copy(receptors, 0, GetInputActivations(), receptorsCopied, receptors.Length);
            receptorsCopied += receptors.Length;
            sensor.OnReset();
        }

        Compute();

        int neuronsActioned = 0;
        foreach (var actuator in actuators)
            actuator.Act(new ArraySegment<float>(GetOutputActivations(), neuronsActioned, actuator.InputCount));

        hudUpdateRequired = true;
    }

    public virtual bool UpdateHUD(GameObject brainHudGameObject)
    {
        bool updateRequired = hudUpdateRequired;
        this.hudUpdateRequired = false;
        return updateRequired;
    }

    public abstract void Build(int nInputs, int nOutputs);

    public abstract float[] GetInputActivations();

    public abstract float[] GetOutputActivations();

    public abstract void Compute();
}

