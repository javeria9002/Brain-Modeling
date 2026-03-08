using System;
using System.Collections.Generic;

public class Neuron
{
    protected Point position_;
    protected double signal_;
    protected double attenuation_;
    protected List<Neuron> neighbours;

    public Neuron(double x, double y, double attenuation = 1.0)
    {
        position_ = new Point(x, y);
        signal_ = 0.0;
        attenuation_ = attenuation;
        neighbours = new List<Neuron>();
    }

    public Point Position() { return position_; }
    public double Signal() { return signal_; }

    public static Neuron operator +(Neuron left, Neuron right)
    {
        left.neighbours.Add(right);
        return left;
    }

    public void AddNeighbour(Neuron n)
    {
        neighbours.Add(n);
    }

    public virtual void Fire(double receivedSignal)
    {
        Accumulate(receivedSignal);
        Propagate();
    }

    protected virtual void Accumulate(double receivedSignal)
    {
        signal_ = receivedSignal * attenuation_;
    }

    protected virtual void Propagate()
    {
        foreach (Neuron n in neighbours)
        {
            n.Fire(signal_);
        }
    }

    public virtual void Display()
    {
        Console.WriteLine("The neuron at position " + position_ +
                          " with an attenuation factor of " + attenuation_ +
                          " is connected to following (" + neighbours.Count + ") neuron(s):");
        if (neighbours.Count == 0)
        {
            Console.WriteLine("  not connected to any neuron");
        }
        else
        {
            foreach (Neuron n in neighbours)
            {
                Console.WriteLine("  - Neuron at position " + n.position_);
            }
        }
    }

    public override string ToString()
    {
        return signal_.ToString("F4") + "\n";
    }
}