using AdventOfCode.Utility;
using System.Linq;
using System;
using System.Collections.Generic;

namespace AdventOfCode.Year2019
{
    static class Solver2019_23
    {
        public static string Solve(IInputResolver input, bool second = false)
        {
            var instructions = new long[10_000];
            var inData = input.AsEnumerable()
                .First().Split(',')
                .Select(s => long.Parse(s)).ToArray();
            inData.CopyTo(instructions, 0);

            int compCount = 50;
            var network = new Dictionary<int, IntCompWithNic>(compCount);
            for (int i = 0; i < compCount; i++)
            {
                var c = new IntCompWithNic(i, instructions);
                network.Add(i, c);
                Console.WriteLine(c);
            }

            // Then just solve the problem...
            var done = false;
            int itt = 0;
            var nat = new Nat(network);
            while(!done)
            {
                long tickRes = -1;
                foreach (var comp in network)
                {
                    tickRes = comp.Value.Tick(network, nat, itt);
                    if (!second && tickRes != -1)
                        return tickRes.ToString();
                }
                itt++;
                var natResult = nat.CheckIfIdle();
                if (second && natResult != -1)
                    return natResult.ToString();
            }

            return "-1";
        }
    }

    public class Nat
    {
        public (long x, long y) LastMessage { get; set; }
        public HashSet<long> SentToZero { get; set; }
        public Dictionary<int, IntCompWithNic> Network { get; set; }
        public Nat(Dictionary<int, IntCompWithNic> net)
        {
            SentToZero = new HashSet<long>();
            Network = net;
        }

        public void SetNatMsg((long x, long y) msg)
            => LastMessage = msg;

        public long CheckIfIdle()
        {
            var allIdle = true;
            foreach (var c in Network)
            {
                var notIdle = c.Value.NicQueue.Count > 0
                    || c.Value.Comp.ProvidedOutput;
                if (notIdle)
                {
                    allIdle = false;
                    break;
                }
            }
            if(allIdle)
            {
                var c0 = Network[0];
                c0.AddToQueue(LastMessage);
                if(SentToZero.Contains(LastMessage.y))
                {
                    return LastMessage.y;
                }
                SentToZero.Add(LastMessage.y);
            }
            return -1;
        }
    }

    public class IntCompWithNic
    {
        public int Id { get; set; }
        internal IntcodeComputer Comp { get; set; }
        public Queue<(long x, long y)> NicQueue { get; set; }

        public IntCompWithNic(int id, long[] inst)
        {
            Id = id;
            Comp = new IntcodeComputer(inst);
            Comp.RunProgram(null);
            Comp.RunProgram(id); // First run to set id.
            Comp.RunProgram(-1); // One empty run to get things going (tested).
            NicQueue = new Queue<(long x, long y)>();
        }

        public override string ToString()
            => $"Id: {Id}, Comp: {Comp.ToString()}, OP: {Comp.LastOutput}, Q: {NicQueue.Count}";

        public long Tick(Dictionary<int, IntCompWithNic> net, Nat nat, int itt)
        {
            if (Comp.Halted) return -1;
            else if(Comp.NeedNewInput)
            {
                if (NicQueue.Count == 0)
                    Comp.RunProgram(-1);
                else
                {
                    var (x, y) = NicQueue.Dequeue();
                    Comp.RunProgram(x);
                    Comp.RunProgram(y);
                }
            }
            else if (Comp.ProvidedOutput)
            {
                var address = Comp.LastOutput;
                Comp.RunProgram(null);
                var x = Comp.LastOutput;
                Comp.RunProgram(null);
                var y = Comp.LastOutput;
                // Need to run again, else we will pick up the y as address next tick.
                Comp.RunProgram(null);

                var msg = (x, y);
                if (address == 255)
                {
                    nat.SetNatMsg(msg);
                    return y;
                }
                else
                    SendTo(address, msg, net);
            }
            return -1;
        }

        public void AddToQueue((long x, long y) msg)
        {
            NicQueue.Enqueue(msg);
        }

        private void SendTo(long address, (long x, long y) msg, Dictionary<int, IntCompWithNic> net)
        {
            var c = net[(int)address];
            c.AddToQueue(msg);
        }
    }
}
