# To-do list web app

## Domain description

```

User = {
    id: Id; // Name implies that this is primary key
    firstName: String, constrain(length <= 30);
    secondName: String, constrain(length <= 50);
    password: String, constrain(length <= 40);
 } : unique ["firstName", "secondName"];

Todo = {
    todoId: Id; // Name implies that this is primary key
    userId: Id, foreign(User.id); // Name already implies that this is foreign key for user table. But for demonstration purposes we will explicitly specify it.
    text: String;
    isDone: Boolean;
};
```

## Display (React)

This code displays list of user's todo. Written in fictional LGX. HTML tags are also to some extent fictional:
```
user: User -> Boolean;
todo: Todo -> Boolean;

todoList = userId =>
    data = query (todo _ userId $text $isDone); // Something like prolog???
    data |> map(
        item => <div><checkbox value={item.isDone}/>{item.text}</div>
    )
```
