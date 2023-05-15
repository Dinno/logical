# To-do list web app

## Domain description

```
(user : (
    id: Id; // Name implies that this is primary key
    firstName: String, length <= 30;
    secondName: String, length <= 50;
    password: String, length <= 40
) -> Boolean) : unique ["firstName", "secondName"];

todo : (
    todoId: Id, // Name implies that this is primary key
    userId: Id, // Name implies that this is foreign key for user table
    text: String,
    isDone: Boolean
) -> Boolean

```

## Display (React)

This code displays list of user's todo. Written in fictional LGX. HTML tags are also to some extent fictional:
```
todoList = userId =>
    data = query (todo _ userId $text $isDone); // Something like prolog???
    data |> map(
        item => <div><checkbox value={item.isDone}/>{item.text}</div>
    )
```
