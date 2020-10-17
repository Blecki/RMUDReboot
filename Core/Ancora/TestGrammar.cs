using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;

namespace Ancora
{
    public class TestGrammar : Grammar
    {
        public TestGrammar()
        {
            DefaultParserFlags = ParserFlags.IGNORE_LEADING_WHITESPACE | ParserFlags.IGNORE_TRAILING_WHITESPACE;
            
            var delimeters = "&%^|<>=,/-+*[]{}() \t\r\n.:;\"@";
            var digits = "0123456789";
            var id = Identifier(c => !digits.Contains(c) && !delimeters.Contains(c), c => !delimeters.Contains(c)).CreateAst("IDENT");
            var numberLiteral = Token(c => digits.Contains(c)).CreateAst("NUMBERLIT");

            #region Setup Operator Table

            var opTable = new OperatorTable();

            opTable.AddOperator(".", 5);
            opTable.AddOperator("@", 5);

            opTable.AddOperator("&&", 1);
            opTable.AddOperator("||", 1);
            opTable.AddOperator("==", 1);
            opTable.AddOperator("!=", 1);
            opTable.AddOperator(">", 1);
            opTable.AddOperator("<", 1);
            opTable.AddOperator("<=", 1);
            opTable.AddOperator(">=", 1);

            opTable.AddOperator("+", 2);
            opTable.AddOperator("-", 2);

            opTable.AddOperator("*", 3);
            opTable.AddOperator("/", 3);
            opTable.AddOperator("%", 3);

            opTable.AddOperator("&", 4);
            opTable.AddOperator("|", 4);
            opTable.AddOperator("^", 4);

            #endregion

            var statement = LateBound();
            var expression = LateBound();

            #region Type Names
            var typeName = LateBound();
            var typeIdentifier = (id.Value() + Maybe(('<' + DelimitedList(typeName, ',').Flatten() + '>').CreateAst("GENERICARGS"))).CreateAst("TYPENAMETOKEN");
            typeName.SetSubParser(DelimitedList(typeIdentifier, '.').CreateAst("TYPENAME"));
            var typeSpecifier = Character(':') + typeName.PassThrough();
            #endregion

            #region Strings
            var embeddedExpression = ('[' + expression + ']').CreateAst("EMBEDDED");
            var text = Token(c => c != '\\' && c != '\"' && c != '[').CreateAst("TEXT");
            var escapeSequence = ('\\' + (Character('n') | '\\' | '\"')).CreateAst("ESCAPE");
            var _string = ('\"' + NoneOrMany(escapeSequence | text).Flatten() + '\"').CreateAst("STRING");
            var complexString = (Character('@') + '\"' + NoneOrMany(escapeSequence | embeddedExpression | text).Flatten() + '\"').CreateAst("COMPLEXSTRING");
            #endregion

            #region UnitTypes
            var unit = (Token(c => c >= 'a' && c <= 'z').Value() 
                + Maybe(('^' + numberLiteral.Value()).CreateAst("POWER"))).CreateAst("UNIT");
            var unitSet = OneOrMany(unit).CreateAst("UNITSET");
            var unitType = (unitSet + Maybe('/' + unitSet.PassThrough())).CreateAst("UNITTYPE");
            var number = (numberLiteral.Value() + Maybe(unitType)).CreateAst("NUMBER");
            #endregion

            #region Blocks and Bodies

            var block = (Character('{') + NoneOrMany(statement).PassThrough() + '}').CreateAst("BLOCK");
            var body = block | statement;
            var functionBlock = body + Maybe(Keyword("error" + body)).CreateAst("ERROR") + Maybe(Keyword("post" + body)).CreateAst("POST");

            #endregion

            #region Declarations

            var ruleModifier = id;
            var whenClause = Keyword("when") + functionBlock;
            var ruleName = OneOrMany(id);
            var ruleDeclaration = Maybe(ruleModifier) + ruleName + '(' + Maybe(DelimitedList(id + typeSpecifier, ',')) + ')' + typeSpecifier +
                Maybe(whenClause) + functionBlock;
            var rule = Keyword("rule") + ruleDeclaration;

            var functionDeclaration = (id.CreateAst("NAME") + '(' + Maybe(DelimitedList((id + typeSpecifier).CreateAst("ARG"), ',').Flatten()) + ')' + typeSpecifier.CreateAst("RETURNTYPE") + functionBlock.CreateAst("BODY")).CreateAst("FUNC");
            var methodDeclaration = Keyword("method") + functionDeclaration;
            var fieldDeclaration = Keyword("var") + id + typeSpecifier + Maybe(expression) + ';';
            var memberDeclaration = fieldDeclaration | methodDeclaration | rule;
            var typeDeclaration = Keyword("type") + typeIdentifier + Maybe(typeSpecifier) + '{' + NoneOrMany(memberDeclaration) + '}';
            var declarationTerm = id | ( '(' + id + typeSpecifier + ')' );
            var macro = Keyword("macro") + id + OneOrMany(declarationTerm) + Maybe(typeSpecifier) + functionBlock;
            var function = Keyword("func") + HardError(functionDeclaration).PassThrough();
            var _namespace = Keyword("namespace") + typeIdentifier + '{' + NoneOrMany(typeDeclaration) + '}';

            var headerUsing = (Keyword("using") + typeName.Value() + ';').CreateAst("USING");
            var headerBlock = NoneOrMany(headerUsing);

            var file = (headerBlock.Flatten() + NoneOrMany(_namespace | typeDeclaration | function | macro | rule).Flatten()).CreateAst("ROOT");

            #endregion
                        
            #region Terms and Operators
            var term = LateBound();
            var op = Operator(opTable).CreateAst("OP");
            expression.SetSubParser(Expression(term, op, opTable));
            var parenthetical = '(' + expression + ')';
            var argList = ('(' + Maybe(DelimitedList(expression, Character(','))).PassThrough() + ')').Flatten();
            var invokation = id + OneOrMany(term | id | argList) + (functionBlock | ';');
            var cast = Keyword(">>>") + typeName;
            var indexer = Character('@') + term;
            var memberAccess = Character('.') + id;
            var bracketInvokation = '[' + id + OneOrMany(term | id | argList) + Maybe(functionBlock) + ']';
            var list = '{' + Maybe(DelimitedList(term, ',')) + '}';
            var memberInitializer = Keyword("let") + id + "=" + expression + ";";
            var memberInitializerBlock = '{' + NoneOrMany(memberInitializer) + '}';
            var _new = Keyword("new") + typeName + (memberInitializerBlock | ';');
            term.SetSubParser(((id.PassThrough() | number.PassThrough() | _new.PassThrough() | _string.PassThrough() | complexString.PassThrough() | parenthetical.PassThrough() | bracketInvokation.PassThrough() | list.PassThrough()) + NoneOrMany(argList | indexer | cast | memberAccess).Flatten()).CreateAst("TERM"));
            #endregion

            #region Statements
            var local = (Keyword("var") + id.Value() + typeSpecifier + '=' + expression + ';').CreateAst("LOCAL");
            var let = (Keyword("let") + term + '=' + expression + ';').CreateAst("LET");
            var _if = (Keyword("if") + parenthetical.CreateAst("CONDITION") + functionBlock.CreateAst("THEN") + Maybe(Keyword("else") + functionBlock.CreateAst("ELSE").PassThrough())).CreateAst("IF");
            var loop = (Keyword("loop") + Maybe(id.CreateAst("LABEL")) + functionBlock).CreateAst("LOOP");
            var _break = (Keyword("break") + Maybe(id.Value()) + ';').CreateAst("BREAK");
            var _continue = (Keyword("continue") + Maybe(id.Value()) + ';').CreateAst("CONTINUE");
            var _return = (Keyword("return") + Maybe(expression.CreateAst("VALUE")) + ';').CreateAst("RETURN");
            statement.SetSubParser(_if | loop | local | let | _break | _continue | _return | invokation);
            #endregion

            this.Root = file;
        }
    }
}
