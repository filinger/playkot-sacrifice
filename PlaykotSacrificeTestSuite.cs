using System;
using System.Diagnostics;

class PlaykotSacrificeTestSuite {
    
    public static void Main() {
        RunBinaryConversionTests(PlaykotSacrifice.Binary.DecToBin);
        RunStringTrimTests(PlaykotSacrifice.Trim.TrimRepeated);
    }
    
    private static void RunBinaryConversionTests(ConversionMethod<int, string> decToBin) {
        Console.WriteLine("Testing {0}...", decToBin.Method.Name);
        ConversionTest(decToBin, 0, Convert.ToString(0, 2));
        ConversionTest(decToBin, 1, Convert.ToString(1, 2));
        ConversionTest(decToBin, 58, Convert.ToString(58, 2));
        ConversionTest(decToBin, -58, Convert.ToString(-58, 2));
        Console.WriteLine("{0} is OK.", decToBin.Method.Name);
    }
    
    private static void RunStringTrimTests(ConversionMethod<string, string> trimString) {
        Console.WriteLine("Testing {0}...", trimString.Method.Name);
        ConversionTest(trimString, "AAA BBB AAA", "A B A");
        ConversionTest(trimString, "ABC", "ABC");
        Console.WriteLine("{0} is OK.", trimString.Method.Name);
    }
    
    private static void ConversionTest<S, R>(ConversionMethod<S, R> method, S source, R expected) {
        R result = method.Invoke(source);
        if (result.Equals(expected)) {
            Console.WriteLine("OK: {0} -> {1}", source, result);
            return;
        }
        string message = String.Format("FAIL: {0} -> {1}, but expected {2}", source, result, expected);
        throw new Exception(message);
    }
    
    private delegate R ConversionMethod<S, R> (S source);
}
