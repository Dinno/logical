import peg from "peggy";
import fs from "fs";


//console.log(process.cwd());
const data = fs.readFileSync("src/logical.pegjs").toString();


const parser = peg.generate(data, {output: "source", format: "es"});

fs.writeFileSync("src/generated/parser.js", parser);

import { parse } from "./generated/parser"

console.log(parse(`
x y
`, {}));