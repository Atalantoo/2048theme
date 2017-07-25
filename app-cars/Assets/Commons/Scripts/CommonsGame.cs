using System;
using Commons.Lang;

namespace Commons.Game
{
    // TODO decision
    // TODO fplit
    // TODO flow

    class CommonsGame
    {
    }

    public interface IGamemanager
    {
        // GameState turn(GameState inputs);
    }

    // API
    public class Game
    {
        public string[][] input;
        public string[][] output;
    }

    public class Parties
    {
        public string _name;
        public Rounds[] childs;

        public static Parties Get(string name)
        {
            return new Parties()
            {
                _name = name,
                childs = new Rounds[0]
            };
        }

        public Parties Start(Rounds child)
        {
            return Next(child);
        }

        public Parties Next(Rounds child)
        {
            childs = Arrays.Add(childs, child);
            return this;
        }

        public Party Build()
        {
            Console.WriteLine("Rule: " + _name + " build()");
            Party obj = new Party()
            {
                name = _name,
                childs = new Round[this.childs.Length]
            };
            for (int i = 0; i < this.childs.Length; i++)
                obj.childs[i] = this.childs[i].Build();
            Console.WriteLine("Done.");
            Console.WriteLine(" ");
            return obj;
        }
    }

    public class Rounds
    {
        public string _name;
        public Phases[] childs;

        public static Rounds Get(string name)
        {
            return new Rounds()
            {
                _name = name,
                childs = new Phases[0]
            };
        }

        public Rounds Start(Phases child)
        {
            return Next(child);
        }

        public Rounds Next(Phases child)
        {
            childs = Arrays.Add(childs, child);
            return this;
        }

        public Round Build()
        {
            Round obj = new Round()
            {
                name = this._name,
                childs = new Phase[this.childs.Length]
            };
            for (int i = 0; i < this.childs.Length; i++)
                obj.childs[i] = this.childs[i].Build();
            return obj;
        }
    }

    public class Phases
    {
        public string _name;
        public Action[] childs;

        public static Phases Get(string name)
        {
            return new Phases()
            {
                _name = name,
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
                name = _name,
                childs = new Action[this.childs.Length]
            };
            for (int i = 0; i < this.childs.Length; i++)
                obj.childs[i] = this.childs[i];
            return obj;
        }
    }

    // IMPL

    public class Party : GameStructure<Round>
    {
        public Round Round(string child)
        {
            Console.WriteLine("Rule: " + name + ", load: " + child);
            foreach (Round i in childs)
                if (child.Equals(i.name))
                    return i;
            throw new Exception("Not found");
        }
    }

    public class Round : GameStructure<Phase>
    {
        public void Execute()
        {
            Console.WriteLine("  Round: " + name + ", execute...");
            foreach (Phase i in childs)
                i.Execute();
            Console.WriteLine("  Done.");
            Console.WriteLine(" ");
        }
    }

    public class Phase : GameStructure<Action>
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


    public abstract class GameStructure<TC>
    {
        public string name;
        public TC[] childs;
    }

}
