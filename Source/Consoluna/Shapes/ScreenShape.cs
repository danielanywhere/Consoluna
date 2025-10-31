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
using System.Text.RegularExpressions;

using static ConsolunaLib.ConsolunaUtil;

namespace ConsolunaLib.Shapes
{
	//*-------------------------------------------------------------------------*
	//*	ScreenShape																															*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Information about a backing screen view.
	/// </summary>
	public class ScreenShape : ShapeBase
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* CommonInit																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Common initialization for this object.
		/// </summary>
		private void CommonInit()
		{
			StyleName = "ScreenColor";
			ShapeType = ShapeType.Screen;
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	_Constructor																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a new instance of the ScreenShape item.
		/// </summary>
		public ScreenShape(int width, int height)
		{
			CommonInit();
			Size.Width = width;
			Size.Height = height;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the ScreenShape item.
		/// </summary>
		/// <param name="name">
		/// </param>
		/// <param name="x">
		/// </param>
		/// <param name="y">
		/// </param>
		/// <param name="width">
		/// </param>
		/// <param name="height">
		/// </param>
		public ScreenShape(string name, int x = 0, int y = 0, int width = 0,
			int height = 0) : base(name, x, y, width, height)
		{
			CommonInit();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Render																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Render the character output of this shape to the screen buffer.
		/// </summary>
		/// <param name="screenBuffer">
		/// Reference to the screen buffer to which the contents will be written.
		/// </param>
		public override void Render(Consoluna screenBuffer)
		{
			char charBackground = DrawingSymbols.GetSymbolValue("ScreenBackground");

			base.Render(screenBuffer);
			if(Visible && mCharacterWindow.GetLength(0) > 0)
			{
				screenBuffer.ClearCharacterWindow(mCharacterWindow,
					ForeColor, BackColor, charBackground);
			}
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
