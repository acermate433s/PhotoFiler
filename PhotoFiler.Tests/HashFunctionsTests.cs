using Photo.Hasher;
using Photo.Models;
using System.Linq;
using Xunit;

namespace PhotoFiler.Tests
{
    public class HashFunctionsTests
    {
        const string SYMBOLS = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string CLEAR_TEXT = "the quick brown fox jumps over the lazy dog";

        private void HashFunctionCompute_HashLengthMustBeCorrect(IHashFunction hashFunction, string clearText, int hashLength)
        {
            var cipherText = hashFunction.Compute(clearText);

            Assert.True(cipherText.Length == hashLength);
        }

        [Theory]
        [InlineData(CLEAR_TEXT, 5)]
        [InlineData(CLEAR_TEXT, 6)]
        [InlineData(CLEAR_TEXT, 7)]
        [InlineData(SYMBOLS, 5)]
        [InlineData(SYMBOLS, 6)]
        [InlineData(SYMBOLS, 7)]
        public void Md5Compute_HashLengthMustBeCorrect(string clearText, int hashLength)
        {
            HashFunctionCompute_HashLengthMustBeCorrect(
                new MD5(hashLength),
                clearText,
                hashLength);
        }

        [Theory]
        [InlineData(CLEAR_TEXT, 5)]
        [InlineData(CLEAR_TEXT, 6)]
        [InlineData(CLEAR_TEXT, 7)]
        [InlineData(SYMBOLS, 5)]
        [InlineData(SYMBOLS, 6)]
        [InlineData(SYMBOLS, 7)]
        public void Sha512Compute_HashLengthMustBeCorrect(string clearText, int hashLength)
        {
            HashFunctionCompute_HashLengthMustBeCorrect(
                new SHA512(hashLength),
                clearText,
                hashLength);
        }

        [Theory]
        [InlineData(CLEAR_TEXT, 5)]
        [InlineData(CLEAR_TEXT, 6)]
        [InlineData(CLEAR_TEXT, 7)]
        [InlineData(SYMBOLS, 5)]
        [InlineData(SYMBOLS, 6)]
        [InlineData(SYMBOLS, 7)]
        public void RipEmd160Compute_HashLengthMustBeCorrect(string clearText, int hashLength)
        {
            HashFunctionCompute_HashLengthMustBeCorrect(
                new RIPEMD160(hashLength),
                clearText,
                hashLength);
        }

        private void HashFunctionCompute_HashCharactersAreValid(IHashFunction hashFunction, string clearText, int hashLength)
        {
            var cipherText = hashFunction.Compute(clearText);

            Assert.True(cipherText.All(item => SYMBOLS.Contains(item)));
        }

        [Theory]
        [InlineData(CLEAR_TEXT, 5)]
        [InlineData(CLEAR_TEXT, 6)]
        [InlineData(CLEAR_TEXT, 7)]
        [InlineData(SYMBOLS, 5)]
        [InlineData(SYMBOLS, 6)]
        [InlineData(SYMBOLS, 7)]
        public void Md5Compute_HashCharactersAreValid(string clearText, int hashLength)
        {
            HashFunctionCompute_HashCharactersAreValid(
                new MD5(hashLength), 
                clearText, 
                hashLength);
        }
        [Theory]
        [InlineData(CLEAR_TEXT, 5)]
        [InlineData(CLEAR_TEXT, 6)]
        [InlineData(CLEAR_TEXT, 7)]
        [InlineData(SYMBOLS, 5)]
        [InlineData(SYMBOLS, 6)]
        [InlineData(SYMBOLS, 7)]
        public void Sha512Compute_HashCharactersAreValid(string clearText, int hashLength)
        {
            HashFunctionCompute_HashCharactersAreValid(
                new SHA512(hashLength),
                clearText,
                hashLength);
        }

        [Theory]
        [InlineData(CLEAR_TEXT, 5)]
        [InlineData(CLEAR_TEXT, 6)]
        [InlineData(CLEAR_TEXT, 7)]
        [InlineData(SYMBOLS, 5)]
        [InlineData(SYMBOLS, 6)]
        [InlineData(SYMBOLS, 7)]
        public void RipEmd160Compute_HashCharactersAreValid(string clearText, int hashLength)
        {
            HashFunctionCompute_HashCharactersAreValid(
                new RIPEMD160(hashLength),
                clearText,
                hashLength);
        }

    }
}
