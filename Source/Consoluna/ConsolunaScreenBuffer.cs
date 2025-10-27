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
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using static ConsolunaLib.ConsolunaUtil;

namespace ConsolunaLib
{
	//*-------------------------------------------------------------------------*
	//*	ConsolunaScreenBuffer																										*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Screen buffer and related configuration.
	/// </summary>
	public class ConsolunaScreenBuffer
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
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
		/// Create a new instance of the ConsolunaScreenBuffer item.
		/// </summary>
		public ConsolunaScreenBuffer()
		{
			//	Set the default styles.
			mStyles = new ConsolunaScreenStyleCollection();
			mStyles.AddRange(new ConsolunaScreenStyleItem[]
			{
				new ConsolunaScreenStyleItem("ScreenColor",
					foreColor: new ConsolunaColor("#b0aedd"),
					backColor: new ConsolunaColor("#0b0938")),

				new ConsolunaScreenStyleItem("ButtonColor",
					foreColor: new ConsolunaColor("#003300"),
					backColor: new ConsolunaColor("#01af00")),
				new ConsolunaScreenStyleItem("ButtonColorDefault",
					foreColor: new ConsolunaColor("#5cffb1"),
					backColor: new ConsolunaColor("#01af00")),
				new ConsolunaScreenStyleItem("ButtonShortcutColor",
					foreColor: new ConsolunaColor("#333300"),
					backColor: new ConsolunaColor("#01af00")),

				new ConsolunaScreenStyleItem("DialogColor",
					foreColor: new ConsolunaColor("#010101"),
					backColor: new ConsolunaColor("#b4b3b1")),
				new ConsolunaScreenStyleItem("DialogBorderColor",
					foreColor: new ConsolunaColor("#fffeff"),
					backColor: new ConsolunaColor("#b4b3b1")),
				new ConsolunaScreenStyleItem("DialogListBoxColor",
					foreColor: new ConsolunaColor("#002427"),
					backColor: new ConsolunaColor("#03b1ba")),
				new ConsolunaScreenStyleItem("DialogListBoxHighlightColor",
					foreColor: new ConsolunaColor("#eeffe1"),
					backColor: new ConsolunaColor("#01ae04")),
				new ConsolunaScreenStyleItem("DialogTextBoxColor",
					foreColor: new ConsolunaColor("#f2fcff"),
					backColor: new ConsolunaColor("#0001ab")),

				new ConsolunaScreenStyleItem("MenuColor",
					foreColor: new ConsolunaColor("#0b0708"),
					backColor: new ConsolunaColor("#b4b3b1")),
				new ConsolunaScreenStyleItem("MenuHighlightColor",
					backColor: new ConsolunaColor("#01af00")),
				new ConsolunaScreenStyleItem("MenuPanelBorderColor",
					foreColor: new ConsolunaColor("#010101"),
					backColor: new ConsolunaColor("#b4b3b1")),
				new ConsolunaScreenStyleItem("MenuShortcutColor",
					foreColor: new ConsolunaColor("#912120")),

				new ConsolunaScreenStyleItem("ShortcutColor",
					foreColor: new ConsolunaColor("#912120")),

				new ConsolunaScreenStyleItem("TextColor",
					foreColor: new ConsolunaColor("#f2fcff"),
					backColor: new ConsolunaColor("#0001ab")),

				new ConsolunaScreenStyleItem("WindowColor",
					foreColor: new ConsolunaColor("#002593"),
					backColor: new ConsolunaColor("#04b0b0")),
				new ConsolunaScreenStyleItem("WindowBorderColor",
					foreColor: new ConsolunaColor("#b2ffff"),
					backColor: new ConsolunaColor("#04b0b0")),
				new ConsolunaScreenStyleItem("WindowListBoxColor",
					foreColor: new ConsolunaColor("#bfffff"),
					backColor: new ConsolunaColor("#0300ac")),
				new ConsolunaScreenStyleItem("WindowListBoxHighlightColor",
					foreColor: new ConsolunaColor("#ffc3a1"),
					backColor: new ConsolunaColor("#ad0100"))

			});

		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Characters																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Characters">Characters</see>.
		/// </summary>
		private ConsolunaCharacterCollection mCharacters =
			new ConsolunaCharacterCollection();
		/// <summary>
		/// Get a reference to the collection of characters in the buffer.
		/// </summary>
		public ConsolunaCharacterCollection Characters
		{
			get { return mCharacters; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetCharactersInArea																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return an array of characters from the indicated cartesian area of the
		/// screen buffer.
		/// </summary>
		/// <param name="columnIndex">
		/// Starting column index.
		/// </param>
		/// <param name="rowIndex">
		/// Starting row index.
		/// </param>
		/// <param name="width">
		/// Width of area, in characters.
		/// </param>
		/// <param name="height">
		/// Height of area, in characters.
		/// </param>
		/// <returns>
		/// Reference to a 2-dimensional, 0-based array of characters found in
		/// the specified source area of the screen buffer. Any characters outside
		/// the boundaries of the logical screen buffer space are filled with
		/// dummy elements. If either width or height are less than 1, an empty
		/// array is returned.
		/// </returns>
		/// <remarks>
		/// Column and row indices should have legal values prior to calling this
		/// method. Any indicators that fall outside the logical bounds of the
		/// buffer's cartesian space will be filled with dummy elements.
		/// </remarks>
		public ConsolunaCharacterItem[,] GetCharactersInArea(int column,
			int row, int width, int height)
		{
			int bufferIndex = 0;
			int colIndex = 0;
			ConsolunaCharacterItem[,] result = null;
			int rowIndex = 0;
			int xIndex = 0;
			int xEnd = 0;
			int xStart = column;
			int yIndex = 0;
			int yEnd = 0;
			int yStart = row;

			if(width > 0 && height > 0)
			{
				xEnd = xStart + width;
				yEnd = yStart + height;
				result = new ConsolunaCharacterItem[width, height];
				for(yIndex = yStart, rowIndex = 0;
					yIndex < yEnd;
					yIndex ++, rowIndex ++)
				{
					for(xIndex = xStart, colIndex = 0;
						xIndex < xEnd;
						xIndex ++, colIndex ++)
					{
						if(xIndex >= 0 && xIndex < mWidth &&
							yIndex >= 0 && yIndex < mHeight)
						{
							bufferIndex = GetBufferIndex(mWidth, mHeight, xIndex, yIndex);
							result[colIndex, rowIndex] = mCharacters[bufferIndex];
						}
						else
						{
							result[colIndex, rowIndex] = new ConsolunaCharacterItem();
						}
					}
				}
			}

			if(result == null)
			{
				result = new ConsolunaCharacterItem[0, 0];
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Height																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Height">Height</see>.
		/// </summary>
		private int mHeight = 0;
		/// <summary>
		/// Get/Set the logical height of the buffer.
		/// </summary>
		public int Height
		{
			get { return mHeight; }
			set { mHeight = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Styles																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Styles">Styles</see>.
		/// </summary>
		private ConsolunaScreenStyleCollection mStyles = null;
		/// <summary>
		/// Get a reference to the collection of styles associated with this screen
		/// buffer.
		/// </summary>
		public ConsolunaScreenStyleCollection Styles
		{
			get { return mStyles; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Width																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Width">Width</see>.
		/// </summary>
		private int mWidth = 0;
		/// <summary>
		/// Get/Set the logical width of the buffer.
		/// </summary>
		public int Width
		{
			get { return mWidth; }
			set { mWidth = value; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*


}
