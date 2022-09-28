import { Abstraction, Application, Data, Variable } from "../formalTree";
import { parse } from "../generated/parser";

test('variable', () => {
    expect(parse("a", {})).toStrictEqual(new Variable("a"));
    expect(parse("_", {})).toStrictEqual(new Variable("_"));
    expect(parse("a1", {})).toStrictEqual(new Variable("a1"));
    expect(parse("a_", {})).toStrictEqual(new Variable("a_"));
    expect(parse("__", {})).toStrictEqual(new Variable("__"));
    expect(parse("_1", {})).toStrictEqual(new Variable("_1"));
    expect(parse("_1234567890", {})).toStrictEqual(new Variable("_1234567890"));
});

test('abstraction', () => {
    expect(parse("a:Integer=>23", {})).toStrictEqual(
        new Abstraction(
            'a',
            new Variable ('Integer'),
            new Application (
              new Variable('integer' ),
              new Data('23')
            )
        )
    );
});