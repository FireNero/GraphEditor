using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using GraphEditor.Graphics;
using GraphEditor.GraphStruct;

namespace GraphEditor.Algorithms
{
	internal class DijkstraAlgorithm : SelectionAlgorithm
	{
		public DijkstraAlgorithm(GraphCanvas drawingCanvas) : base(drawingCanvas)
		{
		}

		protected override bool IsInputCorrect(out string message)
		{
			if (DrawingCanvas.IsOrientedGraph)
			{
				message = "Algorithms are available only for not-oriented graphs.";
				return false;
			}
			if (GetSelectedVertices().Count != 2)
			{
				message = "Only 2 vertices must be selected.";
				return false;
			}
			if (!DrawingCanvas.GraphStructure.GetAllEdges().All(edge => edge.Weight >= 0))
			{
				message = "Graph must not contain edges with negative weight.";
				return false;
			}
			message = "There is no path between selected vertices.";
			var isUsed = SameTreeChecking();
			return isUsed[(GraphElementVertex) HelperFunctions.GetGraphElement(DrawingCanvas, GetSelectedVertices()[0])] &&
			       isUsed[(GraphElementVertex) HelperFunctions.GetGraphElement(DrawingCanvas, GetSelectedVertices()[1])];
		}

		protected override AlgorithmResult RunAlgorithm()
		{
			var vertices = DrawingCanvas.GraphStructure.Vertices;
			int verticesCount = vertices.Count;
			var startVertex = (GraphElementVertex) HelperFunctions.GetGraphElement(DrawingCanvas, GetSelectedVertices().First());

			var dest = new Dictionary<GraphElementVertex, double>(verticesCount);
			var used = new Dictionary<GraphElementVertex, bool>(verticesCount);
			var path = new Dictionary<GraphElementVertex, GraphElementVertex>();

			var resultVertexes = new List<GraphElementVertex>();
			var resultEdges = new HashSet<GraphElementEdge>();

			for (int i = 0; i < verticesCount; i++)
			{
				dest.Add(vertices[i], Double.PositiveInfinity);
				used.Add(vertices[i], false);
				path.Add(vertices[i], null);
			}
			dest[startVertex] = 0.0;
			for (int i = 0; i < verticesCount; i++)
			{
				GraphElementVertex vertex = null;
				for (int j = 0; j < verticesCount; j++)
				{
					if (!used[vertices[j]] && (vertex == null || dest[vertices[j]] < dest[vertex]))
					{
						vertex = vertices[j];
					}
				}
				if (Double.IsPositiveInfinity(dest[vertex]))
					break;
				used[vertex] = true;
				foreach (var edge in vertex.Connections)
				{
					var to = edge.Begin == vertex ? edge.End : edge.Begin;
					double cost = edge.Weight;
					if (dest[vertex] + cost < dest[to])
					{
						dest[to] = dest[vertex] + cost;
						path[to] = vertex;
					}
				}
			}

			var endVertex = (GraphElementVertex) HelperFunctions.GetGraphElement(DrawingCanvas, GetSelectedVertices()[1]);
			for (var vertex = endVertex; vertex != startVertex; vertex = path[vertex])
			{
				resultVertexes.Add(vertex);
				resultEdges.Add(DrawingCanvas.GraphStructure.GetEdgeBetweenVertex(vertex, path[vertex]));
			}
			resultVertexes.Add(startVertex);
			var result = new List<GraphElementBase>(resultVertexes);
			result.AddRange(resultEdges);
			return new AlgorithmResult(dest[endVertex], result);
		}


	}
}