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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using static ConsolunaLib.ConsolunaUtil;

namespace ConsolunaLib
{
	//*-------------------------------------------------------------------------*
	//*	ConsolunaTokenCollection																								*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of ConsolunaTokenItem Items.
	/// </summary>
	public class ConsolunaTokenCollection : List<ConsolunaTokenItem>
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
		//* ParseWords																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Parse the caller's text into a collection of words.
		/// </summary>
		/// <param name="text">
		/// The text to parse.
		/// </param>
		/// <returns>
		/// Reference to a collection of word tokens.
		/// </returns>
		public static ConsolunaTokenCollection ParseWords(string text)
		{
			bool bFound = false;
			MatchCollection matches = null;
			int matchIndex = 0;
			string matchValue = "";
			ConsolunaTokenCollection result = new ConsolunaTokenCollection();
			List<(string memberName, ConsolunaTokenType tokenType)> tests =
				new List<(string memberName, ConsolunaTokenType tokenType)>()
			{
					("text", ConsolunaTokenType.Text),
					("whitespace", ConsolunaTokenType.Whitespace),
					("punctuation", ConsolunaTokenType.Punctuation),
					("shortcut", ConsolunaTokenType.Shortcut)
			};

			if(text?.Length > 0)
			{
				matches = Regex.Matches(text, ResourceMain.rxWordToken);
				foreach(Match matchItem in matches)
				{
					foreach((string memberName, ConsolunaTokenType tokenType) testItem
						in tests)
					{
						matchValue = GetValue(matchItem, testItem.memberName);
						if(matchValue.Length > 0)
						{
							matchIndex = GetIndex(matchItem, testItem.memberName);
							result.Add(new ConsolunaTokenItem()
							{
								Index = matchIndex,
								TokenType = testItem.tokenType,
								Value = matchValue
							});
							break;
						}
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToString																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the string representation of this collection.
		/// </summary>
		/// <returns>
		/// The string representation of the contents of this collection.
		/// </returns>
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();

			foreach(ConsolunaTokenItem tokenItem in this)
			{
				if(tokenItem.TokenType == ConsolunaTokenType.Shortcut)
				{
					builder.Append('_');
				}
				builder.Append(tokenItem.Value);
			}
			return builder.ToString();
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	ConsolunaTokenItem																											*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Information about a single token in a sequence.
	/// </summary>
	public class ConsolunaTokenItem
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
		//*	Index																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Index">Index</see>.
		/// </summary>
		private int mIndex = 0;
		/// <summary>
		/// Get/Set the index of the word.
		/// </summary>
		public int Index
		{
			get { return mIndex; }
			set { mIndex = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	TokenType																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="TokenType">TokenType</see>.
		/// </summary>
		private ConsolunaTokenType mTokenType = ConsolunaTokenType.None;
		/// <summary>
		/// Get/Set the token type for this item.
		/// </summary>
		public ConsolunaTokenType TokenType
		{
			get { return mTokenType; }
			set { mTokenType = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToString																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the string representation of this item.
		/// </summary>
		/// <returns>
		/// The string representation of the contents of this item.
		/// </returns>
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();

			if(mTokenType == ConsolunaTokenType.Shortcut)
			{
				builder.Append('_');
			}
			builder.Append(mValue);
			return builder.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Value																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Value">Value</see>.
		/// </summary>
		private string mValue = "";
		/// <summary>
		/// Get/Set the value of the token.
		/// </summary>
		public string Value
		{
			get { return mValue; }
			set { mValue = value; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
