using System;
using Commons.Lang;

namespace Commons.Game
{
    // TODO decision
    // TODO fplit
    // TODO flow

    // MANAGER
    public abstract class GameManager
    {
        public string[][] input;
        public string[][] output;
        public abstract void Initialize();
    }

    // RULES
    // Game
    // Mode
    // Turn
    // Phase
    // Steps

    public class Games : RuleBuilder<Turns>
    {
        public static Games Get(string name)
        {
            return new Games()
            {
                name = name,
                childs = new Turns[0]
            };
        }

        public Games Start(Turns child)
        {
            return Next(child);
        }

        public Games Next(Turns child)
        {
            childs = Arrays.Add(childs, child);
            return this;
        }

        public Game Build()
        {
            Console.WriteLine("Rule: " + name + " build()");
            Game obj = new Game()
            {
                name = name,
                childs = new Turn[this.childs.Length]
            };
            for (int i = 0; i < this.childs.Length; i++)
                obj.childs[i] = this.childs[i].Build();
            Console.WriteLine("Done.");
            Console.WriteLine(" ");
            return obj;
        }
    }

    public class Turns : RuleBuilder<Phases>
    {
        public static Turns Get(string name)
        {
            return new Turns()
            {
                name = name,
                childs = new Phases[0]
            };
        }

        public Turns Start(Phases child)
        {
            return Next(child);
        }

        public Turns Next(Phases child)
        {
            childs = Arrays.Add(childs, child);
            return this;
        }

        public Turn Build()
        {
            Turn obj = new Turn()
            {
                name = name,
                childs = new Phase[this.childs.Length]
            };
            for (int i = 0; i < this.childs.Length; i++)
                obj.childs[i] = this.childs[i].Build();
            return obj;
        }
    }

    public class Phases : RuleBuilder<Action>
    {
        public static Phases Get(string name)
        {
            return new Phases()
            {
                name = name,
                childs = new Action[0]
            };
        }

        public Phases Start(Action child)
        {
            return Next(child);
        }

        public Phases Next(Action child)
        {
            childs = Arrays.Add(childs, child);
            return this;
        }

        public Phase Build()
        {
            Phase obj = new Phase()
            {
                name = name,
                childs = new Action[this.childs.Length]
            };
            for (int i = 0; i < this.childs.Length; i++)
                obj.childs[i] = this.childs[i];
            return obj;
        }
    }

    // IMPL

    public class Game : Rule<Turn>
    {
        public Turn Turn(string child)
        {
            Console.WriteLine("Turn: " + name + ", load: " + child);
            foreach (Turn i in childs)
                if (child.Equals(i.name))
                    return i;
            throw new Exception("Not found");
        }
    }

    public class Turn : Rule<Phase>
    {
        public void Execute()
        {
            Console.WriteLine("  Turn: " + name + ", execute...");
            foreach (Phase i in childs)
                i.Execute();
            Console.WriteLine("  Done.");
            Console.WriteLine(" ");
        }
    }

    public class Phase : Rule<Action>
    {
        public void Execute()
        {
            Console.WriteLine("    Phase: " + name);
            foreach (Action i in childs)
            {
                Console.WriteLine("      Step: " + i.Method.Name);
                i.Invoke();
            }
        }
    }

    // RULE ENGINE

    public abstract class Rule<T>
    {
        public string name;
        public T[] childs;
    }

    public abstract class RuleBuilder<T>
    {
        public string name;
        public int level;
        public T[] childs;
    }
}
