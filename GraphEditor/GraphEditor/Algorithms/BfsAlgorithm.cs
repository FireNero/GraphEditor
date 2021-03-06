using System.Collections.Generic;
using System.Linq;
using GraphEditor.Graphics;
using GraphEditor.GraphStruct;

namespace GraphEditor.Algorithms
{
	internal class BfsAlgorithm : SelectionAlgorithm
	{
		public BfsAlgorithm(GraphCanvas drawingCanvas) : base(drawingCanvas)
		{
		}

		protected override bool IsInputCorrect(out string message)
		{
			if (DrawingCanvas.IsOrientedGraph)
			{
				message = "Algorithms are available only for not-oriented graphs.";
				return false;
			}
			message = "The only vertex must be selected.";
			return GetSelectedVertices().Count == 1;
		}

		protected override AlgorithmResult RunAlgorithm()
		{
			var vertices = DrawingCanvas.GraphStructure.Vertices;
			int verticesCount = vertices.Count;
			var startVertex = (GraphElementVertex) HelperFunctions.GetGraphElement(DrawingCanvas, GetSelectedVertices().First());
			var queue = new Queue<GraphElementVertex>();
			queue.Enqueue(startVertex);
			var isUsed = new Dictionary<GraphElementVertex, bool>(verticesCount);
			for (int i = 0; i < verticesCount; i++)
			{
				isUsed.Add(DrawingCanvas.GraphStructure.Vertices[i], false);
			}


			isUsed[startVertex] = true;
			var resultVertexes = new List<GraphElementVertex> {startVertex};
			var resultEdges = new HashSet<GraphElementEdge>();
			while (queue.Count != 0)
			{
				var vertex = queue.Dequeue();
				foreach (var edge in vertex.Connections)
				{
					var to = edge.Begin == vertex ? edge.End : edge.Begin;
					if (!isUsed[to])
					{
						isUsed[to] = true;
						queue.Enqueue(to);

						resultVertexes.Add(to);
						resultEdges.Add(edge);
					}
				}
			}

			var result = new List<GraphElementBase>(resultVertexes);
			result.AddRange(resultEdges);
			return new AlgorithmResult(0, result);
		}
	}
}