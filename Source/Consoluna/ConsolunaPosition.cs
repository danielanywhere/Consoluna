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
