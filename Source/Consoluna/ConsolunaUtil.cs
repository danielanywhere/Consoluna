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

namespace ConsolunaLib
{
	//*-------------------------------------------------------------------------*
	//*	ConsolunaUtil																														*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Utility functionality for Consoluna applications.
	/// </summary>
	public class ConsolunaUtil
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
		//* GetPrintable																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the printable version of this character.
		/// </summary>
		/// <param name="character">
		/// Reference to the character for which to retrieve the printable code.
		/// </param>
		/// <returns>
		/// The printable version of the character.
		/// </returns>
		public static char GetPrintable(char character)
		{
			char result = ' ';
			int value = 0;

			value = ((int)character) & 0xff;
			if(value == 9 || value == 10 || value == 13 ||
				(value > 31 && value < 127))
			{
				result = (char)value;
			}
			return result;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the printable version of this character.
		/// </summary>
		/// <param name="character">
		/// Reference to the console character item for which to retrieve the
		/// printable code.
		/// </param>
		/// <returns>
		/// The printable version of the item's Character property.
		/// </returns>
		public static char GetPrintable(ConsolunaCharacterItem character)
		{
			char result = ' ';
			int value = 0;

			if(character != null)
			{
				value = ((int)character.Character) & 0xff;
				if(value == 9 || value == 10 || value == 13 ||
					(value > 31 && value < 127))
				{
					result = (char)value;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*

}
