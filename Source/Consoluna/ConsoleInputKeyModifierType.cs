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
	//*	ConsoleInputKeyModifierType																							*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Enumeration of known key modifier types in ConsolePlus.
	/// </summary>
	public enum ConsoleInputKeyModifierType
	{
		/// <summary>
		/// No modifier specified or unknown.
		/// </summary>
		None  = 0x00,
		/// <summary>
		/// Shift key.
		/// </summary>
		Shift = 0x01,
		/// <summary>
		/// Alt key.
		/// </summary>
		Alt   = 0x02,
		/// <summary>
		/// Ctrl key.
		/// </summary>
		Ctrl  = 0x04
	}
	//*-------------------------------------------------------------------------*

}
