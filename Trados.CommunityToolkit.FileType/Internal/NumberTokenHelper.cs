using System.Collections.Generic;
using System.Linq;

namespace Trados.Community.Toolkit.FileType.Internal
{
    internal class NumberTokenHelper
    {

        public static bool IsValidFloating(string text, params char[] separators)
        {
            var tokens = text.Trim().ToCharArray().Select(chr => new NumberToken
            {
                value = chr,
                type = char.IsNumber(chr)
                    ? NumberToken.TokenType.Number
                    : (IsNumberStyleSeparator(chr, separators)
                    ? NumberToken.TokenType.Separator
                    : NumberToken.TokenType.Invalid)
            }).ToList();

            if (tokens.Count == 0)
                return false;

            // check for invalid tokens            
            if (tokens.Any(a => a.type == NumberToken.TokenType.Invalid))
                return false;

            // The first and last char must qualify as a number
            if (tokens[0].type != NumberToken.TokenType.Number
                || tokens[tokens.Count - 1].type != NumberToken.TokenType.Number)
                return false;


            SetNumberValueTypes(tokens);

            // check for invalid tokens            
            if (tokens.Any(a => a.type == NumberToken.TokenType.Invalid))
                return false;
            return true;
        }

        /// <summary>
        /// Check if the character is qualified as a thousand or decimal separator.
        /// </summary>
        /// <param name="chr">character to check</param>
        /// <returns>returns true if the character is qualified as a thousand or decimal separator</returns>
        private static bool IsNumberStyleSeparator(char chr, params char[] separators)
        {
            if (separators.Length == 0)
            {
                return chr == '.' || chr == ',';
            }
            else
            {
                return separators.Contains(chr);
            }
        }

        /// <summary>
        /// Set and verify if the numeric character separators are valid.
        /// </summary>
        /// <param name="tokens"></param>
        private static void SetNumberValueTypes(List<NumberToken> tokens)
        {
            var previousSeparatorTokenIndex = -1;
            for (var i = tokens.Count - 1; i >= 0; i--)
            {
                if (tokens[i].type == NumberToken.TokenType.Separator)
                {
                    if (previousSeparatorTokenIndex == -1)
                    {
                        // start by assuming that the last separator is always a decimal separator.
                        // It is possible that this is not true, but can only be verified as you iterate
                        // further down the stack.
                        tokens[i].type = NumberToken.TokenType.DecimalSeparator;
                    }
                    else
                    {
                        // get the previous separator token
                        var previousSeparatorToken = tokens[previousSeparatorTokenIndex];

                        if (previousSeparatorToken.type != NumberToken.TokenType.Invalid)
                        {
                            if (previousSeparatorToken.value != tokens[i].value
                                || (previousSeparatorToken.value == tokens[i].value
                                    && previousSeparatorToken.type == NumberToken.TokenType.ThousandSeparator))
                            {
                                // if the char values are different, then we can start assuming that
                                // the thousand char is used.
                                tokens[i].type = NumberToken.TokenType.ThousandSeparator;
                            }
                            else if (previousSeparatorToken.value == tokens[i].value
                                     && previousSeparatorToken.type == NumberToken.TokenType.DecimalSeparator)
                            {
                                // we can assume that the numbers are only using thousand separators from here onwards.
                                // this means that we can automatically change the previous token type from decimal 
                                // separator to thousand seperator.
                                tokens[i].type = NumberToken.TokenType.ThousandSeparator;
                                previousSeparatorToken.type = NumberToken.TokenType.ThousandSeparator;

                                // check for 3 digits exist between the thousand separators
                                if (tokens.Count - previousSeparatorTokenIndex != 4)
                                    tokens[previousSeparatorTokenIndex].type = NumberToken.TokenType.Invalid;

                            }
                            else
                            {
                                // should be all thousand separators from here onwards; If this is not the 
                                // true, then set as invalid
                                tokens[i].type = NumberToken.TokenType.Invalid;
                            }
                        }

                        // check for connected separator chars
                        if (tokens[i].type != NumberToken.TokenType.Invalid
                            && previousSeparatorTokenIndex == (i + 1))
                        {
                            tokens[i].type = NumberToken.TokenType.Invalid;
                        }


                        // check for 3 digits exist between the thousand separators
                        if (tokens[i].type == NumberToken.TokenType.ThousandSeparator
                            && previousSeparatorTokenIndex > -1)
                        {
                            if (previousSeparatorTokenIndex - i != 4)
                                tokens[i].type = NumberToken.TokenType.Invalid;
                        }
                    }
                    previousSeparatorTokenIndex = i;
                }
            }
        }
    }
}
