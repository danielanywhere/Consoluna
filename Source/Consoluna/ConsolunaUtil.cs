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
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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
		//* AfterShadowEquals																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the two colors are equal
		/// </summary>
		/// <param name="character"></param>
		/// <param name="colorName"></param>
		/// <param name="comparison"></param>
		/// <returns></returns>
		public static bool AfterShadowEquals(CharacterItem character,
			string colorName, ColorInfo comparison)
		{
			ColorInfo currentColor = null;
			bool result = false;

			if(character != null && colorName?.Length > 0)
			{
				switch(colorName)
				{
					case "BackColor":
						currentColor = character.BackColor;
						break;
					case "ForeColor":
						currentColor = character.ForeColor;
						break;
				}
				//currentColor = (ColorInfo)GetPropertyByName(character, colorName);
				if(currentColor != null)
				{
					if(character.Shadowed)
					{
						(float hue, float saturation, float lightness) =
							ColorInfo.ToHsl(currentColor);
						lightness /= 2f;
						currentColor = new ColorInfo(
							ColorInfo.FromHsl((hue, saturation, lightness)));
					}
					result = currentColor.Equals(comparison);
				}
				else
				{
					//	Compare only against null.
					result = (currentColor == comparison);
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ApplyShadowColor																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the specified color of the provided character after the
		/// shadow has been applied.
		/// </summary>
		/// <param name="baseColor">
		/// Reference to the base color to use.
		/// </param>
		/// <param name="shadowed">
		/// Value indicating whether the color is to be shaded.
		/// </param>
		/// <returns>
		/// Reference to the provided base color, if the character was
		/// not shaded. Otherwise, a reference to a shaded version of the
		/// base color.
		/// </returns>
		public static ColorInfo ApplyShadowColor(ColorInfo baseColor,
			bool shadowed)
		{
			ColorInfo result = baseColor;

			if(baseColor != null && shadowed)
			{
				(float hue, float saturation, float lightness) =
					ColorInfo.ToHsl(baseColor);
				lightness /= 2f;
				result = new ColorInfo(
					ColorInfo.FromHsl((hue, saturation, lightness)));
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ClampByte																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Clamp a value to a integer byte.
		/// </summary>
		/// <param name="value">
		/// The raw value to clamp.
		/// </param>
		/// <returns>
		/// Valid byte value between 0 and 255.
		/// </returns>
		public static int ClampByte(int value)
		{
			return Math.Max(0, Math.Min(255, value));
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Clear																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Clear the contents of the caller's string builder.
		/// </summary>
		/// <param name="builder">
		/// Reference to the builder to clear.
		/// </param>
		public static void Clear(StringBuilder builder)
		{
			if(builder?.Length > 0)
			{
				builder.Remove(0, builder.Length);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ConvertRange																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Convert from one numeric range to another.
		/// </summary>
		/// <param name="value">
		/// The value to convert.
		/// </param>
		/// <param name="fromMin">
		/// Original range minimum limit.
		/// </param>
		/// <param name="fromMax">
		/// Original range maximum limit.
		/// </param>
		/// <param name="toMin">
		/// New range minimum limit.
		/// </param>
		/// <param name="toMax">
		/// New range maximum limit.
		/// </param>
		/// <returns>
		/// Specified value, converted to the new range.
		/// </returns>
		public static float ConvertRange(float value,
			float fromMin, float fromMax, float toMin, float toMax)
		{
			float fromRange = (fromMax - fromMin);
			float result = 0;
			float toRange = 0f;

			if(fromRange == 0f)
			{
				result = toMin;
			}
			else
			{
				toRange = (toMax - toMin);
				result = (((value - fromMin) * toRange) / fromRange) + toMin;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* EnsureLegal																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Ensure that the caller's position has been constrained to a legal
		/// coordinate within the provided buffer area.
		/// </summary>
		/// <param name="bufferWidth">
		/// Logical width of the buffer.
		/// </param>
		/// <param name="bufferHeight">
		/// Logical height of the buffer.
		/// </param>
		/// <param name="position">
		/// Reference to the position to constrain.
		/// </param>
		public static void EnsureLegal(int bufferWidth,
			int bufferHeight, PositionInfo position)
		{
			int index = 0;

			if(bufferWidth > 0 && bufferHeight > 0 && position != null)
			{
				index =
					GetBufferIndex(bufferWidth, bufferHeight, position.X, position.Y);
				position.X = index % bufferWidth;
				position.Y = index / bufferWidth;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetArrowheadDown																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the down arrowhead character.
		/// </summary>
		/// <returns>
		/// A unicode down arrowhead character.
		/// </returns>
		public static char GetArrowheadDown()
		{
			return '▼';
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetArrowheadLeft																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the left arrowhead character.
		/// </summary>
		/// <returns>
		/// A unicode left arrowhead character.
		/// </returns>
		public static char GetArrowheadLeft()
		{
			return '◄';
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetArrowheadRight																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the right arrowhead character.
		/// </summary>
		/// <returns>
		/// A unicode right arrowhead character.
		/// </returns>
		public static char GetArrowheadRight()
		{
			return '►';
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetArrowheadUp																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the up arrowhead character.
		/// </summary>
		/// <returns>
		/// A unicode up arrowhead character.
		/// </returns>
		public static char GetArrowheadUp()
		{
			return '▲';
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetBoxBottomLeftCorner																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the box bottom left corner character in the specified style.
		/// </summary>
		/// <param name="borderStyle">
		/// The border style of the character to retrieve.
		/// </param>
		/// <returns>
		/// A unicode box-drawing character for the bottom left corner of a box.
		/// </returns>
		public static char GetBoxBottomLeftCorner(BoxBorderStyle borderStyle)
		{
			char result = '\0';

			switch(borderStyle)
			{
				case BoxBorderStyle.Double:
					result = '╚';
					break;
				case BoxBorderStyle.Single:
					result = '└';
					break;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetBoxBottomRightCorner																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the box bottom right corner character in the specified style.
		/// </summary>
		/// <param name="borderStyle">
		/// The border style of the character to retrieve.
		/// </param>
		/// <returns>
		/// A unicode box-drawing character for the bottom right corner of a box.
		/// </returns>
		public static char GetBoxBottomRightCorner(BoxBorderStyle borderStyle)
		{
			char result = '\0';
			switch(borderStyle)
			{
				case BoxBorderStyle.Double:
					result = '╝';
					break;
				case BoxBorderStyle.Single:
					result = '┘';
					break;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetBoxHorizontalLine																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the box horizontal line character in the specified style.
		/// </summary>
		/// <param name="borderStyle">
		/// The border style of the character to retrieve.
		/// </param>
		/// <returns>
		/// A unicode box-drawing character for the horizontal line of a box.
		/// </returns>
		public static char GetBoxHorizontalLine(BoxBorderStyle borderStyle)
		{
			char result = '\0';
			switch(borderStyle)
			{
				case BoxBorderStyle.Double:
					result = '═';
					break;
				case BoxBorderStyle.Single:
					result = '─';
					break;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetBoxTopLeftCorner																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the box top left corner character in the specified style.
		/// </summary>
		/// <param name="borderStyle">
		/// The border style of the character to retrieve.
		/// </param>
		/// <returns>
		/// A unicode box-drawing character for the top left corner of a box.
		/// </returns>
		public static char GetBoxTopLeftCorner(BoxBorderStyle borderStyle)
		{
			char result = '\0';
			switch(borderStyle)
			{
				case BoxBorderStyle.Double:
					result = '╔';
					break;
				case BoxBorderStyle.Single:
					result = '┌';
					break;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetBoxTopRightCorner																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the box top right corner character in the specified style.
		/// </summary>
		/// <param name="borderStyle">
		/// The border style of the character to retrieve.
		/// </param>
		/// <returns>
		/// A unicode box-drawing character for the top right corner of a box.
		/// </returns>
		public static char GetBoxTopRightCorner(BoxBorderStyle borderStyle)
		{
			char result = '\0';
			switch(borderStyle)
			{
				case BoxBorderStyle.Double:
					result = '╗';
					break;
				case BoxBorderStyle.Single:
					result = '┐';
					break;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetBoxVerticalLine																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the box vertical line character in the specified style.
		/// </summary>
		/// <param name="borderStyle">
		/// The border style of the character to retrieve.
		/// </param>
		/// <returns>
		/// A unicode box-drawing character for the vertical line of a box.
		/// </returns>
		public static char GetBoxVerticalLine(BoxBorderStyle borderStyle)
		{
			char result = '\0';
			switch(borderStyle)
			{
				case BoxBorderStyle.Double:
					result = '║';
					break;
				case BoxBorderStyle.Single:
					result = '│';
					break;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetBufferIndex																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the linear index of the buffer, given the buffer's dimensions
		/// and the cartesian coordinate within.
		/// </summary>
		/// <param name="bufferWidth">
		/// Logical width of the buffer.
		/// </param>
		/// <param name="bufferHeight">
		/// Logical height of the buffer.
		/// </param>
		/// <param name="columnIndex">
		/// Cartesian column index to retrieve.
		/// </param>
		/// <param name="rowIndex">
		/// Cartesian row index to retrieve.
		/// </param>
		/// <returns>
		/// The linear array index of the specified coordinate within the buffer.
		/// </returns>
		public static int GetBufferIndex(int bufferWidth, int bufferHeight,
			int columnIndex, int rowIndex)
		{
			int result =
				(rowIndex * bufferWidth + columnIndex) % (bufferWidth * bufferHeight);

			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetIndex																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the index of the specified group member in the provided match.
		/// </summary>
		/// <param name="match">
		/// Reference to the match to be inspected.
		/// </param>
		/// <param name="groupName">
		/// Name of the group for which the index will be found.
		/// </param>
		/// <returns>
		/// The starting index of the specified group, if found. Otherwise, -1.
		/// </returns>
		public static int GetIndex(Match match, string groupName)
		{
			int result = -1;

			if(match != null && match.Groups[groupName] != null &&
				match.Groups[groupName].Value != null)
			{
				result = match.Groups[groupName].Index;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetLegalPosition																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a position that has been constrained to a legal coordinate
		/// within the provided buffer area.
		/// </summary>
		/// <param name="bufferWidth">
		/// Logical width of the buffer.
		/// </param>
		/// <param name="bufferHeight">
		/// Logical height of the buffer.
		/// </param>
		/// <param name="columnIndex">
		/// Raw column index to process.
		/// </param>
		/// <param name="rowIndex">
		/// Raw row index to process.
		/// </param>
		/// <returns>
		/// Reference to a cartesian coordinate representing a legal position
		/// within the buffer array, if valid. Otherwise, null.
		/// </returns>
		public static PositionInfo GetLegalPosition(int bufferWidth,
			int bufferHeight, int columnIndex, int rowIndex)
		{
			int index = 0;
			PositionInfo result = null;

			if(bufferWidth > 0 && bufferHeight > 0)
			{
				result = new PositionInfo();
				index =
					GetBufferIndex(bufferWidth, bufferHeight, columnIndex, rowIndex);
				result.X = index % bufferWidth;
				result.Y = index / bufferWidth;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

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
		public static char GetPrintable(CharacterItem character)
		{
			char result = ' ';
			int value = 0;

			if(character != null)
			{
				value = ((int)character.Character);
				if(value == 9 || value == 10 || value == 13 ||
					(value > 31 && value < 127) || value > 128)
				{
					result = (char)value;
				}
			}
			return result;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return a printable version of the characters in the caller's string.
		/// </summary>
		/// <param name="text">
		/// The text to convert.
		/// </param>
		/// <returns>
		/// Printable version of the caller's string.
		/// </returns>
		public static string GetPrintable(string text)
		{
			StringBuilder builder = new StringBuilder();
			char[] chars = null;

			if(text?.Length > 0)
			{
				chars = text.ToCharArray();
				foreach(char charItem in chars)
				{
					builder.Append(GetPrintable(charItem));
				}
			}
			return builder.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetPropertyByName																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the value of the property specified by name from supplied
		/// object.
		/// </summary>
		/// <param name="item">
		/// Reference to the item for which the property will be retrieved.
		/// </param>
		/// <param name="propertyName">
		/// Name of the property to retrieve.
		/// </param>
		/// <returns>
		/// Reference to the specified property, if found. Otherwise, null.
		/// </returns>
		public static object GetPropertyByName(object item,
			string propertyName)
		{
			PropertyInfo[] properties = null;
			PropertyInfo property = null;
			object propertyValue = null;
			object result = null;

			if(item != null && propertyName?.Length > 0)
			{
				properties = item.GetType().GetProperties(
					BindingFlags.GetProperty |
					BindingFlags.Instance |
					BindingFlags.NonPublic |
					BindingFlags.Public);
				if(properties?.Length > 0)
				{
					//	Built-in property.
					property =
						properties.FirstOrDefault(x => x.Name == propertyName);
					if(property != null)
					{
						propertyValue = property.GetValue(item);
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetScrollBuffer																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the scroll buffer character.
		/// </summary>
		/// <returns>
		/// Unicode scroll buffer character.
		/// </returns>
		public static char GetScrollBuffer()
		{
			return '▒';
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetScrollPositioner																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the scroll positioning character.
		/// </summary>
		/// <returns>
		/// Unicode scroll positioning character.
		/// </returns>
		public static char GetScrollPositioner()
		{
			return '■';
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetValue																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the value of the specified group member in the provided match.
		/// </summary>
		/// <param name="match">
		/// Reference to the match to be inspected.
		/// </param>
		/// <param name="groupName">
		/// Name of the group for which the value will be found.
		/// </param>
		/// <returns>
		/// The value found in the specified group, if found. Otherwise, empty
		/// string.
		/// </returns>
		public static string GetValue(Match match, string groupName)
		{
			string result = "";

			if(match != null && match.Groups[groupName] != null &&
				match.Groups[groupName].Value != null)
			{
				result = match.Groups[groupName].Value;
			}
			return result;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the value of the specified group member in a match found with
		/// the provided source and pattern.
		/// </summary>
		/// <param name="source">
		/// Source string to search.
		/// </param>
		/// <param name="pattern">
		/// Regular expression pattern to apply.
		/// </param>
		/// <param name="groupName">
		/// Name of the group for which the value will be found.
		/// </param>
		/// <returns>
		/// The value found in the specified group, if found. Otherwise, empty
		/// string.
		/// </returns>
		public static string GetValue(string source, string pattern,
			string groupName)
		{
			Match match = null;
			string result = "";

			if(source?.Length > 0 && pattern?.Length > 0 && groupName?.Length > 0)
			{
				match = Regex.Match(source, pattern);
				if(match.Success)
				{
					result = GetValue(match, groupName);
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ResourceLock																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="ResourceLock">ResourceLock</see>.
		/// </summary>
		private static readonly object mResourceLock = new object();
		/// <summary>
		/// Get a reference to the thread-safe resource lock for this context.
		/// </summary>
		internal static object ResourceLock
		{
			get { return mResourceLock; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SetPropertyByName																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Set the value of the property specified by name on the supplied
		/// object.
		/// </summary>
		/// <param name="item">
		/// Reference to the item for which the property will be set.
		/// </param>
		/// <param name="propertyName">
		/// Name of the property to update.
		/// </param>
		/// <param name="propertyValue">
		/// Value to write to the target property.
		/// </param>
		/// <returns>
		/// True if the value of the property was successfully set. Otherwise,
		/// false.
		/// </returns>
		public static bool SetPropertyByName(object item,
			string propertyName, object propertyValue)
		{
			PropertyInfo[] properties = null;
			PropertyInfo property = null;
			bool result = false;

			if(item != null && propertyName?.Length > 0)
			{
				properties = item.GetType().GetProperties(
					BindingFlags.GetProperty |
					BindingFlags.Instance |
					BindingFlags.NonPublic |
					BindingFlags.Public);
				if(properties?.Length > 0)
				{
					//	Built-in property.
					property =
						properties.FirstOrDefault(x => x.Name.EndsWith(propertyName));
					if(property != null)
					{
						try
						{
							property.SetValue(item, propertyValue);
							result = true;
						}
						catch { }
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToInt																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Provide fail-safe conversion of string to numeric value.
		/// </summary>
		/// <param name="value">
		/// Value to convert.
		/// </param>
		/// <returns>
		/// Int32 value. 0 if not convertible.
		/// </returns>
		public static int ToInt(object value)
		{
			int result = 0;
			if(value != null)
			{
				result = ToInt(value.ToString());
			}
			return result;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Provide fail-safe conversion of string to numeric value.
		/// </summary>
		/// <param name="value">
		/// Value to convert.
		/// </param>
		/// <returns>
		/// Int32 value. 0 if not convertible.
		/// </returns>
		public static int ToInt(string value)
		{
			int result = 0;
			try
			{
				result = int.Parse(value);
			}
			catch { }
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* WordWrap																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Wrap the provided text to remain as much within the supplied window
		/// width as possible.
		/// </summary>
		/// <param name="text">
		/// The text to wrap.
		/// </param>
		/// <param name="maxWidth">
		/// The maximum width of the target window.
		/// </param>
		/// <param name="sliceWord">
		/// Value indicating whehter a word will be sliced if no other alternatives
		/// are available. Default = false.
		/// </param>
		/// <returns>
		/// The string representation of the caller's text, after wrapping.
		/// </returns>
		public static string WordWrap(string text, int maxWidth,
			bool sliceWord = false)
		{
			//StringBuilder builder = new StringBuilder();
			int colIndex = 0;
			int count = 0;
			int index = 0;
			string leftPart = "";
			TokenItem newToken = null;
			string result = "";
			string rightPart = "";
			string shortcutText = "";
			TokenItem token = null;
			TokenCollection tokens = null;
			string tokenText = "";
			TokenType tokenType = TokenType.None;
			string workingText = "";

			if(text?.Length > 0 && maxWidth > 0)
			{
				workingText = text.Replace("\r", "");
				tokens = TokenCollection.ParseWords(workingText);
				colIndex = 0;
				count = tokens.Count;
				for(index = 0; index < count; index++)
				{
					token = tokens[index];
					tokenText = token.Value;
					tokenType = token.TokenType;
					//	Assemble shortcut text.
					shortcutText = tokenText;
					if(index + 1 < count &&
						tokenType == TokenType.Text &&
						tokens[index + 1].TokenType == TokenType.Shortcut)
					{
						//	Next item is a shortcut.
						shortcutText += tokens[index + 1].Value;
						if(index + 2 < count &&
							tokens[index + 2].TokenType == TokenType.Text)
						{
							//	Following item is post-shortcut text.
							shortcutText += tokens[index + 2].Value;
						}
					}
					else if(index + 1 < count &&
						tokenType == TokenType.Shortcut &&
						tokens[index + 1].TokenType == TokenType.Text &&
						(index - 1 < 0 ||
						tokens[index - 1].TokenType != TokenType.Text))
					{
						//	Next item is text and this is not part of an earlier pattern.
						shortcutText += tokens[index + 1].Value;
					}
					if(tokenType == TokenType.Whitespace &&
						tokenText == "\n")
					{
						colIndex = 0;
					}
					else if(colIndex + shortcutText.Length >= maxWidth)
					{
						if(colIndex > 0)
						{
							//	Place the word on the next line.
							if(tokenType == TokenType.Whitespace)
							{
								token.Value = "\n";
							}
							else
							{
								newToken = new TokenItem()
								{
									TokenType = TokenType.Whitespace,
									Value = "\n"
								};
								tokens.Insert(index, newToken);
								count++;
							}
							colIndex = 0;
						}
						else if(sliceWord)
						{
							//	The cursor is currently located at column 0.
							if(tokenText.Length > 1)
							{
								leftPart = tokenText.Substring(0, maxWidth);
								rightPart = tokenText.Substring(maxWidth);
								token.Value = leftPart;
								index++;
								newToken = new TokenItem()
								{
									TokenType = TokenType.Text,
									Value = rightPart
								};
								tokens.Insert(index, newToken);
								newToken = new TokenItem()
								{
									TokenType = TokenType.Whitespace,
									Value = "\n"
								};
								tokens.Insert(index, newToken);
								//index++;
								count += 2;
								//colIndex = rightPart.Length;
								colIndex = 0;
							}
							else if(index + 1 >= count ||
								tokens[index + 1].TokenType != TokenType.Whitespace)
							{
								//	Single character not followed by a whitespace.
								newToken = new TokenItem()
								{
									TokenType = TokenType.Whitespace,
									Value = "\n"
								};
								index++;
								tokens.Insert(index, newToken);
								count++;
								colIndex = 0;
							}
							else
							{
								//	Consume a single character.
								colIndex = 1;
							}
						}
						else
						{
							//	Insert a line break at the end of this word.
							if(index + 1 < count)
							{
								index++;
								token = tokens[index];
								if(token.TokenType == TokenType.Whitespace)
								{
									token.Value = "\n";
								}
								else
								{
									newToken = new TokenItem()
									{
										TokenType = TokenType.Whitespace,
										Value = "\n"
									};
									tokens.Insert(index, newToken);
									count++;
								}
								colIndex = 0;
							}
						}
					}
					else
					{
						if(colIndex == 0 && tokenType == TokenType.Whitespace)
						{
							tokens.RemoveAt(index);
							index--;
							count--;
						}
						else
						{
							colIndex += tokenText.Length;
						}
					}
				}
			}
			if(tokens.Count > 0)
			{
				result = tokens.ToString();
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
