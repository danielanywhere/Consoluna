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
	//*	ConsolunaPosition																												*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Position object for use with Consoluna applications.
	/// </summary>
	public class ConsolunaPosition : ChangeObjectItem
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
		/// Create a new instance of the ConsolunaPosition item.
		/// </summary>
		public ConsolunaPosition()
		{
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the ConsolunaPosition item.
		/// </summary>
		/// <param name="position">
		/// Reference to a position whose member values will be copied.
		/// </param>
		public ConsolunaPosition(ConsolunaPosition position)
		{
			if(position != null)
			{
				mX = position.X;
				mY = position.Y;
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the ConsolunaPosition item.
		/// </summary>
		/// <param name="x">
		/// The initial X value of the new position.
		/// </param>
		/// <param name="y">
		/// The initial Y value of the new position.
		/// </param>
		public ConsolunaPosition(int x, int y)
		{
			mX = x;
			mY = y;
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

			if(o is ConsolunaPosition position)
			{
				if(position.mX == mX &&
					position.mY == mY)
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
			int result = 2025102302;

			factor = 0 - (int)((double)result * 0.25);

			result *= (factor + mX.GetHashCode());
			result *= (factor + mY.GetHashCode());
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* IsEmpty																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the specified position object is
		/// empty.
		/// </summary>
		/// <param name="size">
		/// Reference to the position object to inspect.
		/// </param>
		/// <returns>
		/// True if the object is null or empty. Otherwise, false.
		/// </returns>
		public static bool IsEmpty(ConsolunaPosition position)
		{
			bool result = true;

			if(position != null &&
				(position.mX != 0 || position.mY != 0))
			{
				result = false;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* TransferValues																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Transfer values from the source object to the target.
		/// </summary>
		/// <param name="source">
		/// Reference to the source object to be copied.
		/// </param>
		/// <param name="target">
		/// Reference to the target object receiving the values.
		/// </param>
		public static void TransferValues(ConsolunaPosition source,
			ConsolunaPosition target)
		{
			if(source != null && target != null)
			{
				target.mX = source.mX;
				target.mY = source.mY;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	X																																			*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="X">X</see>.
		/// </summary>
		private int mX = 0;
		/// <summary>
		/// Get/Set the horizontal location.
		/// </summary>
		public int X
		{
			get { return mX; }
			set
			{
				bool bChanged = (mX != value);
				mX = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Y																																			*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Y">Y</see>.
		/// </summary>
		private int mY = 0;
		/// <summary>
		/// Get/Set the vertical location.
		/// </summary>
		public int Y
		{
			get { return mY; }
			set
			{
				bool bChanged = (mY != value);
				mY = value;
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
