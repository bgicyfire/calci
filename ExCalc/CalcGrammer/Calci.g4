grammar Calci;
/*
 * Parser Rules
 */

 seq                : line* EOF ;
 line               : varb ASSIGN_OP add_sub NEWLINE; 
 varb               : VARB ;
 num                : NUMBB ;
 add_sub            : add_sub ADD_SUB_OP mul_div | mul_div;
 mul_div            : mul_div MUL_DIV_OP atom | atom;
 atom               : NUMBB | VARB | '(' add_sub ')';

 /* 
 * Lexer Rules
 */

fragment LOWERCASE  : [a-z] ;
fragment UPPERCASE  : [A-Z] ;
fragment NUM        : [0-9] ;

VARB                : (LOWERCASE | UPPERCASE)+ (LOWERCASE | UPPERCASE | NUM | [_])*;
NEWLINE             : ('\r'? '\n' | '\r')+ ;
NUMBB               : (NUM)+;
ASSIGN_OP            : '=' | '+=' | '-=' | '*=' | '/=';
ADD_SUB_OP          : '+' | '-';
MUL_DIV_OP          : '*' | '/';