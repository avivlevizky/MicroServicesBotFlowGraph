using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using static Contracts.Model.MongoDbModel.Options;

namespace Contracts.Model
{
    public class ChatFlowGraph
    {

        public List<Vertex> Vertices = new List<Vertex>();
        public List<Edge> Edges = new List<Edge>();
        public int Start { get; set; }
        public List<ChatItem> OtherItems { get; set; }
        public string GraphId { get; set; }

        public string Description { get; set; }
        public string UserQuestion { get; set; }


        public int? CurrentState;

        public Vertex GetNextStep(string edge)
        {
            try
            {

                var stateDebug = CurrentState;
                if (CurrentState == null)
                {
                    var start = Vertices.First(v => v.Type == FlowType.Start);
                    CurrentState = Edges.Single(e => e.Start == start.VertexId).End;
                }
                else
                {
                    var indexDebug = Edges.FirstOrDefault(e =>
                        e.Start == CurrentState && IsMatch(e.Cond.ToLower(), edge.ToLower()))?.End;

                    var index = Edges.FirstOrDefault(e => e.Start == CurrentState && IsMatch(e.Cond.ToLower(), edge.ToLower()))?.End ??
                                Edges.FirstOrDefault(e => e.Start == CurrentState && e.Cond.Equals(string.Empty))?.End;
                    CurrentState = Vertices.Single(a => a.VertexId == index)?.VertexId; //if null will go to default option without text

                }
                return Vertices.Single(a => a.VertexId == CurrentState);
            }
            catch (Exception e)
            {
                if (Vertices.Single(a => a.VertexId == CurrentState).Type == FlowType.Question)
                {
                    return Vertices.Single(a => a.VertexId == CurrentState);
                }
                return null;
            }
        }

        private bool IsMatch(string edge, string param)
        {
            edge = edge.Trim();
            param = param.Trim();
            if (edge.ToLower().Equals(param.ToLower()))
            {
                return true;
            }

            if (edge.StartsWith(">") && int.TryParse(edge.Substring(1), out int paramVal) && int.TryParse(param, out int edgeVal))
            {
                return edgeVal > paramVal;
            }

            if (param.StartsWith("<") && int.TryParse(param.Substring(1), out paramVal) && int.TryParse(edge, out edgeVal))
            {
                return paramVal < edgeVal;
            }
            return false;

        }

        public List<Edge> GetOutgoingEdges()
        {
            return Edges.Where(e => e.Start == CurrentState).ToList();
        }

        public bool HasNeighbors()
        {
            if (CurrentState == null)
            {
                return false;
            }
            return Edges.Any(e => e.Start == CurrentState);
        }

        public void ReplaceAfterEval(Edge edg, List<string> lst)
        {
            var toAddEdges = lst.Select(str => new Edge() { Start = edg.Start, End = edg.End, Cond = str }).ToList();
            int index = Edges.IndexOf(edg);
            Edges.Remove(edg);
            foreach (var toAddEdge in toAddEdges)
            {
                Edges.Insert(index, toAddEdge);
            }
        }
    }

    public class Edge
    {
        public int Start { get; set; }
        public int End { get; set; }
        public String Cond { get; set; }
    }

    public class Vertex
    {
        public FlowType Type { get; set; }
        public string Text { get; set; }
        public int VertexId { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum FlowType
    {
        Condition,
        Action,
        Message,
        Question,
        Cache,
        Start,
        Escape,
        Video,
        Image
    }
    public abstract class Condition
    {
    }
    public class Default : Condition { }
    public class DataCondition : Condition
    {
        public string Value;
    }

    public class CompareCondition : Condition
    {
        public ConditionType Type { get; set; }
        public object Value { get; set; }

        internal bool Match(string edge)
        {
            throw new NotImplementedException();
        }
    }

    public enum ConditionType
    {
        Greater,
        GreaterOrEquale,
        Equale,
        LowerOrEquale,
        Lower
    }
}
