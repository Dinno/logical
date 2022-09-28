import { Application, Data, Variable } from "./formalTree"

export const createInteger = (value: string) => new Application(new Variable("integer"), new Data(value));