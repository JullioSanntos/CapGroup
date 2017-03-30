using System;

namespace ACE.Client.Model.Helpers
{
    public class Syllable : IRandomizerType
    {

        private Random _randomUtil;

        private Random RandomUtil => _randomUtil ?? (_randomUtil = new Random());

        public string Next()
        {
            string syllable = string.Empty;
            // first letter
            var letterType = RandomUtil.Next(2); // 1 is Consonant, 0 is vowel
            for (int i = 0; i < 2; i++)
            {
                if (letterType == 1) syllable += Randomizer.ConsonantValues[RandomUtil.Next(Randomizer.ConsonantValues.Count)];
                else syllable += Randomizer.VowelValues[RandomUtil.Next(Randomizer.VowelValues.Count)];
                letterType = letterType == 1 ? 2 : 1; //reverse the letter type
            }
 
            return syllable;
        }
    }
}
