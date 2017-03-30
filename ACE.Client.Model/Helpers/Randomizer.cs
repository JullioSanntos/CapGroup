using System;
using System.Collections.Generic;
using System.Linq;

namespace ACE.Client.Model.Helpers
{
    public class Randomizer 
    {

        #region properties

        #region Random Lists
        private static List<char> _alphabeticValues;

        public static List<char> AlphabeticValues
        {
            get
            {
                if (_alphabeticValues != null) return _alphabeticValues;

                _alphabeticValues = new List<char>();
                for (var i = 65; i < 90; i++)
                {
                    _alphabeticValues.Add((char)i);
                }

                return _alphabeticValues;
            }
        }

        private static List<char> _numericValues;

        public static List<char> NumericValues
        {
            get
            {
                if (_numericValues != null) return _numericValues;

                _numericValues = new List<char>();
                for (var i = 48; i < 58; i++)
                {
                    _numericValues.Add((char)i);
                }

                return _numericValues;
            }
        }

        private static List<char> _consonantValues;
        public static List<char> ConsonantValues
        {
            get
            {
                if (_consonantValues != null) return _consonantValues;

                _consonantValues = AlphabeticValues.Except(VowelValues).ToList();

                return _consonantValues;
            }
        }
        public static List<char> VowelValues => new List<char>() { 'A', 'E', 'I', 'O', 'U', 'Y' };
        #endregion

        private List<char> _randomValues;

        public List<char> RandomValues
        {
            get { return _randomValues; }
            set { _randomValues = value; }
        }

        public int MaxLength { get; private set; }

        public int MinLength { get; private set; }

        private readonly Random _randomUtil;
        private readonly IRandomizerType _randomizerObject;
        #endregion

        public Randomizer()
        {
            _randomUtil = new Random();
        }
        public Randomizer(List<char> randomValues, int minLength, int? maxLength = null)
        {
            _randomValues = randomValues;
            SetLenght(maxLength, minLength);
            _randomUtil = new Random();
        }
        public Randomizer(IRandomizerType ramdomizerObject, int minLength, int? maxLength = null )
        {
            _randomizerObject = ramdomizerObject;
            _randomUtil = new Random();
            SetLenght(maxLength, minLength);
        }


        public string Next()
        {
            var randomStr = string.Empty;
            int iterations;
            if (MinLength == MaxLength) iterations = MaxLength; 
            else iterations = this._randomUtil.Next(MinLength, MaxLength);
            for (int i = 0; i < iterations; i++)
            {
                if (RandomValues != null)
                {
                    var ix = _randomUtil.Next(RandomValues.Count());
                    randomStr += RandomValues[ix];
                }
                if (_randomizerObject != null)
                {
                    randomStr += _randomizerObject.Next();
                }
            }
            
            return randomStr;
        }

        public string Next(List<char> randomValues, int minLength, int? maxLength = null)
        {
            SetLenght(maxLength, minLength);

            var randomStr = string.Empty;
            int iterations;
            if (MinLength == MaxLength) iterations = MaxLength;
            else iterations = this._randomUtil.Next(MinLength, MaxLength);
            for (int i = 0; i < iterations; i++)
            {
                var ix = _randomUtil.Next(randomValues.Count() - 1);
                randomStr += randomValues[ix];
            }

            return randomStr;
        }

        public string Next(IRandomizerType randomizerObject, int minLength, int? maxLength = null)
        {
            SetLenght(maxLength, minLength);

            var randomStr = string.Empty;
            int iterations;
            if (MinLength == MaxLength) iterations = MaxLength;
            else iterations = this._randomUtil.Next(MinLength, MaxLength);
            for (int i = 0; i < iterations; i++)
            {
                randomStr += randomizerObject.Next();
            }

            return randomStr;
        }
        public void SetLenght(int? maxLength, int minLength)
        {
            if (maxLength == null ) maxLength = minLength;
            if (maxLength.Value < minLength || maxLength <= 0) throw new Exception("Invalid MinLength, MaxLength value(s)");
            this.MaxLength = (int)maxLength;
            this.MinLength = minLength;
        }
    }
}
