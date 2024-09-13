class AstNode {
    get type() : AstNode | undefined {
        throw new Error("Abstract method called!");
    }
}

export class Application extends AstNode {
    func: AstNode;
    arg: AstNode;

    constructor(func: AstNode, arg: AstNode) {
        super();
        this.func = func;
        this.arg = arg;
    }
}

export class Abstraction extends AstNode {
    param: AstNode;
    body: AstNode;
    _type: AstNode | undefined;
    init: AstNode | undefined;

    get type() {
        return this._type;
    }

    constructor(param: AstNode, body: AstNode, type: AstNode | undefined, init: AstNode | undefined) {
        super();
        this.param = param;
        this.body = body;
        this._type = type;
        this.init = init;
    }
}

export class Variable extends AstNode {
    name: string;

    constructor(name: string) {
        super();
        this.name = name;
    }
}

export class Integer extends AstNode {
    value: string;

    constructor(value: string) {
        super();
        this.value = value;
    }
}
