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

using ConsolunaLib.Internal;

namespace ConsolunaLib
{
	//*-------------------------------------------------------------------------*
	//*	SizeInfo																																*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Dimension object for use with Consoluna applications.
	/// </summary>
	public class SizeInfo : ChangeObjectItem
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
		/// Create a new instance of the SizeInfo item.
		/// </summary>
		public SizeInfo()
		{
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the SizeInfo item.
		/// </summary>
		/// <param name="width">
		/// Width dimension.
		/// </param>
		/// <param name="height">
		/// Height dimension.
		/// </param>
		public SizeInfo(int width, int height)
		{
			mWidth = width;
			mHeight = height;
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

			if(o is SizeInfo size)
			{
				if(size.mWidth == mWidth &&
					size.mHeight == mHeight)
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
			int result = 2025102303;

			factor = 0 - (int)((double)result * 0.25);

			result *= (factor + mWidth.GetHashCode());
			result *= (factor + mHeight.GetHashCode());
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
		/// Get/Set the vertical size.
		/// </summary>
		public int Height
		{
			get { return mHeight; }
			set
			{
				bool bChanged = (mHeight != value);
				mHeight = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* IsEmpty																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the specified size object is empty.
		/// </summary>
		/// <param name="size">
		/// Reference to the size object to inspect.
		/// </param>
		/// <returns>
		/// True if the object is null or empty. Otherwise, false.
		/// </returns>
		public static bool IsEmpty(SizeInfo size)
		{
			bool result = true;

			if(size != null &&
				(size.mHeight != 0 || size.mWidth != 0))
			{
				result = false;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* HasVolume																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the specified size object has a
		/// volume.
		/// </summary>
		/// <param name="size">
		/// Reference to the size object to inspect.
		/// </param>
		/// <returns>
		/// True if the object is has a volume. Otherwise, false.
		/// </returns>
		public static bool HasVolume(SizeInfo size)
		{
			bool result = false;

			if(size != null &&
				(size.mHeight != 0 && size.mWidth != 0))
			{
				result = true;
			}
			return result;
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
		/// Get/Set the horizontal dimension.
		/// </summary>
		public int Width
		{
			get { return mWidth; }
			set
			{
				bool bChanged = (mWidth != value);
				mWidth = value;
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
