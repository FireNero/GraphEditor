﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using GraphEditor.Tools;

namespace GraphEditor.Converters
{
	class ToolTypeToBoolConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var type = value is ToolType ? (ToolType) value : ToolType.None;
			switch (parameter.ToString().ToLowerInvariant())
			{
				case "vertex":
					return ToolType.Ellipse == type;
				case "edge":
					return ToolType.Edge == type;
				case "remove":
					return ToolType.Eraser == type;
				default:
					return ToolType.Pointer;

			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var boolean = value is bool && (bool) value;
			switch (parameter.ToString().ToLowerInvariant())
			{
				case "vertex":
					return boolean ? ToolType.Ellipse : ToolType.Pointer;
				case "edge":
					return boolean ? ToolType.Edge : ToolType.Pointer;
				case "remove":
					return boolean ? ToolType.Eraser : ToolType.Pointer;
				default:
					return ToolType.Pointer;
			}
		}
	}
}
