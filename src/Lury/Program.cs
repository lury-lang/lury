using System;
using System.Collections.Generic;

namespace NotepadLangCS
{
    class Program
    {
        public static void Main()
        {
            Parser parser = new Parser();

            /*
            string input = @"abstract class Person:
	# protected
	prop trueAge [get, private set]
	abstract prop age [get]

	# public
	def me(age):
		me.trueAge = age

	def tellAge => ""私は\(me.age)歳です。""

class HonestPerson extends Person:
	protected prop age => me.trueAge
 
class Liar extends Person:
	protected prop age => me.trueAge but if $ > 40 then $ / 2

// プログラムはここからスタート
var mike = new HonestPerson(20) // 正直者
var ken = new Liar(20)          // 嘘つき

/* 表示を行う
 * マイクは正直者、ケンは嘘つき
 *//*
println ""mike: "" ~ mike.tellAge
println ""ken: "" ~ ken.tellAge";
            */

            string input2 = "abstract class Person:\n" +
                            "\tdef hoge:\n\n" +
                            "\t\t1+1\n\n" +
                            "";
            //"\tabstract prop age [get]";

            Lexer lexer = new Lexer(input2);
            object result = parser.yyparse(lexer);
            Console.WriteLine(result);
        }
    }

    class SyntaxNode
    {
        public string Token { get; private set; }

        public List<SyntaxNode> Children { get; private set; }

        public SyntaxNode(string token)
        {
            this.Token = token;
            this.Children = new List<SyntaxNode>();
        }
    }
}

