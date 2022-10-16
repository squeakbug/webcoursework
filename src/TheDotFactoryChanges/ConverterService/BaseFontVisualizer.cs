using System;
using System.Collections.Generic;
using System.Drawing;

using DataAccessInterface;

namespace Service
{
    public class BaseFontVisualizer : BaseVisualizer
    {
        private List<string> DivideByDistance(string chars, Configuration cfg)
        {
            char previousCharacter = '\0';

            var collectionList = new List<string>();
            var curCollection = String.Empty;
            int differenceBetweenCharsForNewGroup = cfg.generateLookupBlocks ?
                    cfg.lookupBlocksNewAfterCharCount : int.MaxValue;

            foreach (var ch in chars)
            {
                if (ch - previousCharacter < differenceBetweenCharsForNewGroup && previousCharacter != '\0')
                {
                    for (char sequentialCharIndex = (char)(previousCharacter + 1);
                            sequentialCharIndex < ch;
                            ++sequentialCharIndex)
                    {
                        curCollection += ch;
                    }
                }
                else
                {
                    collectionList.Add(curCollection);
                    curCollection = String.Empty;
                }

                previousCharacter = ch;
            }
            if (curCollection.Length > 0)
                collectionList.Add(curCollection);
            return collectionList;
        }

        protected static string GetCharactersToGenerate(string input, bool isPeg, Configuration cfg)
        {
            input = Utils.ExpandAndRemoveCharacterRanges(input);

            if (isPeg == true)
            {
                return input.Replace("\n", "").Replace("\r", "");
            }

            SortedList<char, char> characterList = new SortedList<char, char>();
            for (int charIndex = 0; charIndex < input.Length; ++charIndex)
            {
                char ch = input[charIndex];
                if (!characterList.ContainsKey(ch))
                {
                    if (ch == ' ' && !cfg.generateSpaceCharacterBitmap)
                        continue;
                    if (ch == '\n' || ch == '\r')
                        continue;
                    characterList.Add(input[charIndex], ' ');
                }
            }

            string result = "";
            foreach (char characterKey in characterList.Keys)
                result += characterKey;
            return result;
        }
        protected GlypthCollection PopulateFontInfo(System.Drawing.Font font, string template, Configuration cfg, bool isPeg,
            ITextRenderer textRenderer)
        {
            if (cfg == null)
                throw new Exception("Configuration wasn't selected");

            string chars = GetCharactersToGenerate(template, isPeg, cfg);
            var minSize = new Size(cfg.spaceGenerationPixels, cfg.minHeight);
            var collection = new GlypthCollection(chars, font, minSize, textRenderer);
            return collection;
        }
    }
}
