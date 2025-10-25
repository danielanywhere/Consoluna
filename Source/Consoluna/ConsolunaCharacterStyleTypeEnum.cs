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
	//*	ConsolunaCharacterStyleTypeEnum																					*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Enumeration of available character styles on Consoluna applications.
	/// </summary>
	[Flags]
	public enum ConsolunaCharacterStyleTypeEnum
	{
		/// <summary>
		/// No style specified or unknown.
		/// </summary>
		None =      0x0000,
		/// <summary>
		/// Normal style.
		/// </summary>
		Normal =    0x0001,
		/// <summary>
		/// Bold style.
		/// </summary>
		Bold =      0x0002,
		/// <summary>
		/// Faint or dim.
		/// </summary>
		Faint =     0x0004,
		/// <summary>
		/// Italic.
		/// </summary>
		Italic =    0x0008,
		/// <summary>
		/// Underline.
		/// </summary>
		Underline = 0x0010,
		/// <summary>
		/// Blinking.
		/// </summary>
		Blink =     0x0020,
		/// <summary>
		/// Inverse color.
		/// </summary>
		Inverse =   0x0040,
		/// <summary>
		/// Invisible or hidden.
		/// </summary>
		Hidden =    0x0080,
		/// <summary>
		/// Strikethrough.
		/// </summary>
		Strike =    0x0100
	}
	//*-------------------------------------------------------------------------*

}
