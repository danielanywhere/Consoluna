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
	//*	BoxShape																																*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Information about a general box shape.
	/// </summary>
	public class BoxShape : ShapeBase
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* CommonInit																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Common initialization.
		/// </summary>
		private void CommonInit()
		{
			StyleName = "BoxColor";
			ShapeType = ShapeType.Box;
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
		/// Create a new instance of the BoxShape item.
		/// </summary>
		public BoxShape()
		{
			CommonInit();
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the BoxShape item.
		/// </summary>
		/// <param name="name">
		/// Unique name of the shape.
		/// </param>
		/// <param name="title">
		/// Text for a title legend.
		/// </param>
		/// <param name="x">
		/// The horizontal starting position of the shape.
		/// </param>
		/// <param name="y">
		/// The vertical starting position of the shape.
		/// </param>
		/// <param name="width">
		/// The width of the shape, in characters.
		/// </param>
		/// <param name="height">
		/// The height of the shape, in characters.
		/// </param>
		public BoxShape(string name, string title = "",
			int x = 0, int y = 0,
			int width = 1, int height = 1) :
			base(name, x, y, width, height)
		{
			CommonInit();
			if(title?.Length > 0)
			{
				mTitle = title;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	BoxBorderStyle																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="BorderStyle">BorderStyle</see>.
		/// </summary>
		private BoxBorderStyle mBoxBorderStyle =
			BoxBorderStyle.Double;
		/// <summary>
		/// Get/Set the border style for the box.
		/// </summary>
		public BoxBorderStyle BorderStyle
		{
			get { return mBoxBorderStyle; }
			set { mBoxBorderStyle = value; }
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
			StringBuilder builder = new StringBuilder();
			char charBLCorner = DrawingSymbols.GetSymbolValue(
				$"BoxBottomLeftCorner{mBoxBorderStyle}");
			char charBRCorner = DrawingSymbols.GetSymbolValue(
				$"BoxBottomRightCorner{mBoxBorderStyle}");
			char charHLine = DrawingSymbols.GetSymbolValue(
				$"BoxHorizontalLine{mBoxBorderStyle}");
			char charTLCorner = DrawingSymbols.GetSymbolValue(
				$"BoxTopLeftCorner{mBoxBorderStyle}");
			char charTRCorner = DrawingSymbols.GetSymbolValue(
				$"BoxTopRightCorner{mBoxBorderStyle}");
			char charVLine = DrawingSymbols.GetSymbolValue(
				$"BoxVerticalLine{mBoxBorderStyle}");
			char[] chars = null;
			int colCount = 0;
			int colEnd = 0;
			int colIndex = 0;
			int rowCount = 0;
			int rowEnd = 0;
			int rowIndex = 0;
			string title = "";

			base.Render(screenBuffer);
			if(Visible && mCharacterWindow.GetLength(0) > 0)
			{
				//	If the character window is full, the base values are good.
				colCount = Size.Width;
				rowCount = Size.Height;
				if(mTitle?.Length > 0)
				{
					title = GetPrintable(mTitle);
					if(title.Length > colCount - 2)
					{
						title = title.Substring(0, colCount - 2);
					}
				}

				//	Draw the actual box.
				screenBuffer.ClearCharacterWindow(mCharacterWindow,
					ForeColor, BackColor);
				colEnd = colCount - 1;
				rowEnd = rowCount - 1;
				mCharacterWindow[0, 0].Symbol = charTLCorner;
				for(colIndex = 1; colIndex < colEnd; colIndex++)
				{
					mCharacterWindow[colIndex, 0].Symbol = charHLine;
				}
				mCharacterWindow[colIndex, 0].Symbol = charTRCorner;
				for(rowIndex = 1; rowIndex < rowEnd; rowIndex++)
				{
					mCharacterWindow[0, rowIndex].Symbol = charVLine;
					mCharacterWindow[colEnd, rowIndex].Symbol = charVLine;
				}
				mCharacterWindow[0, rowIndex].Symbol = charBLCorner;
				for(colIndex = 1; colIndex < colEnd; colIndex++)
				{
					mCharacterWindow[colIndex, rowIndex].Symbol = charHLine;
				}
				mCharacterWindow[colIndex, rowIndex].Symbol = charBRCorner;

				if(title.Length > 0)
				{
					colIndex = 1;
					chars = title.ToCharArray();
					foreach(char charItem in chars)
					{
						mCharacterWindow[colIndex, 0].Symbol = GetPrintable(charItem);
						colIndex++;
						if(colIndex >= colEnd)
						{
							break;
						}
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Title																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Title">Title</see>.
		/// </summary>
		private string mTitle = "";
		/// <summary>
		/// Get/Set the display title of this control.
		/// </summary>
		public string Title
		{
			get { return mTitle; }
			set
			{
				bool bChanged = mTitle != value;
				if(value == null)
				{
					mTitle = "";
				}
				else
				{
					mTitle = value;
				}
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
