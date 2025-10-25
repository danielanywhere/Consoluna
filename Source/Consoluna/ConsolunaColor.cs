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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ConsolunaLib.Internal;

namespace ConsolunaLib
{
	//*-------------------------------------------------------------------------*
	//*	ConsolunaColor																													*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Color description for Consoluna applications.
	/// </summary>
	public class ConsolunaColor : ChangeObjectItem
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
		/// Create a new instance of the ConsolunaColor item.
		/// </summary>
		public ConsolunaColor()
		{
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the ConsolunaColor item.
		/// </summary>
		/// <param name="hexColor">
		/// Hex color to initialize.
		/// </param>
		public ConsolunaColor(string hexColor)
		{
			SetColor(this, hexColor);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Blue																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Blue">Blue</see>.
		/// </summary>
		private byte mBlue = 0x00;
		/// <summary>
		/// Get/Set the blue channel color.
		/// </summary>
		public byte Blue
		{
			get { return mBlue; }
			set
			{
				bool bChanged = (mBlue != value);
				mBlue = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Equals																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether this item's members are equal to the
		/// members of the caller's item.
		/// </summary>
		public override bool Equals(object o)
		{
			bool result = false;

			if(o is ConsolunaColor color)
			{
				if(color.mBlue == mBlue &&
					color.mGreen == mGreen &&
					color.mRed == mRed)
				{
					result = true;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetHashCode																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the unique hash code for this instance.
		/// </summary>
		public override int GetHashCode()
		{
			int factor = 0;
			int result = 2025102301;

			factor = 0 - (int)((double)result * 0.25);

			result *= (factor + mRed.GetHashCode());
			result *= (factor + mGreen.GetHashCode());
			result *= (factor + mBlue.GetHashCode());
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Green																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Green">Green</see>.
		/// </summary>
		private byte mGreen = 0x00;
		/// <summary>
		/// Get/Set the green channel color.
		/// </summary>
		public byte Green
		{
			get { return mGreen; }
			set
			{
				bool bChanged = (mGreen != value);
				mGreen = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Red																																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Red">Red</see>.
		/// </summary>
		private byte mRed = 0x00;
		/// <summary>
		/// Get/Set the red channel color.
		/// </summary>
		public byte Red
		{
			get { return mRed; }
			set
			{
				bool bChanged = (mRed != value);
				mRed = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	SetColor																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Set the color's channel values from the caller's HTML hex code.
		/// </summary>
		/// <param name="consoleColor">
		/// Reference to the object to update.
		/// </param>
		/// <param name="hexColor">
		/// Hex color string to parse.
		/// </param>
		public static void SetColor(ConsolunaColor consoleColor, string hexColor)
		{
			string text = "";

			if(consoleColor != null && hexColor?.Length > 6 &&
				hexColor.StartsWith("#"))
			{
				consoleColor.mRed = Convert.ToByte(hexColor.Substring(1, 2), 16);
				consoleColor.mGreen = Convert.ToByte(hexColor.Substring(3, 2), 16);
				consoleColor.mBlue = Convert.ToByte(hexColor.Substring(5, 2), 16);
				consoleColor.OnPropertyChanged("Color");
			}
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
