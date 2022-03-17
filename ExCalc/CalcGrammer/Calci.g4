grammar Calci;
/*
 * Parser Rules
 */

 seq                : line* EOF ;
 line               : VARB ASSIGN_OP add_sub NEWLINE; 
 add_sub            : add_sub ADD_SUB_OP mul_div | mul_div;
 mul_div            : mul_div MUL_DIV_OP increment | increment;
 increment          : post_increment | pre_increment | post_decrement | pre_decrement | atom;
 post_increment     : VARB '++';
 pre_increment      : '++' VARB;
 post_decrement     : VARB '--';
 pre_decrement      : '--' VARB;
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
WHITESPACE          : (' '|'\t')+ -> skip ;