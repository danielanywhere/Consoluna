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

using static ConsolunaLib.ConsolunaUtil;

namespace ConsolunaLib.Shapes
{
	//*-------------------------------------------------------------------------*
	//*	ScrollBarShape																													*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// A scroll bar control shape.
	/// </summary>
	public class ScrollBarShape : ShapeBase
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
			StyleName = "ScrollBarColor";
			ShapeType = ShapeType.ScrollBar;
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
		/// Create a new instance of the ScrollBarShape item.
		/// </summary>
		public ScrollBarShape()
		{
			CommonInit();
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the ScrollBarShape item.
		/// </summary>
		/// <param name="name">
		/// </param>
		/// <param name="x">
		/// Horizontal position.
		/// </param>
		/// <param name="y">
		/// Vertical position.
		/// </param>
		/// <param name="length">
		/// The length of the shape, in characters.
		/// </param>
		/// <param name="value">
		/// The initial value of the bar.
		/// </param>
		/// <param name="orientation">
		/// Cartesian orientation of the shape.
		/// </param>
		public ScrollBarShape(string name, int x = 0, int y = 0, int length = 1,
			CartesianOrientation orientation = CartesianOrientation.Vertical)
		{
			CommonInit();
			if(name?.Length > 0)
			{
				Name = name;
			}
			Position.X = x;
			Position.Y = y;
			mLength = length;
			mOrientation = orientation;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Length																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Length">Length</see>.
		/// </summary>
		private int mLength = 0;
		/// <summary>
		/// Get/Set the length of the shape, in characters.
		/// </summary>
		public int Length
		{
			get { return mLength; }
			set
			{
				bool bChanged = (mLength != value);
				mLength = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	MaxValue																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="MaxValue">MaxValue</see>.
		/// </summary>
		private int mMaxValue = 100;
		/// <summary>
		/// Get/Set the maximum value on this shape. Default = 100.
		/// </summary>
		public int MaxValue
		{
			get { return mMaxValue; }
			set
			{
				bool bChanged = (mMaxValue != value);
				mMaxValue = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	MinValue																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="MinValue">MinValue</see>.
		/// </summary>
		private int mMinValue = 0;
		/// <summary>
		/// Get/Set the minimum value on this shape. Default = 0,
		/// </summary>
		public int MinValue
		{
			get { return mMinValue; }
			set
			{
				bool bChanged = (mMinValue != value);
				mMinValue = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Orientation																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Orientation">Orientation</see>.
		/// </summary>
		private CartesianOrientation mOrientation = CartesianOrientation.Vertical;
		/// <summary>
		/// Get/Set the orientation of the shape on the display.
		/// </summary>
		public CartesianOrientation Orientation
		{
			get { return mOrientation; }
			set
			{
				bool bChanged = (mOrientation != value);
				mOrientation = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Render																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Render the contents of this control.
		/// </summary>
		/// <param name="screenBuffer">
		/// Reference to the Consoluna session upon which this shape is being
		/// rendered.
		/// </param>
		public override void Render(Consoluna screenBuffer)
		{
			bool bEventsActive = EventsActive;
			char charBuffer = DrawingSymbols.GetSymbolValue("ScrollBuffer");
			char charDArrow = DrawingSymbols.GetSymbolValue("ArrowheadDown");
			char charLArrow = DrawingSymbols.GetSymbolValue("ArrowheadLeft");
			char charPositioner = DrawingSymbols.GetSymbolValue("ScrollPositioner");
			char charRArrow = DrawingSymbols.GetSymbolValue("ArrowheadRight");
			char charUArrow = DrawingSymbols.GetSymbolValue("ArrowheadUp");
			int count = 0;
			int index = 0;
			int position = 0;

			EventsActive = false;
			switch(mOrientation)
			{
				case CartesianOrientation.Horizontal:
					if(mLength > 0)
					{
						Size.Width = mLength;
						Size.Height = 1;
					}
					else
					{
						Size.Width = 0;
						Size.Height = 0;
					}
					break;
				case CartesianOrientation.None:
				case CartesianOrientation.Vertical:
					if(mLength > 0)
					{
						Size.Width = 1;
						Size.Height = mLength;
					}
					else
					{
						Size.Width = 0;
						Size.Height = 0;
					}
					break;
			}
			EventsActive = bEventsActive;

			base.Render(screenBuffer);

			screenBuffer.ClearCharacterWindow(mCharacterWindow,
				ForeColor, BackColor);

			if(Visible && screenBuffer != null)
			{
				if(mLength > 0)
				{
					switch(mOrientation)
					{
						case CartesianOrientation.Horizontal:
							switch(mLength)
							{
								case 1:
									//	First arrow only at X, Y.
									mCharacterWindow[0, 0].Symbol = charLArrow;
									break;
								case 2:
									//	Both arrows only.
									mCharacterWindow[0, 0].Symbol = charLArrow;
									mCharacterWindow[1, 0].Symbol = charRArrow;
									break;
								case 3:
									//	Both arrows and positioner only.
									mCharacterWindow[0, 0].Symbol = charLArrow;
									mCharacterWindow[1, 0].Symbol = charPositioner;
									mCharacterWindow[2, 0].Symbol = charRArrow;
									break;
								default:
									//	Both arrows, positioner and at least one space.
									count = mLength - 2;
									position = (int)ConvertRange(mValue,
										mMinValue, mMaxValue, 1f, count);
									mCharacterWindow[0, 0].Symbol = charLArrow;
									for(index = 1; index <= count; index ++)
									{
										if(index == position)
										{
											mCharacterWindow[index, 0].Symbol = charPositioner;
										}
										else
										{
											mCharacterWindow[index, 0].Symbol = charBuffer;
										}
									}
									mCharacterWindow[index, 0].Symbol = charRArrow;
									break;
							}
							break;
						case CartesianOrientation.None:
						case CartesianOrientation.Vertical:
							switch(mLength)
							{
								case 1:
									//	First arrow only at X, Y.
									mCharacterWindow[0, 0].Symbol = charUArrow;
									break;
								case 2:
									//	Both arrows only.
									mCharacterWindow[0, 0].Symbol = charUArrow;
									mCharacterWindow[0, 1].Symbol = charDArrow;
									break;
								case 3:
									//	Both arrows and positioner only.
									mCharacterWindow[0, 0].Symbol = charUArrow;
									mCharacterWindow[0, 1].Symbol = charPositioner;
									mCharacterWindow[0, 2].Symbol = charDArrow;
									break;
								default:
									//	Both arrows, positioner and at least one space.
									count = mLength - 2;
									position = (int)ConvertRange(mValue,
										mMinValue, mMaxValue, 1f, count);
									mCharacterWindow[0, 0].Symbol = charUArrow;
									for(index = 1; index <= count; index++)
									{
										if(index == position)
										{
											mCharacterWindow[0, index].Symbol = charPositioner;
										}
										else
										{
											mCharacterWindow[0, index].Symbol = charBuffer;
										}
									}
									mCharacterWindow[0, index].Symbol = charDArrow;
									break;
							}
							break;
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Value																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Value">Value</see>.
		/// </summary>
		private int mValue = 0;
		/// <summary>
		/// Get/Set the current value on this shape.
		/// </summary>
		public int Value
		{
			get { return mValue; }
			set
			{
				bool bChanged = (mValue != value);
				mValue = value;
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
