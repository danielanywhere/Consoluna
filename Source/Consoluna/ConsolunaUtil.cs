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
			int bufferHeight, ConsolunaPosition position)
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
		public static ConsolunaPosition GetLegalPosition(int bufferWidth,
			int bufferHeight, int columnIndex, int rowIndex)
		{
			int index = 0;
			ConsolunaPosition result = null;

			if(bufferWidth > 0 && bufferHeight > 0)
			{
				result = new ConsolunaPosition();
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
		public static char GetPrintable(ConsolunaCharacterItem character)
		{
			char result = ' ';
			int value = 0;

			if(character != null)
			{
				value = ((int)character.Character) & 0xff;
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
						properties.FirstOrDefault(x => x.Name.EndsWith(propertyName));
					if(property != null)
					{
						propertyValue = property.GetValue(item);
						if(propertyValue != null)
						{
							result = propertyValue.ToString();
						}
					}
				}
			}
			return result;
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
			ConsolunaTokenItem newToken = null;
			string result = "";
			string rightPart = "";
			string shortcutText = "";
			ConsolunaTokenItem token = null;
			ConsolunaTokenCollection tokens = null;
			string tokenText = "";
			ConsolunaTokenType tokenType = ConsolunaTokenType.None;
			string workingText = "";

			if(text?.Length > 0 && maxWidth > 0)
			{
				workingText = text.Replace("\r", "");
				tokens = ConsolunaTokenCollection.ParseWords(workingText);
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
						tokenType == ConsolunaTokenType.Text &&
						tokens[index + 1].TokenType == ConsolunaTokenType.Shortcut)
					{
						//	Next item is a shortcut.
						shortcutText += tokens[index + 1].Value;
						if(index + 2 < count &&
							tokens[index + 2].TokenType == ConsolunaTokenType.Text)
						{
							//	Following item is post-shortcut text.
							shortcutText += tokens[index + 2].Value;
						}
					}
					else if(index + 1 < count &&
						tokenType == ConsolunaTokenType.Shortcut &&
						tokens[index + 1].TokenType == ConsolunaTokenType.Text &&
						(index - 1 < 0 ||
						tokens[index - 1].TokenType != ConsolunaTokenType.Text))
					{
						//	Next item is text and this is not part of an earlier pattern.
						shortcutText += tokens[index + 1].Value;
					}
					if(tokenType == ConsolunaTokenType.Whitespace &&
						tokenText == "\n")
					{
						colIndex = 0;
					}
					else if(colIndex + shortcutText.Length >= maxWidth)
					{
						if(colIndex > 0)
						{
							//	Place the word on the next line.
							if(tokenType == ConsolunaTokenType.Whitespace)
							{
								token.Value = "\n";
							}
							else
							{
								newToken = new ConsolunaTokenItem()
								{
									TokenType = ConsolunaTokenType.Whitespace,
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
								newToken = new ConsolunaTokenItem()
								{
									TokenType = ConsolunaTokenType.Text,
									Value = rightPart
								};
								tokens.Insert(index, newToken);
								newToken = new ConsolunaTokenItem()
								{
									TokenType = ConsolunaTokenType.Whitespace,
									Value = "\n"
								};
								tokens.Insert(index, newToken);
								//index++;
								count += 2;
								//colIndex = rightPart.Length;
								colIndex = 0;
							}
							else if(index + 1 >= count ||
								tokens[index + 1].TokenType != ConsolunaTokenType.Whitespace)
							{
								//	Single character not followed by a whitespace.
								newToken = new ConsolunaTokenItem()
								{
									TokenType = ConsolunaTokenType.Whitespace,
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
								if(token.TokenType == ConsolunaTokenType.Whitespace)
								{
									token.Value = "\n";
								}
								else
								{
									newToken = new ConsolunaTokenItem()
									{
										TokenType = ConsolunaTokenType.Whitespace,
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
						if(colIndex == 0 && tokenType == ConsolunaTokenType.Whitespace)
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
