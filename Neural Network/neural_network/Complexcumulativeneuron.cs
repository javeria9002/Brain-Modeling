using System;
using System.Collections.Generic;

public class ComplexCumulativeNeuron : ComplexNeuron
{
    private int totalIncomingConnections_;
    private int counter_;
    private double accumulatedSignal_;
    private double lastFiredSignal_; 

    public ComplexCumulativeNeuron(double x, double y, double attenuation = 1.0)
        : base(x, y, attenuation)
    {
        totalIncomingConnections_ = 0;
        counter_ = 0;
        accumulatedSignal_ = 0.0;
        lastFiredSignal_ = 0.0;
    }

    public void RegisterIncoming()
    {
        totalIncomingConnections_++;
    }

    public override void Fire(double receivedSignal)
    {
        accumulatedSignal_ += receivedSignal;
        counter_++;

        if (totalIncomingConnections_ > 0 && counter_ == totalIncomingConnections_)
        {
            signal_ = 1.0 / (1.0 + Math.Exp(-accumulatedSignal_));

            lastFiredSignal_ = signal_;

            for (int i = 0; i < complexNeighbours_.Count; i++)
            {
                double weightedSignal = signal_ * attenuations_[i];
                complexNeighbours_[i].Fire(weightedSignal);
            }

            counter_ = 0;
            accumulatedSignal_ = 0.0;
        }
    }

    public double LastSignal()
    {
        return lastFiredSignal_;
    }

    public override string ToString()
    {
        return lastFiredSignal_.ToString("F4") + "\n";
    }
}