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

    public class Rules 
    {
        public string name;
        public Rounds [] childs;

        public static Rules Get(string name)
        {
            Rules builder = new Rules();
            builder.name = name;
            return builder;
        }

        public Rules Start(Rounds child)
        {
            childs = new Rounds[1];
            childs[0] = child;
            return this;
        }

        public Rules Next(Rounds child)
        {
            childs = Arrays.Add(childs, child);
            return this;
        }

        public Rule Build()
        {
            Console.WriteLine("Rule: "+name+" build()");
            Rule obj = new Rule();
            obj.name = name;
            obj.childs = new Round[this.childs.Length];
            for (int i = 0; i < this.childs.Length; i++)
                obj.childs[i] = this.childs[i].Build();
            Console.WriteLine("Done.");
            Console.WriteLine(" ");
            return obj;
        }
    }

    public class Rounds 
    {
        public string name;
        public Phases[] childs;

        public static Rounds  Get(string name)
        {
            Rounds  builder = new Rounds ();
            builder.name = name;
            return builder;
        }

        public Rounds  Start(Phases child)
        {
            childs = new Phases[1];
            childs[0] = child;
            return this;
        }

        public Rounds  Next(Phases child)
        {
            childs = Arrays.Add(childs, child);
            return this;
        }

        public Round Build()
        {
            Round obj = new Round();
            obj.name = name;
            obj.childs = new Phase[this.childs.Length];
            for (int i = 0; i < this.childs.Length; i++)
                obj.childs[i] = this.childs[i].Build();
            return obj;
        }
    }

    public class Phases 
    {
        public string name;
        public Action[] childs;

        public static Phases Get(string name)
        {
            Phases builder = new Phases();
            builder.name = name;
            return builder;
        }

        public Phases Start(Action child)
        {
            childs = new Action[1];
            childs[0] = child;
            return this;
        }

        public Phases Next(Action child)
        {
            childs = Arrays.Add(childs, child);
            return this;
        }

        public Phase Build()
        {
            Phase obj = new Phase();
            obj.name = name;
            obj.childs = new Action[this.childs.Length];
            for (int i = 0; i < this.childs.Length; i++)
                obj.childs[i] = this.childs[i];
            return obj;
        }
    }

    // IMPL

    public class Rule : GameStructure<Round> 
    {
        public Round GetRound(string child)
        {
            Console.WriteLine("Rule: " + name + ", load: "+ child);
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
            Console.WriteLine("  Round: "+ name + ", execute...");
            foreach (Phase i in childs)
                i.Execute();
            Console.WriteLine("  Done.");
            Console.WriteLine(" ");
        }
    }

    public class Phase 
    {
        public string name;
        public Action[] childs;

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


    public class GameStructure<TC>
    {
        public string name;
        public TC[] childs;
        /*public Func<GameState, GameState>[] funcs;*/
    }

    public class GameStructureBuilder<T, TC>
    {
        public GameStructure<TC> obj;

        public static GameStructureBuilder<T, TC> Get(string name)
        {
            GameStructureBuilder<T, TC> build = new GameStructureBuilder<T, TC>();
            build.obj = new GameStructure<TC>();
            /* build.obj.name = name;*/
            return build;
        }

        public GameStructureBuilder<T, TC> Start(TC child)
        {
            obj.childs[0] = child;
            return this;
        }
        public GameStructureBuilder<T, TC> Start(Func<GameState, GameState> func)
        {
            throw new NotImplementedException();
        }

        public GameStructureBuilder<T, TC> Next(TC child)
        {
            obj.childs[1] = child;
            return this;
        }

        public GameStructure<TC> Build()
        {
            return obj;
        }

    }
}
