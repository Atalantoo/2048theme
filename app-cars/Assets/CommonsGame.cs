using System;
using Commons.Lang;

namespace Commons.Game
{
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

    public class Rules<G> where G:Game
    {
        public Rounds<G> [] childs;

        public static Rules<G> get(string name)
        {
            Rules<G> builder = new Rules<G>();
            return builder;
        }

        public Rules<G> start(Rounds<G> child)
        {
            childs = new Rounds<G>[1];
            childs[0] = child;
            return this;
        }

        public Rules<G> next(Rounds<G> child)
        {
            childs = Arrays.add(childs, child);
            return this;
        }

        public Rule<G> build()
        {
            Rule<G> obj = new Rule<G>();
            obj.childs = new Round<G>[this.childs.Length];
            for (int i = 0; i < this.childs.Length; i++)
                obj.childs[i] = this.childs[i].build();
            return obj;
        }
    }

    public class Rounds<G> where G : Game
    {
        public Phases<G>[] childs;

        public static Rounds<G>  get(string name)
        {
            Rounds<G>  builder = new Rounds<G> ();
            return builder;
        }

        public Rounds<G>  start(Phases<G> child)
        {
            childs = new Phases<G>[1];
            childs[0] = child;
            return this;
        }

        public Rounds<G>  next(Phases<G> child)
        {
            childs = Arrays.add(childs, child);
            return this;
        }

        public Round<G> build()
        {
            Round<G> obj = new Round<G>();
            obj.childs = new Phase<G>[this.childs.Length];
            for (int i = 0; i < this.childs.Length; i++)
                obj.childs[i] = this.childs[i].build();
            return obj;
        }
    }

    public class Phases<G> where G : Game
    {
        public Func<G, G>[] childs;

        public static Phases<G> get(string name)
        {
            Phases<G> builder = new Phases<G>();
            /* builder.obj = new Phase();*/
            /* builder.obj.name = name;*/
            return builder;
        }

        public Phases<G> start(Func<G, G> child)
        {
            childs = new Func<G, G>[1];
            childs[0] = child;
            return this;
        }

        public Phases<G> next(Func<G, G> child)
        {
            childs = Arrays.add(childs, child);
            return this;
        }

        public Phase<G> build()
        {
            Phase<G> obj = new Phase<G>();
            obj.childs = new Func<G, G>[this.childs.Length];
            for (int i = 0; i < this.childs.Length; i++)
                obj.childs[i] = this.childs[i];
            return obj;
        }
    }

    // IMPL

    public class Rule<G> : GameStructure<Round<G>> where G : Game
    {/*
        public Round getRound(string name)
        {
            foreach (Round i in childs)
                if (name.Equals(i.name))
                    return i;
            throw new Exception();
        }*/
    }

    public class Round<G> : GameStructure<Phase<G>> where G: Game
    {
        public G execute(G game)
        {
            foreach (Phase<G> i in childs)
                game = i.execute(game);
            return game;
        }
    }

    public class Phase<G> where G : Game
    {
        /* public string name; */
        public Func<G, G>[] childs;

        public G execute(G state)
        {
            foreach (Func<G, G> i in childs)
                state = i.Invoke(state);
            return state;
        }
    }


    public class GameStructure<TC>
    {
        /* public string name;*/
        public TC[] childs;
        /*public Func<GameState, GameState>[] funcs;*/
    }

    public class GameStructureBuilder<T, TC>
    {
        public GameStructure<TC> obj;

        public static GameStructureBuilder<T, TC> get(string name)
        {
            GameStructureBuilder<T, TC> build = new GameStructureBuilder<T, TC>();
            build.obj = new GameStructure<TC>();
            /* build.obj.name = name;*/
            return build;
        }

        public GameStructureBuilder<T, TC> start(TC child)
        {
            obj.childs[0] = child;
            return this;
        }
        public GameStructureBuilder<T, TC> start(Func<GameState, GameState> func)
        {
            throw new NotImplementedException();
        }

        public GameStructureBuilder<T, TC> next(TC child)
        {
            obj.childs[1] = child;
            return this;
        }

        public GameStructure<TC> build()
        {
            return obj;
        }

    }
}
