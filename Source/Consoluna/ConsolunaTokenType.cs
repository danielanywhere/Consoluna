using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolunaLib
{
	//*-------------------------------------------------------------------------*
	//*	ConsolunaTokenType																											*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Enumeration of the recognized token types.
	/// </summary>
	public enum ConsolunaTokenType
	{
		/// <summary>
		/// No token type specified or unknown.
		/// </summary>
		None,
		/// <summary>
		/// Text value.
		/// </summary>
		Text,
		/// <summary>
		/// Whitespace.
		/// </summary>
		Whitespace,
		/// <summary>
		/// Punctuation character.
		/// </summary>
		Punctuation
	}
	//*-------------------------------------------------------------------------*

}
