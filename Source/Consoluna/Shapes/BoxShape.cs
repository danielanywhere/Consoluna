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
		////*-----------------------------------------------------------------------*
		////* BLC																																		*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Return the active box bottom left corner character.
		///// </summary>
		///// <returns>
		///// Box bottom left corner character.
		///// </returns>
		//private char BLC()
		//{
		//	char result = '\0';

		//	switch(mBorderStyle)
		//	{
		//		case BoxBorderStyle.Double:
		//			result = '╚';
		//			break;
		//		case BoxBorderStyle.Single:
		//			result = '└';
		//			break;
		//	}
		//	return result;
		//}
		////*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////* BRC																																		*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Return the active box bottom right corner character.
		///// </summary>
		///// <returns>
		///// Box bottom right corner character.
		///// </returns>
		//private char BRC()
		//{
		//	char result = '\0';
		//	switch(mBorderStyle)
		//	{
		//		case BoxBorderStyle.Double:
		//			result = '╝';
		//			break;
		//		case BoxBorderStyle.Single:
		//			result = '┘';
		//			break;
		//	}
		//	return result;
		//}
		////*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////* HLC																																		*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Return the active box horizontal line character.
		///// </summary>
		///// <returns>
		///// Box horizontal line character.
		///// </returns>
		//private char HLC()
		//{
		//	char result = '\0';
		//	switch(mBorderStyle)
		//	{
		//		case BoxBorderStyle.Double:
		//			result = '═';
		//			break;
		//		case BoxBorderStyle.Single:
		//			result = '─';
		//			break;
		//	}
		//	return result;
		//}
		////*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////* TLC																																		*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Return the active box top left corner character.
		///// </summary>
		///// <returns>
		///// Box top left corner character.
		///// </returns>
		//private char TLC()
		//{
		//	char result = '\0';
		//	switch(mBorderStyle)
		//	{
		//		case BoxBorderStyle.Double:
		//			result = '╔';
		//			break;
		//		case BoxBorderStyle.Single:
		//			result = '┌';
		//			break;
		//	}
		//	return result;
		//}
		////*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////* TRC																																		*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Return the active box top right corner character.
		///// </summary>
		///// <returns>
		///// Box top right corner character.
		///// </returns>
		//private char TRC()
		//{
		//	char result = '\0';
		//	switch(mBorderStyle)
		//	{
		//		case BoxBorderStyle.Double:
		//			result = '╗';
		//			break;
		//		case BoxBorderStyle.Single:
		//			result = '┐';
		//			break;
		//	}
		//	return result;
		//}
		////*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////* VLC																																		*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Return the active box vertical line character.
		///// </summary>
		///// <returns>
		///// Box vertical line character.
		///// </returns>
		//private char VLC()
		//{
		//	char result = '\0';
		//	switch(mBorderStyle)
		//	{
		//		case BoxBorderStyle.Double:
		//			result = '║';
		//			break;
		//		case BoxBorderStyle.Single:
		//			result = '│';
		//			break;
		//	}
		//	return result;
		//}
		////*-----------------------------------------------------------------------*

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
			ShapeType = ShapeType.Text;
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
			if(title?.Length > 0)
			{
				mTitle = title;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	BorderStyle																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="BorderStyle">BorderStyle</see>.
		/// </summary>
		private BoxBorderStyle mBorderStyle =
			BoxBorderStyle.Double;
		/// <summary>
		/// Get/Set the border style for the box.
		/// </summary>
		public BoxBorderStyle BorderStyle
		{
			get { return mBorderStyle; }
			set { mBorderStyle = value; }
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
			char charBLCorner = GetBoxBottomLeftCorner(mBorderStyle);
			char charBRCorner = GetBoxBottomRightCorner(mBorderStyle);
			char charHLine = GetBoxHorizontalLine(mBorderStyle);
			char charTLCorner = GetBoxTopLeftCorner(mBorderStyle);
			char charTRCorner = GetBoxTopRightCorner(mBorderStyle);
			char charVLine = GetBoxVerticalLine(mBorderStyle);
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
				mCharacterWindow[0, 0].Character = charTLCorner;
				for(colIndex = 1; colIndex < colEnd; colIndex++)
				{
					mCharacterWindow[colIndex, 0].Character = charHLine;
				}
				mCharacterWindow[colIndex, 0].Character = charTRCorner;
				for(rowIndex = 1; rowIndex < rowEnd; rowIndex++)
				{
					mCharacterWindow[0, rowIndex].Character = charVLine;
					mCharacterWindow[colEnd, rowIndex].Character = charVLine;
				}
				mCharacterWindow[0, rowIndex].Character = charBLCorner;
				for(colIndex = 1; colIndex < colEnd; colIndex++)
				{
					mCharacterWindow[colIndex, rowIndex].Character = charHLine;
				}
				mCharacterWindow[colIndex, rowIndex].Character = charBRCorner;

				if(title.Length > 0)
				{
					colIndex = 1;
					chars = title.ToCharArray();
					foreach(char charItem in chars)
					{
						mCharacterWindow[colIndex, 0].Character = GetPrintable(charItem);
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
