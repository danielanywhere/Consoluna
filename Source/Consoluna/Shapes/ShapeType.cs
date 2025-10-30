/*
 * Copyright (c). 2025 Daniel Patterson, MCSD (danielanywhere).
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolunaLib.Shapes
{
	//*-------------------------------------------------------------------------*
	//*	ShapeType																																*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Enumeration of recognized shape types.
	/// </summary>
	public enum ShapeType
	{
		/// <summary>
		/// No shape specified or unknown.
		/// </summary>
		None = 0,
		/// <summary>
		/// Text in the area.
		/// </summary>
		Text,
		/// <summary>
		/// Label in the area. Text and label differ in that upon the text
		/// shape, the full foreground and background areas are written,
		/// while on the label, only foreground for the present data is written.
		/// </summary>
		Label,
		/// <summary>
		///	A box, with or without borders.
		/// </summary>
		Box,
		/// <summary>
		/// A dialog box with border, close icon, optional maximize icon,
		/// and optional button.
		/// </summary>
		Dialog,
		/// <summary>
		/// A custom shape using a series of character sequences.
		/// </summary>
		Custom
	}
	//*-------------------------------------------------------------------------*

}
