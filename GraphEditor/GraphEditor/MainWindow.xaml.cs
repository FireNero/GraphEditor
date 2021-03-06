﻿using System;
using System.Windows;
using GraphEditor.Algorithms;
using GraphEditor.IO;
using GraphEditor.Tools;
using Microsoft.Win32;

namespace GraphEditor
{
	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private string currentFileName = "";

		public MainWindow()
		{
			InitializeComponent(); 
		}

		#region Buttons event handlers

		#region Graph Edit Buttons

		private void AddEdgeButton_OnChecked(object sender, RoutedEventArgs e)
		{
			DrawingGraphCanvas.Tool = ToolType.Edge;
		}

		private void AddEdgeButton_OnUnchecked(object sender, RoutedEventArgs e)
		{
			if (DrawingGraphCanvas.Tool == ToolType.Edge)
				DrawingGraphCanvas.Tool = ToolType.Pointer;
		}

		private void AddVertexButton_OnChecked(object sender, RoutedEventArgs e)
		{
			DrawingGraphCanvas.Tool = ToolType.Ellipse;
		}

		private void AddVertexButton_OnUnchecked(object sender, RoutedEventArgs e)
		{
			if (DrawingGraphCanvas.Tool == ToolType.Ellipse)
				DrawingGraphCanvas.Tool = ToolType.Pointer;
		}

		private void RemoveVertexOrNodeButton_OnClick(object sender, RoutedEventArgs e)
		{
			DrawingGraphCanvas.Delete();
		}

		private void UndoButton_OnClick(object sender, RoutedEventArgs e)
		{
			DrawingGraphCanvas.Undo();
		}

		private void RedoButton_OnClick(object sender, RoutedEventArgs e)
		{
			DrawingGraphCanvas.Redo();
		}

		private void OrientedGraphToggleButton_OnChecked(object sender, RoutedEventArgs e)
		{
			DrawingGraphCanvas.IsOrientedGraph = true;
		}

		private void OrientedGraphToggleButton_OnUnchecked(object sender, RoutedEventArgs e)
		{
			DrawingGraphCanvas.IsOrientedGraph = false;
		}

		#endregion

		#region IO Buttons

		private void NewFileButton_OnClick(object sender, RoutedEventArgs e)
		{
			if (IsSaveAndContinue())
			{
				New();
			}
		}

		private void OpenFileButton_OnClick(object sender, RoutedEventArgs e)
		{
			if (IsSaveAndContinue())
			{
				Open();
			}
		}

		private void SaveButton_OnClick(object sender, RoutedEventArgs e)
		{
			Save();
		}

		private void SaveAsButton_OnClick(object sender, RoutedEventArgs e)
		{
			SaveAs();
		}

		#endregion

		#region Application Buttons

		private void MinimizeButton_OnClick(object sender, RoutedEventArgs e)
		{
			WindowState = WindowState.Minimized;
		}

		private void ExitButton_OnClick(object sender, RoutedEventArgs e)
		{
			Close();
		}

		#endregion

		#region Algo Buttons

		private void KruskalAlgorithmButton_OnClick(object sender, RoutedEventArgs e)
		{
			var kruskalAlgorithm = new KruskalAlgorithm(DrawingGraphCanvas);
			try
			{
				MessageBox.Show(String.Format("Total cost: {0}.", kruskalAlgorithm.Execute()), "Algorithm result");
			}
			catch (ArgumentException argumentException)
			{
				MessageBox.Show(argumentException.Message, "Algorithm error");
			}
		}

		private void BfsAlgorithmButton_OnClick(object sender, RoutedEventArgs e)
		{
			var bfs = new BfsAlgorithm(DrawingGraphCanvas);
			try
			{
				MessageBox.Show(String.Format("Minimum lenght: {0}.", bfs.Execute()), "Algorithm result");
			}
			catch (ArgumentException argumentException)
			{
				MessageBox.Show(argumentException.Message, "Algorithm error");
			}
		}

		private void DijkstraAlgorithmButton_OnClick(object sender, RoutedEventArgs e)
		{
			var dijkstra = new DijkstraAlgorithm(DrawingGraphCanvas);
			try
			{
				MessageBox.Show(String.Format("Total cost: {0}.", dijkstra.Execute()), "Algorithm result");
			}
			catch (ArgumentException argumentException)
			{
				MessageBox.Show(argumentException.Message, "Algorithm error");
			}
		}

		private void FleuryAlgorithmButton_OnClick(object sender, RoutedEventArgs e)
		{
			var fleury = new FleuryAlgorithm(DrawingGraphCanvas);
			try
			{
				MessageBox.Show(String.Format("Cycle consist of {0} vertices.", fleury.Execute()), "Algorithm result");
			}
			catch (ArgumentException argumentException)
			{
				MessageBox.Show(argumentException.Message, "Algorithm error");
			}
		}

		#endregion

		#endregion

		#region Methods

		private void New()
		{
			DrawingGraphCanvas.Clear();
			currentFileName = "";
		}

		/// <summary>
		///     Open new graph
		/// </summary>
		/// <returns>New file name if open success else old file name.</returns>
		/// >
		private void Open()
		{
			var openFileDialog = new OpenFileDialog
			                     {
				                     Filter = "XML files (*.xml)|*.xml|All Files|*.*",
				                     DefaultExt = "xml",
			                     };
			if (openFileDialog.ShowDialog() != false)
			{
				try
				{
					InputOutputService.Load(openFileDialog.FileName, DrawingGraphCanvas);
					currentFileName = openFileDialog.FileName;
				}
				catch (Exception)
				{
					MessageBox.Show("Can't open selected file.", Title, MessageBoxButton.OK);
				}
			}
		}

		private void Save()
		{
			if (currentFileName == "")
			{
				SaveAs();
			}
			else
			{
				Save(currentFileName);
			}
		}

		private void SaveAs()
		{
			var saveFileDialog = new SaveFileDialog
			                     {
				                     Filter = "XML files (*.xml)|*.xml|All Files|*.*",
				                     DefaultExt = "xml",
			                     };

			if (saveFileDialog.ShowDialog() != false)
			{
				Save(saveFileDialog.FileName);
			}
		}

		private void Save(string filename)
		{
			try
			{
				InputOutputService.Save(filename, DrawingGraphCanvas);
				currentFileName = filename;
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Saving error.", MessageBoxButton.OK);
			}
		}

		private MessageBoxResult ConfirmSave()
		{
			return MessageBox.Show("Do you want to save changes?", Title, MessageBoxButton.YesNoCancel);
		}

		private bool IsSaveAndContinue()
		{
			if (DrawingGraphCanvas.IsDirty)
			{
				switch (ConfirmSave())
				{
					case MessageBoxResult.Yes:
						Save();
						return true;
					case MessageBoxResult.Cancel:
						return false;
					default:
						return true;
				}
			}
			return true;
		}

		#endregion
	}
}