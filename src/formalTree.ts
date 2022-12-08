class Expression {
    getType() {
        throw new Error("Abstract method called!");
    }
}

export class Kind extends Expression {
    constructor() {
        super();
    }
}

export class Application extends Expression {
    func: Expression;
    arg: Expression;

    constructor(func: Expression, arg: Expression) {
        super();
        this.func = func;
        this.arg = arg;
    }
}

export class Abstraction extends Expression {
    param: string;
    body: Expression;

    constructor(param: string, body: Expression) {
        super();
        this.param = param;
        this.body = body;
    }
}

export class Variable extends Expression {
    name: string;

    constructor(name: string) {
        super();
        this.name = name;
    }
}

export class Data extends Expression {
    data: string;

    constructor(data: string) {
        super();
        this.data = data;
    }
}
