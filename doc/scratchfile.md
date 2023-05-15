# Model hierarchy levels
* Math
* Common world models
* Domain models
* Application model
* Application layer/component models

# Model layers example

```
dataset 
    -> DB 
        -> migration
        -> queries
    -> API
        -> implementation
    -> client model
    -> interface
```

# (?) Example of DB table model 
```
table: String -> Array Column -> Table;
int: Type;

table "user"
[
    column ("id", int),
]
```

# Basic definitions
```
List: T -> {
    empty: List T;
    list: T -> List T -> List T;
};

List: TYPE -> TYPE;
empty: T: TYPE -> List T;
list: T: TYPE -> T -> List T -> List T;
@destructor List$destructor: T: TYPE -> P: TYPE -> P -> (T -> List T -> P) -> (List T -> P);

Tuple: TYPE -> List TYPE -> TYPE;
null

```

# Site description example

## Application Model

```
User = TYPE
    id
    name: String,
    age: Integer,
    Boolean;
// Analogous to: User = String => Integer => Boolean;

todo:

```

Expression: a => a a
Should it be interpreted as: (a => a) a 
or as: a => (a a)
Answer: a => (a a)

Expression: a a => a
Is interpreted as: a (a => a)

Expression: a => a a => a
Should it be interpreted as: a => a (a => a) 
or as: (a => a) (a => a)
Answer: a => a (a => a)
