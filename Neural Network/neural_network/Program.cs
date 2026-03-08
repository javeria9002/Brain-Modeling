using System;

class Program
{
    static void PrintHeader(string title)
    {
        Console.WriteLine();
        Console.WriteLine("╔══════════════════════════════════════════════╗");
        Console.WriteLine("║  " + title.PadRight(44) + "║");
        Console.WriteLine("╚══════════════════════════════════════════════╝");
        Console.WriteLine();
    }

    static void PrintSubHeader(string title)
    {
        Console.WriteLine("   " + title);
        Console.WriteLine();
    }

    static void PrintDivider()
    {
        Console.WriteLine("  ------------------------------------------------");
    }

    static void Main()
    {
        Console.WriteLine();
        Console.WriteLine("  -----------------------------------------------------");
        Console.WriteLine("  ---                 BRAIN MODELING                ---");
        Console.WriteLine("  ---         Neural Network Simulation in C#       ---");
        Console.WriteLine("  -----------------------------------------------------");

        PrintHeader("TEST 1: Basic Neuron");
        TestBasicNeuron();

        PrintHeader("TEST 2: Cumulative Neuron");
        TestCumulativeNeuron();

        PrintHeader("TEST 3: Complex Neuron");
        TestComplexNeuron();

        PrintHeader("TEST 4: Complex Cumulative Neuron");
        TestComplexCumulativeNeuron();

        PrintHeader("LOGIC GATE: OR  (a | b)");
        TestOrGate();

        PrintHeader("LOGIC GATES: AND, NOT, NAND");
        TestAndGate();
        TestNotGate();
        TestNandGate();

        PrintHeader("LOGIC GATE: XNOR  ~(a ^ b)");
        TestXnorGate();

        Console.WriteLine();
        Console.WriteLine("  --------------------------------------------------");
        Console.WriteLine("  ---           ALL TESTS COMPLETED              ---");
        Console.WriteLine("  --------------------------------------------------");
        Console.WriteLine();
    }

    static void TestBasicNeuron()
    {
        Neuron neuron1 = new Neuron(0, 1, 0.5);
        Neuron neuron2 = new Neuron(1, 0, 1.0);
        Neuron neuron3 = new Neuron(1, 1, 2.0);

        neuron1.AddNeighbour(neuron2);
        neuron1.AddNeighbour(neuron3);
        neuron2.AddNeighbour(neuron3);

        PrintSubHeader("Neuron connections");
        neuron1.Display();

        PrintSubHeader("Firing neuron1 with signal = 10");
        Console.WriteLine("  Expected:  neuron1 = 10 x 0.5 = 5.0");
        Console.WriteLine("             neuron2 = 5.0 x 1.0 = 5.0");
        Console.WriteLine("             neuron3 = 5.0 x 2.0 = 10.0  (last write wins)");
        Console.WriteLine();

        neuron1.Fire(10);

        Console.WriteLine("  Results:");
        Console.WriteLine("  neuron1 signal  ->  " + neuron1.Signal().ToString("F4"));
        Console.WriteLine("  neuron2 signal  ->  " + neuron2.Signal().ToString("F4"));
        Console.WriteLine("  neuron3 signal  ->  " + neuron3.Signal().ToString("F4"));
        Console.WriteLine();
    }

    static void TestCumulativeNeuron()
    {
        CumulativeNeuron neuron4 = new CumulativeNeuron(0, 0, 0.75);
        neuron4.RegisterIncoming();
        neuron4.RegisterIncoming();

        PrintSubHeader("CumulativeNeuron at (0,0)  attenuation=0.75");
        Console.WriteLine("  Firing twice with signal = 10 each time");
        Console.WriteLine("  Expected:  10 + 10 = 20  ->  sigmoid(20) ~= 1.0");
        Console.WriteLine();

        neuron4.Fire(10);
        neuron4.Fire(10);

        Console.WriteLine("  Results:");
        Console.WriteLine("  neuron4 signal  ->  " + neuron4.LastSignal().ToString("F4"));
        Console.WriteLine();
    }

    static void TestComplexNeuron()
    {
        ComplexNeuron senderNeuron = new ComplexNeuron(0, 0);
        ComplexNeuron receiverNeuronA = new ComplexNeuron(1, 0);
        ComplexNeuron receiverNeuronB = new ComplexNeuron(2, 0);

        senderNeuron.Attach(0.5, receiverNeuronA);  
        senderNeuron.Attach(2.0, receiverNeuronB);  

        senderNeuron.Fire(10);

        PrintSubHeader("senderNeuron fires 10  ->  different weight per connection");
        Console.WriteLine("  receiverNeuronA  weight=0.5   expected= 5.0    got= " + receiverNeuronA.Signal().ToString("F4"));
        Console.WriteLine("  receiverNeuronB  weight=2.0   expected=20.0    got= " + receiverNeuronB.Signal().ToString("F4"));
        Console.WriteLine();

        Neuron basicSenderNeuron = new Neuron(0, 0, 0.5);
        Neuron basicReceiverNeuronA = new Neuron(1, 0);
        Neuron basicReceiverNeuronB = new Neuron(2, 0);

        basicSenderNeuron.AddNeighbour(basicReceiverNeuronA);
        basicSenderNeuron.AddNeighbour(basicReceiverNeuronB);
        basicSenderNeuron.Fire(10);

        PrintSubHeader("Basic Neuron fires 10  ->  SAME weight for all (limitation)");
        Console.WriteLine("  basicReceiverNeuronA  weight=0.5   got= " + basicReceiverNeuronA.Signal().ToString("F4") + "   <- same");
        Console.WriteLine("  basicReceiverNeuronB  weight=0.5   got= " + basicReceiverNeuronB.Signal().ToString("F4") + "   <- same");
        Console.WriteLine();
    }

    static void TestComplexCumulativeNeuron()
    {
        ComplexNeuron sourceNeuron1 = new ComplexNeuron(0, 0);
        ComplexNeuron sourceNeuron2 = new ComplexNeuron(1, 0);
        ComplexCumulativeNeuron collectorNeuron = new ComplexCumulativeNeuron(2, 0);
        ComplexNeuron outputNeuron1 = new ComplexNeuron(3, 0);
        ComplexNeuron outputNeuron2 = new ComplexNeuron(4, 0);

        sourceNeuron1.Attach(1.0, collectorNeuron);  
        sourceNeuron2.Attach(1.0, collectorNeuron);  
        collectorNeuron.Attach(0.5, outputNeuron1);  
        collectorNeuron.Attach(2.0, outputNeuron2);  

        sourceNeuron1.Fire(5);
        sourceNeuron2.Fire(5);

        PrintSubHeader("sourceNeuron1 and sourceNeuron2 fire 5.0  ->  collectorNeuron accumulates then propagates");
        Console.WriteLine("  Accumulated:  5 + 5 = 10  ->  sigmoid(10) ~= 1.0");
        Console.WriteLine();
        Console.WriteLine("  collectorNeuron signal         expected=~1.0   got= " + collectorNeuron.LastSignal().ToString("F4"));
        Console.WriteLine("  outputNeuron1   weight=0.5     expected=~0.5   got= " + outputNeuron1.Signal().ToString("F4"));
        Console.WriteLine("  outputNeuron2   weight=2.0     expected=~2.0   got= " + outputNeuron2.Signal().ToString("F4"));
        Console.WriteLine();

        ComplexNeuron firstSourceNeuron = new ComplexNeuron(0, 1);
        ComplexNeuron secondSourceNeuron = new ComplexNeuron(1, 1);
        ComplexCumulativeNeuron waitingCollector = new ComplexCumulativeNeuron(2, 1);

        firstSourceNeuron.Attach(1.0, waitingCollector);
        secondSourceNeuron.Attach(1.0, waitingCollector);

        PrintSubHeader("Proving collectorNeuron WAITS for all signals before firing");
        firstSourceNeuron.Fire(5);
        Console.WriteLine("  After 1st signal only  ->  waitingCollector= " + waitingCollector.LastSignal().ToString("F4") + "  (waiting for 2nd signal...)");
        secondSourceNeuron.Fire(5);
        Console.WriteLine("  After 2nd signal       ->  waitingCollector= " + waitingCollector.LastSignal().ToString("F4") + "  (all signals received, fired!)");
        Console.WriteLine();
    }

    static void TestOrGate()
    {
        PrintSubHeader("Truth Table  (bias=-10, inputA=+20, inputB=+20)");
        PrintDivider();
        Console.WriteLine("  a    b    sum        sigmoid    result");
        PrintDivider();

        int[,] inputs = { { 0, 0 }, { 0, 1 }, { 1, 0 }, { 1, 1 } };
        for (int index = 0; index < 4; index++)
        {
            int inputA = inputs[index, 0], inputB = inputs[index, 1];

            Neuron biasNeuron = new Neuron(2, 0, -10);
            Neuron neuronA = new Neuron(1, 0, 20.0);
            Neuron neuronB = new Neuron(0, 0, 20.0);
            CumulativeNeuron outputNeuron = new CumulativeNeuron(1, 3, 1);

            outputNeuron.RegisterIncoming();
            outputNeuron.RegisterIncoming();
            outputNeuron.RegisterIncoming();

            biasNeuron.AddNeighbour(outputNeuron);
            neuronA.AddNeighbour(outputNeuron);
            neuronB.AddNeighbour(outputNeuron);

            biasNeuron.Fire(1);
            neuronA.Fire(inputA);
            neuronB.Fire(inputB);

            double sum = -10 + 20 * inputA + 20 * inputB;
            double sigmoid = outputNeuron.LastSignal();
            Console.WriteLine("  " + inputA + "    " + inputB
                + "    " + sum.ToString("F1").PadLeft(5)
                + "      " + sigmoid.ToString("F4")
                + "     ->  " + Threshold(sigmoid));
        }
        PrintDivider();
        Console.WriteLine();
    }
    static void TestAndGate()
    {
        PrintSubHeader("AND Gate  (bias=-30, inputA=+20, inputB=+20)");
        PrintDivider();
        Console.WriteLine("  a    b    sum        sigmoid    result");
        PrintDivider();

        int[,] inputs = { { 0, 0 }, { 0, 1 }, { 1, 0 }, { 1, 1 } };
        for (int index = 0; index < 4; index++)
        {
            int inputA = inputs[index, 0], inputB = inputs[index, 1];

            Neuron biasNeuron = new Neuron(2, 0, -30);
            Neuron neuronA = new Neuron(1, 0, 20.0);
            Neuron neuronB = new Neuron(0, 0, 20.0);
            CumulativeNeuron outputNeuron = new CumulativeNeuron(1, 3, 1);

            outputNeuron.RegisterIncoming();
            outputNeuron.RegisterIncoming();
            outputNeuron.RegisterIncoming();

            biasNeuron.AddNeighbour(outputNeuron);
            neuronA.AddNeighbour(outputNeuron);
            neuronB.AddNeighbour(outputNeuron);

            biasNeuron.Fire(1);
            neuronA.Fire(inputA);
            neuronB.Fire(inputB);

            double sum = -30 + 20 * inputA + 20 * inputB;
            double sigmoid = outputNeuron.LastSignal();
            Console.WriteLine("  " + inputA + "    " + inputB
                + "    " + sum.ToString("F1").PadLeft(5)
                + "      " + sigmoid.ToString("F4")
                + "     ->  " + Threshold(sigmoid));
        }
        PrintDivider();
        Console.WriteLine();
    }
    static void TestNotGate()
    {
        PrintSubHeader("NOT Gate  (bias=+10, inputA=-20)");
        PrintDivider();
        Console.WriteLine("  a    sum        sigmoid    result");
        PrintDivider();

        for (int inputA = 0; inputA <= 1; inputA++)
        {
            Neuron biasNeuron = new Neuron(1, 0, 10);
            Neuron neuronA = new Neuron(0, 0, -20.0);
            CumulativeNeuron outputNeuron = new CumulativeNeuron(1, 2, 1);

            outputNeuron.RegisterIncoming();
            outputNeuron.RegisterIncoming();

            biasNeuron.AddNeighbour(outputNeuron);
            neuronA.AddNeighbour(outputNeuron);

            biasNeuron.Fire(1);
            neuronA.Fire(inputA);

            double sum = 10 + (-20) * inputA;
            double sigmoid = outputNeuron.LastSignal();
            Console.WriteLine("  " + inputA
                + "    " + sum.ToString("F1").PadLeft(5)
                + "      " + sigmoid.ToString("F4")
                + "     ->  " + Threshold(sigmoid));
        }
        PrintDivider();
        Console.WriteLine();
    }
    static void TestNandGate()
    {
        PrintSubHeader("NAND Gate  (bias=+30, inputA=-20, inputB=-20)");
        PrintDivider();
        Console.WriteLine("  a    b    sum        sigmoid    result");
        PrintDivider();

        int[,] inputs = { { 0, 0 }, { 0, 1 }, { 1, 0 }, { 1, 1 } };
        for (int index = 0; index < 4; index++)
        {
            int inputA = inputs[index, 0], inputB = inputs[index, 1];

            Neuron biasNeuron = new Neuron(2, 0, 30);
            Neuron neuronA = new Neuron(1, 0, -20.0);
            Neuron neuronB = new Neuron(0, 0, -20.0);
            CumulativeNeuron outputNeuron = new CumulativeNeuron(1, 3, 1);

            outputNeuron.RegisterIncoming();
            outputNeuron.RegisterIncoming();
            outputNeuron.RegisterIncoming();

            biasNeuron.AddNeighbour(outputNeuron);
            neuronA.AddNeighbour(outputNeuron);
            neuronB.AddNeighbour(outputNeuron);

            biasNeuron.Fire(1);
            neuronA.Fire(inputA);
            neuronB.Fire(inputB);

            double sum = 30 + (-20) * inputA + (-20) * inputB;
            double sigmoid = outputNeuron.LastSignal();
            Console.WriteLine("  " + inputA + "    " + inputB
                + "    " + sum.ToString("F1").PadLeft(5)
                + "      " + sigmoid.ToString("F4")
                + "     ->  " + Threshold(sigmoid));
        }
        PrintDivider();
        Console.WriteLine();
    }

    static void TestXnorGate()
    {
        PrintSubHeader("Truth Table  (4 ComplexNeurons + 3 ComplexCumulativeNeurons)");
        PrintDivider();
        Console.WriteLine("  a    b    andDetector  norDetector  finalOutput  result");
        PrintDivider();

        int[,] inputs = { { 0, 0 }, { 0, 1 }, { 1, 0 }, { 1, 1 } };
        for (int index = 0; index < 4; index++)
        {
            int inputA = inputs[index, 0], inputB = inputs[index, 1];

            ComplexNeuron biasNeuron1 = new ComplexNeuron(2, 0);  
            ComplexNeuron inputNeuronA = new ComplexNeuron(1, 0); 
            ComplexNeuron inputNeuronB = new ComplexNeuron(0, 0);   
            ComplexNeuron biasNeuron2 = new ComplexNeuron(3, 3, 1); 

            ComplexCumulativeNeuron andDetectorNeuron = new ComplexCumulativeNeuron(1, 3, 1);
            ComplexCumulativeNeuron norDetectorNeuron = new ComplexCumulativeNeuron(0, 3, 1);

            ComplexCumulativeNeuron finalOutputNeuron = new ComplexCumulativeNeuron(6, 2, 1);

            biasNeuron1.Attach(-30, andDetectorNeuron);
            biasNeuron1.Attach(10, norDetectorNeuron);
            inputNeuronA.Attach(20, andDetectorNeuron);
            inputNeuronA.Attach(-20, norDetectorNeuron);
            inputNeuronB.Attach(20, andDetectorNeuron);
            inputNeuronB.Attach(-20, norDetectorNeuron);

            biasNeuron2.Attach(-10, finalOutputNeuron);
            andDetectorNeuron.Attach(20, finalOutputNeuron);
            norDetectorNeuron.Attach(20, finalOutputNeuron);

            biasNeuron1.Fire(1);
            biasNeuron2.Fire(1);
            inputNeuronA.Fire(inputA);
            inputNeuronB.Fire(inputB);

            Console.WriteLine("  " + inputA + "    " + inputB
                + "    " + andDetectorNeuron.LastSignal().ToString("F4")
                + "       " + norDetectorNeuron.LastSignal().ToString("F4")
                + "       " + finalOutputNeuron.LastSignal().ToString("F4")
                + "       ->  " + Threshold(finalOutputNeuron.LastSignal()));
        }
        PrintDivider();
        Console.WriteLine();
    }

    static int Threshold(double signal)
    {
        return signal >= 0.5 ? 1 : 0;
    }
}