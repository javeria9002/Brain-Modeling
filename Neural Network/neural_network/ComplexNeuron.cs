using System;
using System.Collections.Generic;

public class ComplexNeuron : Neuron
{
    protected List<double> attenuations_;
    protected List<Neuron> complexNeighbours_;

    public ComplexNeuron(double x, double y, double attenuation = 1.0)
        : base(x, y, attenuation)
    {
        attenuations_ = new List<double>();
        complexNeighbours_ = new List<Neuron>();
    }

    public void Attach(double weight, Neuron n)
    {
        attenuations_.Add(weight);
        complexNeighbours_.Add(n);

        if (n is ComplexCumulativeNeuron)
        {
            ((ComplexCumulativeNeuron)n).RegisterIncoming();
        }
    }

    public override void Fire(double receivedSignal)
    {
        signal_ = receivedSignal * attenuation_;
        ComplexPropagate();
    }

    protected void ComplexPropagate()
    {
        for (int i = 0; i < complexNeighbours_.Count; i++)
        {
            double weightedSignal = signal_ * attenuations_[i];
            complexNeighbours_[i].Fire(weightedSignal);
        }
    }
}