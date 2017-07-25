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

    public class Rules
    {
        public Rounds[] childs;

        public static Rules get(string name)
        {
            Rules builder = new Rules();
            return builder;
        }

        public Rules start(Rounds child)
        {
            childs = new Rounds[1];
            childs[0] = child;
            return this;
        }

        public Rules next(Rounds child)
        {
            childs = Arrays.add(childs, child);
            return this;
        }

        public Rule build()
        {
            Rule obj = new Rule();
            obj.childs = new Round[this.childs.Length];
            for (int i = 0; i < this.childs.Length; i++)
                obj.childs[i] = this.childs[i].build();
            return obj;
        }
    }

    public class Rounds
    {
        public Phases[] childs;

        public static Rounds get(string name)
        {
            Rounds builder = new Rounds();
            return builder;
        }

        public Rounds start(Phases child)
        {
            childs = new Phases[1];
            childs[0] = child;
            return this;
        }

        public Rounds next(Phases child)
        {
            childs = Arrays.add(childs, child);
            return this;
        }

        public Round build()
        {
            Round obj = new Round();
            obj.childs = new Phase[this.childs.Length];
            for (int i = 0; i < this.childs.Length; i++)
                obj.childs[i] = this.childs[i].build();
            return obj;
        }
    }

    public class Phases
    {
        public Func<Game, Game>[] childs;

        public static Phases get(string name)
        {
            Phases builder = new Phases();
            /* builder.obj = new Phase();*/
            /* builder.obj.name = name;*/
            return builder;
        }

        public Phases start(Func<Game, Game> child)
        {
            childs = new Func<Game, Game>[1];
            childs[0] = child;
            return this;
        }

        public Phases next(Func<Game, Game> child)
        {
            childs = Arrays.add(childs, child);
            return this;
        }

        public Phase build()
        {
            Phase obj = new Phase();
            obj.childs = new Func<Game, Game>[this.childs.Length];
            for (int i = 0; i < this.childs.Length; i++)
                obj.childs[i] = this.childs[i];
            return obj;
        }
    }

    // IMPL

    public class Rule : GameStructure<Round>
    {/*
        public Round getRound(string name)
        {
            foreach (Round i in childs)
                if (name.Equals(i.name))
                    return i;
            throw new Exception();
        }*/
    }

    public class Round : GameStructure<Phase>
    {
        public Game execute(Game state)
        {
            foreach (Phase i in childs)
                state = i.execute(state);
            return state;
        }
    }

    public class Phase
    {
        /* public string name; */
        public Func<Game, Game>[] childs;

        public Game execute(Game state)
        {
            foreach (Func<Game, Game> i in childs)
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
