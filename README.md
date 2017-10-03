# SimpleAssemblerInterpreter
Для получения зачёта необходимо было сделать парсер и интерпретатор простого ассемблера. Собственно, это он, пусть тут полежит

Описание простого ассемблера:
* 123 - добавляет 123 в стек
* gg - добавляет значение переменной в стек
* set gg - удаляет значение из стека, сохраняет в переменную gg
* if gg - удаляет значение из стека, если оно было true, то переходим по метке gg, если false, то переходим к следующей инструкции, иначе ошибка
* pop - удаляет значение из стека
* a b c 2 call - все 4 элемента удаляются со стека, вызывается c(a, b), результат добавляется на стек
* gg: объявляет метку gg
* goto gg - переходит к метке gg
* +, -, *, /, <, >, <=, >=, ==, != - бинарные операторы, со стека удаляется значение b, со стека удаляется значение a, значение a @ b добавляется в стек, где @ - любой из бинарных операторов, работают так же, как в лабе 4.

Пример программы, печатающей числа от 0 до 9 (комменты не парсятся, нужно убирать):
10 set max        //max = 0;
0 set i           //i = 0;
loop_start: 
i max >= if stop  // while (i < max) {
i print 1 call    //   print(i);
pop
i 1 + set i       //   i = i + 1;
goto loop_start   // }
stop: