using System;

public class CumulativeNeuron : Neuron
{
    private int totalIncomingConnections_;
    private int counter_;
    private double lastFiredSignal_; 
    public CumulativeNeuron(double x, double y, double attenuation = 1.0)
        : base(x, y, attenuation)
    {
        totalIncomingConnections_ = 0;
        counter_ = 0;
        lastFiredSignal_ = 0.0;
    }

    public void RegisterIncoming()
    {
        totalIncomingConnections_++;
    }

    protected override void Accumulate(double receivedSignal)
    {
        signal_ = signal_ + receivedSignal;
        counter_++;
    }

    public override void Fire(double receivedSignal)
    {
        Accumulate(receivedSignal);

        if (counter_ == totalIncomingConnections_)
        {
            signal_ = 1.0 / (1.0 + Math.Exp(-signal_));

            lastFiredSignal_ = signal_;

            Propagate();

            counter_ = 0;
            signal_ = 0.0;
        }
    }

    public double LastSignal()
    {
        return lastFiredSignal_;
    }

    public override void Display()
    {
        Console.WriteLine("CumulativeNeuron at position " + position_ +
                          " with attenuation " + attenuation_);
        Console.WriteLine("  Last fired signal (after sigmoid): " + lastFiredSignal_.ToString("F4"));
    }

    public override string ToString()
    {
        return lastFiredSignal_.ToString("F4") + "\n";
    }
}