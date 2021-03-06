﻿namespace GraphEditor.Commands
{
	internal abstract class CommandBase
	{
		public abstract void Undo(GraphCanvas drawingCanvas);
		public abstract void Redo(GraphCanvas drawingCanvas);
	}
}